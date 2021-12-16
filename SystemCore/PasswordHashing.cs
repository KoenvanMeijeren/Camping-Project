using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SystemCore
{
    public static class PasswordHashing
    {
        /// <summary>
        /// Check if string is a Base64 string. Used to check if database password is a base64 string.
        /// </summary>
        /// <param name="base64">database account password</param>
        /// <returns>Boolean value for given question</returns>
        public static bool IsBase64String(string base64)
        {
            if (base64 == null)
            {
                return false;
            }
            
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
        }

        /// <summary>
        /// Validate entered password with hashed database password.
        /// </summary>
        /// <param name="databasePassword">Hashed database password</param>
        /// <param name="enteredPassword">Entered 'clean' password</param>
        /// <returns>Boolean value based on validation</returns>
        public static bool SignInHashValidation(string databasePassword, string enteredPassword)
        {
            if (!IsBase64String(databasePassword))
            {
                return false;
            }

            // Extract the bytes
            byte[] hashBytes = Convert.FromBase64String(databasePassword);

            // Get the salt
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Compute the hash on the password the user entered
            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            // Compare the results
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Hash the given string.
        /// </summary>
        /// <param name="enteredPassword">String unhashed password</param>
        /// <returns>String hashed password</returns>
        public static string HashPassword(string enteredPassword)
        {
            //Create the salt value with a cryptographic PRNG
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            //Create the Rfc2898DeriveBytes and get the hash value
            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            //Combine the salt and password bytes for later use
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            //Turn the combined salt+hash into a string
            string savedPasswordHash = Convert.ToBase64String(hashBytes);

            return savedPasswordHash;
        }
    }
}
