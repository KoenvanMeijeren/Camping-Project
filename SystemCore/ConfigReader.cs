using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;

namespace SystemCore
{
    /// <summary>
    /// Provides a class for reading data from the application config.
    /// </summary>
    public static class ConfigReader
    {
        private static Dictionary<string, string> _configData;

        /// <summary>
        /// Initializes the config data.
        /// </summary>
        /// <footer>
        /// Reads the data of the config file and stores it in the config data property.
        /// This is done once, when the config is initialized, re-reading the config data is skipped.
        /// </footer>
        private static void Initialize()
        {
            if (ConfigReader._configData != null && ConfigReader._configData.Count != 0)
            {
                return;
            }
            
            ConfigReader._configData = new Dictionary<string, string>();

            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            foreach (string setting in appSettings.AllKeys)
            {
                ConfigReader._configData.Add(setting, appSettings.Get(setting));
            }
        }

        /// <summary>
        /// Gets a specific value of a config key.
        /// </summary>
        /// <param name="setting">The key of the value</param>
        /// <returns>The value of the key</returns>
        public static string GetSetting(string setting)
        {
            ConfigReader.Initialize();

            ConfigReader._configData.TryGetValue(setting, out string value);

            return value;
        }
    }
}
