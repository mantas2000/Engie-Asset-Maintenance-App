////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: InputValidation.cs
//FileType: Visual C# Source file
//Author : Mohammed Albulushi, Mantas Burcikas, Joel Carter
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class containing functions for input validations.
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace engie_maintenance_app.Security
{
    public static class InputValidation
    {
        /// <summary>
        /// Check that every user input does not contain invalid characters.
        /// </summary>
        /// <param name="userInputs">The user inputs to be checked.</param>
        /// <returns>If any of the inputs contain invalid characters.</returns>
        public static bool Validate(IEnumerable<string> userInputs)
        {
            return userInputs.All(input => !input.Any(t => CheckChars(input)));
        }

        /// <summary>
        /// Checks if the given string contains any forbidden characters.
        /// </summary>
        /// <param name="userInput">The string to test.</param>
        /// <returns>True if the given string contains invalid chars/words.</returns>
        public static bool CheckChars(string userInput)
        {
            var badChars = new List<string>
            {
                "\"", "\'", "&apos", ";",
                "select", "insert", "delete from", "drop table", "delete", "drop", 
                "script", "update", "master", "truncate", "declare"
            };
            
            // Return true if user's input contains any of invalid chars combination.
            return badChars.Any(userInput.ToLower().Contains);
        }

        /// <summary>
        /// Checks if the given email is valid.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the email is valid.</returns>
        public static bool CheckEmail(string email)
        {
            // Regex for email pattern.
            var emailPattern = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";

            if (Regex.IsMatch(email, emailPattern))
            {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Checks if the given password is valid.
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <returns>True if the password is valid.</returns>
        public static bool CheckPass(string password)
        {
            // Regex for the password pattern.
            var passwordPattern = @"(?-i)(?=^.{8,15}$)((?!.*\s)(?=.*[A-Z])(?=.*[a-z]))(?=(1)(?=.*\d)|.*[^A-Za-z0-9])^.*$";

            if (Regex.IsMatch(password, passwordPattern))
            {
                return true;
            }

            return false;
        }
    }
}