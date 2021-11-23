using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;

namespace System
{
    public static class ConfigReader
    {
        public static void Reader()
        {
            Dictionary<string, string> configData = new Dictionary<string, string>();

            // Read all the keys from the config file
            NameValueCollection sAll;
            sAll = ConfigurationManager.AppSettings;

            foreach (string s in sAll.AllKeys)
            {
                configData.Add(s, sAll.Get(s));
                //Console.WriteLine("Key: " + s + " Value: " + sAll.Get(s));
            }

            Console.WriteLine("Dictionary");
            foreach (KeyValuePair<string, string> entry in configData)
            {
                Console.WriteLine("Key: " + entry.Key + " Value: " + entry.Value);
            }
            Console.ReadLine();
        }
    }
}
