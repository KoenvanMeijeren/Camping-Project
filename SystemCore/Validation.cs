using System;

namespace SystemCore
{
    public static class Validation
    {
        public static bool IsInputFilled(string input)
        {
            return (!string.IsNullOrEmpty(input) && input.Length != 0);
        }

        public static bool IsNumber(string input)
        {
            return (int.TryParse(input, out int result));
        }

        public static bool IsBirthdateValid(DateTime birthdate)
        {
            if (birthdate < new DateTime(1900,01,01))
            {
                return false;
            }

            if (birthdate > DateTime.Now)
            {
                return false;
            }

            return true;
        }

        public static bool IsBirthdateAdult(DateTime birthdate)
        {
            int AgeLimit = 18;
            int age = DateTime.Now.Year - birthdate.Year;

            if (birthdate > DateTime.Now.AddYears(-age))
            {
                age--;
            }
            if (age <= AgeLimit)
            {
                return false;
            }
            return true;
        }
    }
}