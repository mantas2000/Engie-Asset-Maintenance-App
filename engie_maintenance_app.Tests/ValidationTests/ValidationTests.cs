using NUnit.Framework;
using System.Collections.Generic;
using engie_maintenance_app.DataAccessLayer;

namespace engie_maintenance_app.Tests.ValidationTests
{
    public class ValidationTests
    {
        /// <summary>
        /// Checks if chars are valid
        /// </summary>
        /// <param name="stri">The string to check</param>
        /// <returns>True if chars are valid</returns>
        private bool CheckValidChars(string stri)
        {
            return !Security.InputValidation.CheckChars(stri);
        }

        /// <summary>
        /// Checks if chars are invalid
        /// </summary>
        /// <param name="stri">The string to check</param>
        /// <returns>True if chars are invalid</returns>
        private bool CheckInvalidChars(string stri)
        {
            return Security.InputValidation.CheckChars(stri);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestInvalidChars()
        {
            List<string> invalid_chars = new List<string>()
            {
                "\"", "\'", "&apos", ";",
                "select", "insert", "delete from", "count", "drop table",
                "asc", "mid", "char", "or", "net", "delete", "drop",
                "script", "update", "and", "chr", "master", "truncate", "declare", "mid",

                "middle", "updates", "andor", "deleted", "counted", "counting", "declared",
                "selecting", "seleted"
            };

            bool testSucceeded = false;

            foreach (string x in invalid_chars)
            {
                if (CheckInvalidChars(x)) { testSucceeded = true; }
            }

            Assert.IsTrue(testSucceeded);
        }

        [Test]
        public void TestValidChars()
        {
            List<string> valid_chars = new List<string>()
            {
                "aaaaaaaaaaaaaaa", "awdjawd", "££%*!", "12345678890", "la", "\\", ",.,.",
                "الْأَبْجَدِيَّة الْعَرَبِيَّة", "خ", "ṣ", "æ", "Ö", "í", "ý", "`", "=-_+", "[]{}"
            };

            bool testSucceeded = false;

            foreach (string x in valid_chars)
            {
                if (CheckValidChars(x)) { testSucceeded = true; }
            }

            Assert.IsTrue(testSucceeded);
        }

        /// <summary>
        /// Checks if "abc@abc.com" is valid.
        /// Uses Security.InputValidation.CheckEmail.
        /// </summary>
        [Test]
        public void CheckValidEmail()
        {
            // Expected result.
            const bool expectedResult = true;

            // Returns true if the email is valid.
            bool actualResult = Security.InputValidation.CheckEmail("abc@abc.com");

            // Check if the application passed the test.
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Checks that "abc@abc" is an invalid email
        /// Security.InputValidation.CheckEmail
        /// </summary>
        [Test]
        public void CheckInvalidEmail()
        {
            // Expected result.
            const bool expectedResult = false;

            // Returns false if the email is not valid.
            bool actualResult = Security.InputValidation.CheckEmail("abc@abc");

            // Check if the application passed the test.
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Checks that "Password1!" is a valid password.
        /// Uses Security.InputValidation.CheckPass.
        /// </summary>
        [Test]
        public void CheckValidPassword()
        {
            // Expected result.
            const bool expectedResult = true;

            // Returns true if the password fits all requirements.
            bool actualResult = Security.InputValidation.CheckPass("Password1!");

            // Check if the application passed the test.
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Checks that "password1!" is invalid.
        /// Uses Security.InputValidation.CheckPass.
        /// </summary>
        [Test]
        public void CheckInvalidPassword()
        {
            // Expected result.
            const bool expectedResult = false;

            // Returns false if the password doesn't fit all requirements.
            bool actualResult = Security.InputValidation.CheckPass("password1!");

            // Check if the application passed the test.
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Checks that 'new[] {"{hello, 123, goodbye}"}' is valid.
        /// Uses Security.InputValidation.Validate.
        /// </summary>
        [Test]
        public void CheckValidationPassing()
        {
            // Expected result
            const bool expectedResult = true;

            // Returns true if all inputs don't contain any non valid chars
            bool actualResult = Security.InputValidation.Validate(new[] { "{hello, 123, goodbye}" });

            // Check if the application passed the test
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Checks that 'new[] {"{hello, DeLeTe, goodbye}"}' is invalid
        /// Uses Security.InputValidation.Validate.
        /// </summary>
        [Test]
        public void CheckValidationFailing()
        {
            // Expected result.
            const bool expectedResult = false;

            // Returns false if any of inputs contain any non valid chars.
            bool actualResult = Security.InputValidation.Validate(new[] { "{hello, DeLeTe, goodbye}" });

            // Check if the application passed the test.
            Assert.AreEqual(expectedResult, actualResult);
        }
        [Test]
        public void TestCommonUsernamesAgainstValidChars()
        {
            // Expected result.
            const bool expectedResult = true;

            // Returns false if any of inputs contain any non valid chars.
            bool actualResult = Security.InputValidation.Validate(new Resources.JsonParserTest().GetJsonData());

            // Check if the application passed the test.
            Assert.AreEqual(expectedResult, actualResult);
        }
        
    }
}