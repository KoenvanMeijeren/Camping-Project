using System.Text.RegularExpressions;

namespace SystemCore
{
    /// <summary>
    /// Provides a class which groups common reqex patterns used for checking data validness.
    /// </summary>
    public static class RegexHelper
    {
        /// <summary>
        /// Determines if the input is a correct email.
        /// </summary>
        /// <param name="email">The supped email address</param>
        /// <returns>Whether the input is a correct email or not. (E.g. name@site.nl</returns>
        public static bool IsEmailValid(string email)
        {
            if (email == null)
            {
                return false;
            }
            
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);
            return regex.IsMatch(email.Trim());
        }
    }
}