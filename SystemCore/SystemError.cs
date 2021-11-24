using System;

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
        /// <param name="message">The message.</param>
        public static void Handle(Exception exception, string message = null)
        {
            bool.TryParse(ConfigReader.GetSetting("debug"), out bool debug);
            if (debug)
            {
                throw exception;
            }

            throw new Exception("Something went wrong while running this application, please try again or contact the administrators.");
        }
    }
}