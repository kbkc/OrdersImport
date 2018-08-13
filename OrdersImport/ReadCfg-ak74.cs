using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;




namespace Rest02
{
    public static class ReadCfg
    {
        public static string FileType = "csv";
        public static string OutPath = "d:";
        public static Boolean error_flag = false;
        
        public static Boolean send_mail_on_error = true;
        public static Boolean send_mail_on_success = false;
        public static string send_mail_on_error_to = "";

        public static IEnumerable<ConnectionNode> nodes_list;
        public static cfg_mail cfm; 


        public static void ProcXml()
        {
            // XDocument 
            var xml = XDocument.Load(@"app_cfg.xml");

            // Query the data and write out a subset of contacts

            try
            {
                var query = from c in xml.Root.Descendants("dbconn")

                            // where (int)c.Attribute("id") < 4
                            //"user:"+c.Element("user").Value + " \n query text: " + c.Element("query_list").Value + "";
                            select new ConnectionNode()
                            {
                                label = (string)c.Element("label").Value,
                                addr = (string)c.Element("addr").Value,
                                dbname = (string)c.Element("dbname").Value,
                                user = (string)c.Element("user").Value,
                                pass = (string)c.Element("pass").Value,
                                //tmp_tname = (string)c.Element("tmp_tname").Value,
                                query_id = (string)c.Element("query_id").Value,
                                query_list = (string)c.Element("query_list").Value,
                                conn_str = "Server=" + (string)c.Element("addr").Value + ";" +
                                           "Database=" + (string)c.Element("dbname").Value + ";" +
                                           "user=" + (string)c.Element("user").Value + ";" +
                                           "password='" + (string)c.Element("pass").Value + "';" +
                                           "port=3306;Encrypt=true;default command timeout=30;"
                                         
                                 
                            };




                //foreach (string name in query)
                //{
                //    Console.WriteLine(" ----- \n {0}\n ---- \n", name);
                //    Console.ReadKey();
                //}

            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                Console.ReadKey();
            }



        }

    }
    abstract public class ConnectionNode
    {
        public string label { get; set; }
        public string addr { get; set; }
        public string dbname { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
        public string file_prefix { get; set; }

        public string conn_str { get; set; }
        public string query_id { get; set; }
        public string query_list { get; set; }
        //private string	ConnStr, SqlStrDoc,	SqlStrTable,LastRecordNum,PartDoc,PartTable,;
        public abstract void SqlProc();
        void CreateCsv() { }
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
