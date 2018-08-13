using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Rest02
{
    public static class SerializerAddon
    {
        public static List<Dictionary<string, string>> DicList(string sstr, string in_str)
        {
            string find_text="";
            int dic_count=1;

            string stmp;
            string text="";

            List<string> ls = new List<string>();

            switch (sstr)
            {
                case "person": find_text = "s:6:\"Person\";a:";
                    dic_count = 1;
                    text = in_str.Remove(0, in_str.IndexOf(find_text));
                    text = text.Remove(text.IndexOf("}"), text.Length - text.IndexOf("}")-1);

                    break;
                case "cart": find_text = ":{s:4:\"cart\";a:"; 

                    stmp = in_str.Remove(0, in_str.IndexOf(find_text)+find_text.Length);
                    stmp = stmp.Remove(stmp.IndexOf(":"), stmp.Length - stmp.IndexOf(":"));
                    dic_count = Int32.Parse(stmp);

                    text = in_str.Remove(0, in_str.IndexOf(find_text) + find_text.Length);
                    text = text.Remove(0,text.IndexOf("{")+1);
                    text = text.Remove(text.IndexOf("}s:3:\"num\";"), text.Length - text.IndexOf("}s:3:\"num\";"));
                    break;
            }


            // Add strings to List of strings
            for(int i=0;i<dic_count;i++)
            {
                text = text.Remove(text.IndexOf("{"), text.IndexOf("{") + 1);
                stmp = text.Remove(text.IndexOf("}"),text.Length- text.IndexOf("}") );
                stmp = stmp.Replace(";a:", ";i:");
                ls.Add(stmp);
                text = text.Remove(0,text.IndexOf("}")+1);
            }

            List<Dictionary<string, string>> dic = new List<Dictionary<string, string>>();
            string isKey = "key";
            string k = "", v = "";
            string stmp01 = "";
            string list_str = "";
            object obj;
            Serializer ss = new Serializer();
            Hashtable dd = new Hashtable();

            foreach (string aaa in ls)
            {
                list_str = aaa;
                Dictionary<string, string> dic_soon = new Dictionary<string, string>();
                do
                {
                    obj = ss.Deserialize(list_str);
                    if (obj == null) list_str = list_str.Remove(0, in_str.IndexOf(';') + 1);

                    if (obj.GetType() == typeof(string))
                    {

                        stmp01 = (string)obj;
                        if (stmp01 == null) stmp01 = "";

                        switch (isKey)
                        {
                            case "key": k = stmp01; isKey = "val";  break;
                            case "val": v = stmp01; isKey = "key";                              
                                dic_soon.Add(k,v); 
                                break;
                        }
                   
                        list_str = list_str.Remove(0, list_str.IndexOf(';') + 1);
                    }
                    else Console.Write("\nERROR: OBJECT TYPE is {0}\n", obj.GetType());

                } while (list_str != "");
                dic.Add(dic_soon);
            }
            return dic;
        }
    }
}
