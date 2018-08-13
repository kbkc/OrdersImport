using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.IO;
using System.Windows.Forms;



namespace Rest02
{
    public static class dbProc
    {

        public static void MysqlConn(ConnectionNode aa)
        {
            MySqlConnection conn = new MySqlConnection(aa.conn_str.ToString());
            try
            {
                conn.Open();
                ReadCfg.mail_message += "\n\n * * *  " + aa.label + "\nConnection opened";
                dbProceed(conn, aa, 1);
                conn.Close();
                ReadCfg.mail_message += "\nConnection closed ";//conn_str.ToString()
            }
            catch (Exception ex)
            {
                ReadCfg.error_flag = true;
                ReadCfg.mail_message += "\n\n* * *  " + aa.label + "connection Exception error:\n " + ex.ToString() + "\n" + aa.conn_str.ToString() + "\n * * *\n";
            }
        }




        public static void dbProceed(MySqlConnection cn, ConnectionNode c, int qtype)
        {
            MySqlCommand cmd;
            MySqlDataReader myReader;

            try
            {
                //****************************************************************************************
                //
                //                                   JOOMSOPPING 
                //
                //****************************************************************************************
                if (c.label == "ReformaUA")
                {

                    ////////////////////////////////////////////
                    // --------- FIll  ORDER HEAD DATA ------ //
                    cmd = new MySqlCommand(c.query_01, cn);
                    myReader = cmd.ExecuteReader();
                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            var ord = new Order
                            {
                                order_id = myReader.GetString(0),
                                num = myReader.GetString(1),
                                date = myReader.GetString(21),
                                email = myReader.GetString(43),
                                name = myReader.GetString(60) + " " + myReader.GetString(61),
                                firm = myReader.GetString(38),
                                phone = myReader.GetString(72),
                                paytype = myReader.GetString(8),
                                summ = myReader.GetString(3),
                                discount = myReader.GetString(9),
                                shopsite = myReader.GetString(88),
                                valute = myReader.GetString(17),
                                codepage = c.codepage,
                                order_rows = new List<Order_row>()
                            };
                            if (c.orders != null)
                            {
                                c.orders.Add(ord);
                            }
                        }
                        myReader.Close();
                        //--------- END FIll ORDER HEAD DATA ------ //
                        /////////////////////////////////////////////
                        // ---------- FIll  ORDER TABLE DATA ------- //
                        foreach (var ee in c.orders)
                        {
                            cmd.CommandText = c.query_02 + ee.order_id;
                            myReader = cmd.ExecuteReader();
                            if (myReader.HasRows)
                                while (myReader.Read())
                                {
                                    var ord_list = new Order_row
                                    {
                                        art = myReader.GetString(3),
                                        name = myReader.GetString(4),
                                        qtty = myReader.GetString(5),
                                        sum = myReader.GetString(6)
                                    };
                                    if (ee.order_rows != null)
                                        ee.order_rows.Add(ord_list);
                                    // Console.WriteLine("\n---  ---\n" + ord_list.art + ";" + ord_list.id + ";" + ord_list.name + ";" + ord_list.qtty + ";" + ord_list.sum);
                                }
                            myReader.Close();
                        }
                        // --------- END FIll ORDER TABLE DATA ------ //
                        ////////////////////////////////////////////////
                        ReadCfg.mail_message += "\n orders written to list";
                    }
                    else
                    {
                        Console.WriteLine(" no orders");
                        ReadCfg.mail_message += "\n no orders now";
                    }
                }
                //****************************************************************************************
                //
                //                                   OPENCART NFU-RU 
                //
                //****************************************************************************************
                else if (c.label == "NfuRuOC")
                {

                    ////////////////////////////////////////////
                    // --------- FIll  ORDER HEAD DATA ------ //
                    //Console.WriteLine("\n\n\n {0}\n\n\n ", c.query_01);
                    //Console.WriteLine("\n\n\n {0}\n\n\n ", c.query_02);
                    //Console.ReadLine();
                    cmd = new MySqlCommand(c.query_01, cn);
                    myReader = cmd.ExecuteReader();
                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            var ord = new Order
                            {
                                order_id = myReader.GetString(0),
                                num = myReader.GetString(1),
                                date = myReader.GetString(2),
                                email = myReader.GetString(3),
                                name = myReader.GetString(4),
                                firm = myReader.GetString(5),
                                phone = myReader.GetString(6),
                                paytype = myReader.GetString(7),
                                summ = myReader.GetString(8),
                                discount = myReader.GetString(9),
                                shopsite = myReader.GetString(10),
                                valute = myReader.GetString(11),
                                codepage = c.codepage,
                                order_rows = new List<Order_row>()
                            };
                            if (c.orders != null)
                            {
                                c.orders.Add(ord);
                            }
                        }
                        myReader.Close();
                        //--------- END FIll ORDER HEAD DATA ------ //
                        /////////////////////////////////////////////
                        // ---------- FIll  ORDER TABLE DATA ------- //
                        foreach (var ee in c.orders)
                        {
                            cmd.CommandText = c.query_02 + ee.order_id;
                            myReader = cmd.ExecuteReader();
                            if (myReader.HasRows)
                                while (myReader.Read())
                                {
                                    var ord_list = new Order_row
                                    {
                                        art = myReader.GetString(0),
                                        name = myReader.GetString(1),
                                        qtty = myReader.GetString(2),
                                        sum = myReader.GetString(3)
                                    };
                                    if (ee.order_rows != null)
                                        ee.order_rows.Add(ord_list);

                                }
                            myReader.Close();
                        }
                        // --------- END FIll ORDER TABLE DATA ------ //
                        ////////////////////////////////////////////////
                        ReadCfg.mail_message += "\n orders written to list";
                    }
                    else
                    {
                        Console.WriteLine(" no orders");
                        ReadCfg.mail_message += "\n no orders now";
                    }

                }




                //****************************************************************************************
                //
                //                                   SHOPPER.PL 
                //
                //****************************************************************************************
                else if (c.label == "ReformaPl" || c.label == "ReformaCom")
                {

                    ////////////////////////////////////////////
                    // --------- FIll  ORDER HEAD DATA ------ //
                    cmd = new MySqlCommand(c.query_01, cn);
                    myReader = cmd.ExecuteReader();
                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            var ord = new Order
                            {
                                order_id = myReader.GetString(0),
                                num = myReader.GetString(0),
                                date = myReader.GetString(1),
                                email = myReader.GetString(2),
                                name = myReader.GetString(3),
                                firm = myReader.GetString(4),
                                phone = myReader.GetString(5),
                                paytype = myReader.GetString(6),
                                summ = myReader.GetString(7),
                                discount = myReader.GetString(8),
                                shopsite = myReader.GetString(9),
                                valute = myReader.GetString(10),
                                codepage = c.codepage,
                                order_rows = new List<Order_row>()
                            };
                            if (c.orders != null)
                            {
                                c.orders.Add(ord);
                            }
                        }
                        myReader.Close();
                        //--------- END FIll ORDER HEAD DATA ------ //
                        /////////////////////////////////////////////
                        // ---------- FIll  ORDER TABLE DATA ------- //
                        foreach (var ee in c.orders)
                        {
                            cmd.CommandText = c.query_02 + ee.order_id;
                            myReader = cmd.ExecuteReader();
                            if (myReader.HasRows)
                                while (myReader.Read())
                                {
                                    var ord_list = new Order_row
                                    {
                                        art = myReader.GetString(0),
                                        name = myReader.GetString(1),
                                        qtty = myReader.GetString(2),
                                        sum = myReader.GetString(3)
                                    };
                                    if (ee.order_rows != null)
                                        ee.order_rows.Add(ord_list);

                                }
                            myReader.Close();
                        }
                        // --------- END FIll ORDER TABLE DATA ------ //
                        ////////////////////////////////////////////////
                        ReadCfg.mail_message += "\n orders written to list";
                    }
                    else
                    {
                        Console.WriteLine(" no orders");
                        ReadCfg.mail_message += "\n no orders now";
                    }

                }

                //****************************************************************************************
                //
                //                                   PHPSHOP 
                //
                //****************************************************************************************
                else if (c.label == "NfuRu" || c.label == "NfuUa")
                {
                    ////////////////////////////////////////////
                    // --------- FIll  DATA ------ //
                    cmd = new MySqlCommand(c.query_01, cn);
                    myReader = cmd.ExecuteReader();
                    if (myReader.HasRows)
                    {

                        while (myReader.Read())
                        {
                            var ord = new Order
                            {
                                order_id = myReader.GetString(0),
                                date = myReader.GetString(1),
                                num = myReader.GetString(2),
                                csv_text = myReader.GetString(3),
                                codepage = c.codepage,
                                dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc),


                                order_rows = new List<Order_row>()
                            };

                            if (c.orders != null)
                            {
                                ord.dtDateTime = ord.dtDateTime.AddSeconds(Double.Parse(ord.date)).ToLocalTime();
                                ord.date = ord.dtDateTime.ToString();
                                c.orders.Add(ord);

                            }
                        }
                        myReader.Close();
                        // --------- END FIll  DATA ------ //
                        ///////////////////////////////////////////////  
                        foreach (Order s in c.orders)
                        {
                            List<Dictionary<string, string>> dic_p = new List<Dictionary<string, string>>();
                            List<Dictionary<string, string>> dic_c = new List<Dictionary<string, string>>();
                            dic_c = SerializerAddon.DicList("cart", s.csv_text);
                            dic_p = SerializerAddon.DicList("person", s.csv_text);
                            foreach (Dictionary<string, string> dicp in dic_p)
                            {
                                s.email = dicp["mail"].ToString();
                                s.name = dicp["name_person"].ToString();
                                s.firm = dicp["org_name"].ToString();
                                s.phone = dicp["tel_name"].ToString();
                                s.paytype = dicp["order_metod"].ToString();
                                s.summ = "0";
                                s.discount = "unk";
                                s.shopsite = c.label;
                                s.valute = "unk";

                                foreach (Dictionary<string, string> dicc in dic_c)
                                {
                                    var ord_list = new Order_row
                                    {
                                        art = dicc["uid"].ToString(),
                                        name = dicc["name"].ToString(),
                                        qtty = dicc["num"].ToString(),
                                        sum = dicc["price"].ToString()
                                    };
                                    if (s.order_rows != null) s.order_rows.Add(ord_list);
                                }
                            }
                        }
                        ReadCfg.mail_message += "\n orders written to list";
                    }
                    else
                    {
                        Console.WriteLine(" no orders");
                        ReadCfg.mail_message += "\n no orders now";
                    }

                }// ----------------- END NFU.RU --------------------- //


                ////////////////////////////////////////////////////////
                // ------ CREATE CSV TEXT ------- //
                foreach (Order ss in c.orders)
                {

                    ss.order_file_name = ffn_return(ReadCfg.OutPath, ss.shopsite + "_" + ss.num + "." + ReadCfg.FileType);
                  //  ReadCfg.mess(ss.order_file_name);
                    ss.csv_text = "N Зак.;Дата;e-mail;Имя;Фирма;тел.;оплата;сумма;скидка;Сайт;Валюта\n"
                        + ss.num + ";" + ss.date + ";" + ss.email + ";" + ss.name + ";" + ss.firm + ";" + ss.phone
                        + ";" + ss.paytype + ";" + ss.summ + ";" + ss.discount + ";" + ss.shopsite + ";" + ss.valute + "\n"
                        //   + "Валюта;Курс;;;;;;;\n"
                        //   + ss.valute + ";1\n"
                        //   + "Начало заказанных товаров;;;;;;;;\n"
                        + "id;Арт.;Наименование;Кол.;Сумма;;;;;;\n";
                    if (ss.order_rows != null)
                    {
                        int i = 1;
                        foreach (Order_row dd in ss.order_rows)
                        {
                            ss.csv_text += i.ToString() + ";" + dd.art + ";" + dd.name + ";" + dd.qtty + ";" + dd.sum + ";;;;;;\n";
                            i++;
                        }
                    }
                }
                // ---- END CREATE CSV TEXT ----- //
                ////////////////////////////////////
            } // end try
            catch (MySqlException ex)
            {
                ReadCfg.error_flag = true;
                //    Console.WriteLine(("").PadRight(68, '-'));
                //    Console.WriteLine("Exception error:\n {0}", ex.ToString());
                //    Console.WriteLine(("").PadRight(68, '-'));
                ReadCfg.mail_message += "* * * \n DB Exception error:\n " + ex.ToString() + "\n" + c.query_01 + "\n" + c.query_02 + "\n * * *\n";
            }


        }
        public static string ffn_return(string fpath, string fname)
        {
            string divider = "\\";

           // if(fpath[0]=='\\')

            return fpath + divider + fname;
        }
    }
}


