using System;

namespace SystemCore
{
    public static class Validation
    {
        public const int AdultAge = 18;
        
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
            return int.TryParse(input, out int result);
        }
        
        public static bool IsDecimalNumber(string input)
        {
            return float.TryParse(input, out float result);
        }

        public static bool IsBirthdateValid(DateTime birthdate)
        {
            return !(birthdate < new DateTime(1900, 01, 01)) && !(birthdate > DateTime.Now);
        }

        public static bool IsBirthdateAdult(DateTime birthdate)
        {
            return birthdate.AddYears(AdultAge) <= DateTime.Today;
        }
    }
}