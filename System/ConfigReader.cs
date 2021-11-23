using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;

namespace SystemCore
{
    public static class ConfigReader
    {
        private static Dictionary<string, string> ConfigData;

        private static void Initalize()
        {
            // Initalize once, then return initalized value.
            if (ConfigData == null || ConfigData.Count == 0)
            {

                ConfigData = new Dictionary<string, string>();

                // Read all the keys from the config file
                NameValueCollection appSettings = ConfigurationManager.AppSettings;

                foreach (string setting in appSettings.AllKeys)
                {
                    ConfigData.Add(setting, appSettings.Get(setting));
                }
            }
        }


        public static string GetSetting(string setting)
        {
            Initalize();

            string value; 
            return ConfigData.TryGetValue(setting, out value).ToString();
        }
    }
}
