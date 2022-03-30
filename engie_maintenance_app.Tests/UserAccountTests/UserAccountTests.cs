using System;
using System.Threading.Tasks;
using engie_maintenance_app.Objects;
using engie_maintenance_app.Security;
using NUnit.Framework;

namespace engie_maintenance_app.Tests.UserAccountTests
{
    [TestFixture]
    public class UserAccountTests
    {
        /// <summary>
        /// Tries to ChangeUserPassword with a user that DOES exist.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestChangeUserPasswordUserExists()
        {
            // Add timestamp to this email in runtime
            string emailPrefix = "a.vann1";
            string emailSuffix = "@ncl.ac.uk";

            string email = emailPrefix + emailSuffix;

            string password = "Password1!";
            string salt = Sha256Hash.CreateSalt(10);
            string hashedPassword = Sha256Hash.GenerateHash(password, salt);

            // If method says it succeeded check it did.
            if (await WebServices.WebServices.ChangeUserPassword(email, salt, hashedPassword))
            {
                Console.WriteLine("ChangeUserPassword thinks is successed");
                User user = await WebServices.WebServices.GetUserDetails(emailPrefix + emailSuffix);
                if (user.Password == hashedPassword)
                {
                    Assert.Pass("ChangeUserPassword worked");
                }
            }
            else
            {
                Assert.Fail("ChangeUserPassword failed to change password");
            }
        }

        /// <summary>
        /// Tries to ChangeUserPassword with a user that DOES NOT exist.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestChangeUserPasswordUserDoesNotExists()
        {
            // Account that doesn't exist
            string emailPrefix = DateTime.Now.ToString();
            string emailSuffix = "@gmail.com";

            string email = emailPrefix + emailSuffix;

            string password = "Password1!";
            string salt = Sha256Hash.CreateSalt(10);
            string hashedPassword = Sha256Hash.GenerateHash(password, salt);

            // Method should return false as account doesn't exist
            Assert.IsFalse(await WebServices.WebServices.ChangeUserPassword(email, salt, hashedPassword));
        }


        /// <summary>
        /// Checks if "averyuniqueemailaddress@gmail.com" is unique
        /// </summary>
        [Test]
        public void CheckEmailIsUnique()
        {
            // A unique email.
            var email = "averyuniqueemailaddress@gmail.com";
            
            // Returns true if the email does not exists in the db.
            var IsUnique = WebServices.WebServices.EmailUnique(email);
            
            // The test passes if the result is true.
            Assert.IsTrue(IsUnique.Result);
        }

        /// <summary>
        /// Checks if "muhammedtariq141@gmail.com" is NOT unique
        /// </summary>
        [Test]
        public void CheckEmailIsNotUnique()
        {
            // a unique email
            var email = "muhammedtariq141@gmail.com";
            
            // returns false if the email does not exists in the db
            var IsNotUnique = WebServices.WebServices.EmailUnique(email);
            
            // The test passes if the result is not unique
            Assert.IsFalse(IsNotUnique.Result);
        }
        
        /// <summary>
        /// Creates a new account using current time
        /// </summary>
        [Test]
        public async Task CreateNewAccount()
        {
            // new account details with unique email each time and hashed password
            var salt = Sha256Hash.CreateSalt(10);
            var hashedPassword = Sha256Hash.GenerateHash("todo", salt);
            String username = "Test";
            String email = "testemail"+DateTime.Now.ToString("yyyyMMddHHmmssfff")+"@gmail.com";
            String roleType = "Public";
            String engineerType = "Water";


            // creates a new account
            await WebServices.WebServices.CreateAccount(username, hashedPassword, email, salt, roleType, engineerType);
            
            // confirms if the new account is created with correct details
            User userDetails = WebServices.WebServices.GetUserDetails(email).Result;

            // checks if the user is actually registered in the db
            if (userDetails != null)
            {
                // checks if the data stored in the db matches the one we provided
                Assert.AreEqual(hashedPassword,userDetails.Password);
                Assert.AreEqual(salt,userDetails.Salt);
                Assert.AreEqual(username,userDetails.Username);
                Assert.AreEqual(email,userDetails.Email);
                Assert.AreEqual(roleType,userDetails.RoleType);
                Assert.AreEqual(engineerType,userDetails.EngineerType);
            }
        }
    }
}



