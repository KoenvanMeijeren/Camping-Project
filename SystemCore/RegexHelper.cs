using System.Text.RegularExpressions;

namespace SystemCore
{
    public class RegexHelper
    {
        public static bool IsEmailValid(string email)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);//student@mail.nl
            return regex.IsMatch(email.Trim());
        }
    }
}