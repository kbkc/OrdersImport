using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Xml.Linq;

namespace Rest02
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello!\n Wait please...");

            // ---- Read cfg file --- //
            ReadCfg.ProcXml();

            Dictionary<string, string> idl = new Dictionary<string, string>();
            string k = "";
            string v = "";

            /*
                        public static DateTime UnixTimeStampToDateTime( double unixTimeStamp )
            {
                // Unix timestamp is seconds past epoch
                System.DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
                return dtDateTime;
            }

             */

            //System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

            //dtDateTime = dtDateTime.AddSeconds(1406643957).ToLocalTime();//1406550535 1406643957

            //Console.WriteLine(dtDateTime.ToString());
            //Console.ReadKey();

            //return;

            foreach (ConnectionNode aa in ReadCfg.nodes_list)
            {
                try
                {
                    Console.WriteLine("------- " + aa.label + " -------");

                    // main block
                    dbProc.MysqlConn(aa);

                    // write order to .csv
                    if (aa.orders.Count() > 0)
                    {
                        foreach (Order ord in aa.orders)
                        {
                            File.WriteAllText(ord.order_file_name, ord.csv_text, Encoding.GetEncoding(ord.codepage)); //utf8 65001,  koi 866
                            Console.WriteLine("Order #" + ord.order_id + " done.\n");
                            k = aa.label;
                            v = ord.order_id;
                        }
                        idl.Add((string)k, (string)v);
                    }
                    else Console.WriteLine("orders :" + aa.orders.Count().ToString());
                    // end  write order to .csv
                }
                catch (Exception ex)
                {
                    ReadCfg.error_flag = true;
                    ReadCfg.mail_message += "* * * \n Connection Exception error:\n " + ex.ToString() + "\n" + aa.conn_str.ToString() + "\n * * *\n";
                }
            }
            Console.WriteLine("\n");
            Console.WriteLine(("").PadRight(35, '-'));
            // ---------------- WRITE XML ----------------//
            // -------------  replace id -------------- //
            if (idl.Count() > 0)
            {
                XDocument xml = XDocument.Load(ReadCfg.CfgFileName);

                foreach (KeyValuePair<string, string> kv in idl)
                {
                    Console.WriteLine(kv.Key + ":" + kv.Value);

                    var aaa = from c in xml.Root.Descendants("dbconn")
                              where c.Element("label").Value.Contains(kv.Key)
                              select c;
                    foreach (var cc in aaa) { cc.Element("query_id").Value = kv.Value; }
                }
                xml.Save(ReadCfg.CfgFileName);
            }
            // ---------- end of  replace id ----------- //

            // -------------------------------- MAIL SEND BLOCK -------------------------------------------

            if ((ReadCfg.send_mail_on_error && ReadCfg.error_flag) || (ReadCfg.send_mail_on_success && !ReadCfg.error_flag))
            {
                // Console.WriteLine("\n\n sending mail.....");
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(ReadCfg.cfm.user);
                    mail.To.Add(new MailAddress(ReadCfg.send_mail_on_error_to));
                    mail.Subject = "-- Orders export errors";

                    mail.Body = ReadCfg.mail_message;
                    SmtpClient client = new SmtpClient();
                    client.Host = ReadCfg.cfm.smtp;
                    client.Port = ReadCfg.cfm.smtp_port;
                    client.EnableSsl = ReadCfg.cfm.enablessl;
                    client.Credentials = new NetworkCredential(ReadCfg.cfm.user, ReadCfg.cfm.pass);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;

                    if ((ReadCfg.send_mail_on_success && !ReadCfg.error_flag))
                    {
                        mail.Subject = "--- Orders export success";
                        client.Send(mail);
                    }
                    if ((ReadCfg.send_mail_on_error && ReadCfg.error_flag))
                    {
                        client.Send(mail);
                    }
                    client.Timeout = 20000;
                    mail.Dispose();
                }
                catch (Exception e)
                {
                    throw new Exception("Mail.Send: " + e.Message);
                }
                //  Console.WriteLine("\n\n mail sent.");
            } // ----------------------------------------------------------- END MAIL SEND BLOCK

            Console.WriteLine("...Bye");
            // Console.ReadKey();
        }
    }
}