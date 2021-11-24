using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;

namespace SystemCore
{
    public static class ConfigReader
    {
        private static Dictionary<string, string> _configData;

        private static void Initialize()
        {
            // Initialize once, on initialized skip re-reading the config data.
            if (ConfigReader._configData != null && ConfigReader._configData.Count != 0)
            {
                return;
            }
            
            ConfigReader._configData = new Dictionary<string, string>();

            // Read all the keys from the config file
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            foreach (string setting in appSettings.AllKeys)
            {
                ConfigReader._configData.Add(setting, appSettings.Get(setting));
            }
        }

        public static string GetSetting(string setting)
        {
            ConfigReader.Initialize();

            return ConfigReader._configData.TryGetValue(setting, out string value).ToString();
        }
    }
}
