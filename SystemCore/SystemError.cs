using System;
using System.Windows;

namespace SystemCore
{
    /// <summary>
    /// Provides a class for handling exceptions and errors.
    /// </summary>
    public static class SystemError
    {
        /// <summary>
        /// Handles an exception, either shows the error or a user friendly error message and stops running the program.
        /// </summary>
        /// <param name="exception">The exception</param>
        public static void Handle(Exception exception)
        {
            // Do nothing with the exception if we do not have a loaded config. This is fine because a non-existing 
            // config breaks the application anyway.
            if (string.IsNullOrEmpty(ConfigReader.GetSetting("debug")))
            {
                return;
            }
            
            var success = bool.TryParse(ConfigReader.GetSetting("debug"), out bool debug);
            if (success && debug)
            {
                throw exception;
            }

            MessageBox.Show(
                "Er ging iets fout tijdens het uitvoeren van deze applictie, probeer het alstublieft opnieuw of neem contact op met de beheerders.", 
                "Foutmelding",
                MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.None
            );
            Environment.Exit(-1);
        }
    }
}
