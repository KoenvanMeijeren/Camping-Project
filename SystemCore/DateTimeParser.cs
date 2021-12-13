using System;
using System.Globalization;

namespace SystemCore
{
    /// <summary>
    /// Provides a date time parser for parsing date times.
    /// </summary>
    public static class DateTimeParser
    {
        public const string
            InputDateTimeFormatString = "dd-MM-yyyy hh:mm:00",
            DatabaseDateTimeFormatString = "MM/dd/yyyy hh:mm:00",
            DatabaseDateFormatString = "yyyy-MM-dd";
        
        /// <summary>
        /// Parses a datetime string from database format to datetime object.
        /// </summary>
        /// <param name="date">The date in database format</param>
        /// <returns>The datetime object or datetime object with min value in case of invalid date.w</returns>
        public static DateTime TryParse(string date)
        {
            bool success = DateTime.TryParseExact(date, 
                DatabaseDateTimeFormatString, 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, 
                out var dateObject);

            if (success)
            {
                return dateObject;
            }
            
            success = DateTime.TryParseExact(date, 
                InputDateTimeFormatString, 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, 
                out dateObject);

            if (success)
            {
                return dateObject;
            }
            
            success = DateTime.TryParseExact(date, 
                DatabaseDateFormatString, 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, 
                out dateObject);

            if (success)
            {
                return dateObject;
            }
            
            success = DateTime.TryParse(date, out dateObject);

            return success ? dateObject : DateTime.MinValue;
        }
        
        /// <summary>
        /// Parses a datetime object to string as database format.
        /// </summary>
        /// <param name="date">The datetime object</param>
        /// <returns>The data as string in database format.</returns>
        public static string TryParseToDatabaseDateTimeFormat(DateTime? date)
        {
            return date is not DateTime dateTime ? null : dateTime.ToString(DatabaseDateTimeFormatString);
        }
        
        /// <summary>
        /// Parses a datetime object to string as database format.
        /// </summary>
        /// <param name="date">The datetime object</param>
        /// <returns>The data as string in database format.</returns>
        public static string TryParseToDatabaseDateFormat(DateTime? date)
        {
            return date is not DateTime dateTime ? null : dateTime.ToString(DatabaseDateFormatString);
        }
    }
}