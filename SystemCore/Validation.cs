using System;

namespace SystemCore
{
    public static class Validation
    {
        public const int AdultAge = 18;
        private const int InvalidBirthdateYear = 1900;
        private const int InvalidBirthdateMonth = 01;
        private const int InvalidBirthdateDay = 01;
        
        
        public static bool IsInputFilled(string input)
        {
            return !string.IsNullOrEmpty(input) && input.Length != 0;
        }

        public static bool IsInputBelowMaxLength(string input, int max)
        {
            return input != null && input.Length <= max;
        }
        
        public static bool IsNumber(string input)
        {
            return int.TryParse(input, out _);
        }
        
        public static bool IsDecimalNumber(string input)
        {
            return double.TryParse(input, out _);
        }

        public static bool IsBirthdateValid(DateTime birthdate)
        {
            return !(birthdate < new DateTime(InvalidBirthdateYear, InvalidBirthdateMonth, InvalidBirthdateDay)) &&
                   !(birthdate > DateTime.Now);
        }

        public static bool IsBirthdateAdult(DateTime birthdate)
        {
            return birthdate.AddYears(AdultAge) <= DateTime.Today;
        }
    }
}