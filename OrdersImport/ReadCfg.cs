using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;





namespace Rest02
{
    public static class ReadCfg
    {
        public static string CfgFileName = "app_cfg.xml";
        public static string FileType = "csv";
        
        public static string OutPath = "d:";
        public static Boolean error_flag = false;
        
        public static Boolean send_mail_on_error = true;
        public static Boolean send_mail_on_success = false;
        public static Boolean show_addon_messages = false;
        public static string send_mail_on_error_to = "";
        public static string mail_message = "";

        public static IEnumerable<ConnectionNode> nodes_list;
        public static cfg_mail cfm; 


        public static void ProcXml()
        {
            // XDocument 
            XDocument xml = XDocument.Load(CfgFileName);

            // Query the data and write out a subset of contacts
            try
            {
                 nodes_list = from c in xml.Root.Descendants("dbconn")
                            select new ConnectionNode()
                            {
                                label = (string)c.Element("label").Value,
                                addr = (string)c.Element("addr").Value,
                                dbname = (string)c.Element("dbname").Value,
                                user = (string)c.Element("user").Value,
                                pass = (string)c.Element("pass").Value,
                                //tmp_tname = (string)c.Element("tmp_tname").Value,
                                orders = new List<Order>(),
                                codepage = (int)Int32.Parse(c.Element("codepage").Value),
                                
                                
                                query_id = (string)c.Element("query_id").Value,
                                query_01 = (string)c.Element("query_01").Value.Replace("query_id", (string)c.Element("query_id").Value),
                                query_02 = (string)c.Element("query_02").Value,
                                conn_str = "Server=" + (string)c.Element("addr").Value + ";" +
                                           "Database=" + (string)c.Element("dbname").Value + ";" +
                                           "user=" + (string)c.Element("user").Value + ";" +
                                           "password='" + (string)c.Element("pass").Value + "';" +
                                           "port=3306;Encrypt=true;default command timeout=30;",
                               last_order_id = "0"
                            };

                 if (String.Compare(xml.Root.Element("send_mail_on_error").Value.ToString(), "false", true) == 0) send_mail_on_error = false;
                 if (String.Compare(xml.Root.Element("send_mail_on_success").Value.ToString(), "true", true) == 0) send_mail_on_success = true;
                if (String.Compare(xml.Root.Element("show_addon_messages").Value.ToString(), "true", true) == 0) show_addon_messages = true;
                send_mail_on_error_to = xml.Root.Element("send_mail_on_error_to").Value.ToString();
                 OutPath = xml.Root.Element("csv_out_dir").Value.ToString();
                 //csv_source_ffn = xd.Root.Element("csv_source_ffn").Value.ToString();
                 foreach (var ms in xml.Descendants(xml.Root.Element("use_mail_node").Value.ToString()))
                 {
                     cfm = new cfg_mail()
                     {
                         pop = (string)ms.Element("pop").Value,
                         smtp = (string)ms.Element("smtp").Value,
                         pop_port = Convert.ToInt32(ms.Element("pop_port").Value.ToString()),
                         smtp_port = Convert.ToInt32(ms.Element("smtp_port").Value.ToString()),
                         user = (string)ms.Element("user").Value,
                         pass = (string)ms.Element("pass").Value,
                         enablessl = Convert.ToBoolean(ms.Element("enablessl").Value.ToString())
                     };
                 }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                error_flag = true;
                mail_message +="\n XML read error {0} Exception caught."+e;
            //    Console.ReadKey();
            }
        }


        public static void mess(string s)
        {
            if (show_addon_messages) MessageBox.Show(s); //Console.WriteLine(s); //
        }
    }



    public class ConnectionNode
    {
        public string label { get; set; }
        public string addr { get; set; }
        public string dbname { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
        public string file_prefix { get; set; }

        public string conn_str { get; set; }
        public string query_id { get; set; }
        public string query_01 { get; set; }
        public string query_02 { get; set; }
        public int codepage { get; set; }
        public string last_order_id { get; set; }

        public List<Order> orders;

        public void idset(string s) { this.last_order_id = s; }

        //public IEnumerable<Order> orders;
    }

    public class Order
    {
        public string order_id { get; set; }
        public string num { get; set; }
        public string date { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string firm { get; set; }
        public string phone { get; set; }
        public string paytype { get; set; }
        public string summ { get; set; }
        public string discount { get; set; }
        public string shopsite { get; set; }
        public string valute { get; set; }
        public int codepage { get; set; }
        public string order_file_name { get; set; }

        public string csv_text { get; set; }
        public List<Order_row> order_rows;
        public DateTime dtDateTime;

 
        //public IEnumerable<Order_row> order_rows;
        void SaveToCsv() { }
    }

    public class Order_row
    {
        public string id { get; set; }
        public string art { get; set; }
        public string name { get; set; }
        public string qtty { get; set; }
        public string sum { get; set; }
        public string file_prefix { get; set; }
    }

    public class cfg_mail
    {
        public string pop { get; set; }
        public string smtp { get; set; }
        public int pop_port { get; set; }
        public int smtp_port { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
        public Boolean enablessl { get; set; }
    };


}
