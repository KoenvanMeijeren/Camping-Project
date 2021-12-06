using System;
using System.Globalization;

namespace SystemCore
{
    public class DateTimeParser
    {
        public const string DatabaseDateTimeFormatString = "MM/dd/yyyy hh:mm:00";
        
        public static DateTime Parse(string date)
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