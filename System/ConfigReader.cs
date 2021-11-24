using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;

namespace SystemCore
{
    public static class ConfigReader
    {
        private static Dictionary<string, string> ConfigData;

        private static void Initialize()
        {
            // Initialize once, on initialized skip re-reading the config data.
            if (ConfigReader.ConfigData != null && ConfigReader.ConfigData.Count != 0)
            {
                return;
            }
            
            ConfigReader.ConfigData = new Dictionary<string, string>();

            // Read all the keys from the config file
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            foreach (string setting in appSettings.AllKeys)
            {
                ConfigReader.ConfigData.Add(setting, appSettings.Get(setting));
            }
        }

        public static string GetSetting(string setting)
        {
            ConfigReader.Initialize();

            return ConfigReader.ConfigData.TryGetValue(setting, out string value).ToString();
        }
    }
}
