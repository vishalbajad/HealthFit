using HealthFit.Utilities;
using System;
using System.Security.Cryptography;

namespace HealthFit.Utilities
{
    public class PasswordHasher
    {
        public static string HashPassword(string password, out string salt)
        {
            byte[] saltBytes = GenerateSalt();
            salt = Convert.ToBase64String(saltBytes);

            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];

            // Concatenate the salt and password bytes
            Array.Copy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
            Array.Copy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);

            using (var hasher = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] hashedBytes = hasher.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static bool VerifyPassword(string password, string salt, string hashedPassword)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];

            // Concatenate the salt and password bytes
            Array.Copy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
            Array.Copy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);

            using (var hasher = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] hashedBytes = hasher.ComputeHash(combinedBytes);
                string hashedPasswordToCompare = Convert.ToBase64String(hashedBytes);

                return hashedPasswordToCompare.Equals(hashedPassword);
            }
        }

        private static byte[] GenerateSalt()
        {
            const int saltLength = 16; // Salt length in bytes
            byte[] saltBytes = new byte[saltLength];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }

            return saltBytes;
        }
    }
}

//string password = "MyPassword123";
//string salt;
//string hashedPassword = PasswordHasher.HashPassword(password, out salt);

//// Store the hashedPassword and salt in the database for the user

//// Later, when verifying the password
//bool isPasswordValid = PasswordHasher.VerifyPassword(password, salt, hashedPassword);
