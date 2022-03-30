////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: Sha256Hash.cs
//FileType: Visual C# Source file
//Author : Muhammed Albulushi, Mantas Burcikas
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class containing functions for creating salt and hashing password.
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Security.Cryptography;
using System.Text;

namespace engie_maintenance_app.Security
{
    public static class Sha256Hash
    {
        /// <summary>
        /// Creates a ranrdom salt of given length.
        /// </summary>
        /// <param name="length">The length of salt to create.</param>
        /// <returns>A base64 string of the random salt</returns>
        public static string CreateSalt(int length)
        {
            // Create salt of given length.
            var salt = new byte[length];
            
            // Using a cryptographic Random Number Generator create salt.
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Return salt as a Base64 String.
            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// Generates a hash of the given input using the given salt.
        /// Uses SHA256 with UTF8 encoding.
        /// </summary>
        /// <param name="input">The string to hash.</param>
        /// <param name="salt">The salt to salt the hash with.</param>
        /// <returns>The hashed and salted string.</returns>
        public static string GenerateHash(string input, string salt)
        {
            // Return string converted into SHA256 hash.
            return ByteArrayToString(new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(input + salt)));
        }

        /// <summary>
        /// Converts the given byte array to a string using BitConverter
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        private static string ByteArrayToString(byte[] hash)
        {
            // Removes the seperator "-" from hexadecimal form of the hash.
            return BitConverter.ToString(hash).Replace("-","");
        }
    }
}

