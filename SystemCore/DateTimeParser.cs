using System;
using System.Globalization;

namespace SystemCore
{
    /// <summary>
    /// Provides a date time parser for parsing date times.
    /// </summary>
    public static class DateTimeParser
    {
        public const string DatabaseDateTimeFormatString = "MM/dd/yyyy hh:mm:00";
        
        /// <summary>
        /// Parses a datetime string from database format to datetime object.
        /// </summary>
        /// <param name="date">The date in database format</param>
        /// <returns>The datetime object or datetime object with min value in case of invalid date.w</returns>
        public static DateTime ParseFromDatabaseFormat(string date)
        {
            bool success = DateTime.TryParseExact(date, 
                DatabaseDateTimeFormatString, 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, 
                out DateTime dateObject);

            return success ? dateObject : DateTime.MinValue;
        }
    }
}