////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: WebServices.cs
//FileType: Visual C# Source file
//Author : Joel Carter, Archie Vann
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class containing functions to get data from the web api.
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using engie_maintenance_app.Objects;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace engie_maintenance_app.WebServices
{
    public class WebServices
    {
        private static readonly string
            _url = "http://engiewebservices.eu-west-2.elasticbeanstalk.com/WebService1.asmx/";

        /// <summary>
        /// Checks if given email exists in database.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if email does not exist in database.</returns>
        public static async Task<bool> EmailUnique(String email)
        {
            bool emailUnique = false;
            try
            {
                string url = _url + "EmailUnique";
                IEnumerable<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("email", email),
                };

                string xmlString = await new DataAccessLayer.HttpRequest().PostHttpRequest(parameters, url);

                if (xmlString.Contains("true"))
                {
                    emailUnique = true;
                }
                else
                {
                    emailUnique = false;
                }

                return emailUnique;
            }
            catch (Exception e)
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine(e);

                throw new NoInternetException();
            }
        }

        /// <summary>
        /// Changes the passwordHash and salt of the given email to the given passwordHash and salt.
        /// </summary>
        /// <param name="email">The email thats password should be changed</param>
        /// <param name="newSalt">The salt used to create the hashed password.</param>
        /// <param name="newPassword">The hashed password.</param>
        /// <returns></returns>
        public static async Task<bool> ChangeUserPassword(string email, string newSalt, string newPassword)
        {
            string url = _url + "ChangeUserPassword";
            IEnumerable<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("email", email),
                new KeyValuePair<string, string>("newSalt", newSalt),
                new KeyValuePair<string, string>("newPassword", newPassword)
            };

            string xmlString = await new DataAccessLayer.HttpRequest().PostHttpRequest(parameters, url);

            bool passwordChangeSuccessful;

            if (xmlString.Contains("true"))
            {
                passwordChangeSuccessful = true;
            }
            else
            {

                passwordChangeSuccessful = false;
            }

            return passwordChangeSuccessful;
        }

        /// <summary>
        /// Creates an account with given details.
        /// </summary>
        /// <param name="username">The account's username.</param>
        /// <param name="encryptedPassword">The unencypted password.</param>
        /// <param name="email">The account's email.</param>
        /// <param name="salt">The account's password salt.</param>
        /// <param name="roleType">The account's type.</param>
        /// <param name="engineerType">The account's engineer type</param>
        /// <returns></returns>

        public static async Task<bool> CreateAccount(string username, string encryptedPassword, string email,
            string salt,
            string roleType, string engineerType)
        {
            try
            {
                string url = _url + "CreateAccount";
                IEnumerable<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("encryptedPassword", encryptedPassword),
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("salt", salt),
                    new KeyValuePair<string, string>("roleType", roleType),
                    new KeyValuePair<string, string>("engineerType", engineerType)
                };

                await new DataAccessLayer.HttpRequest().PostHttpRequest(parameters, url);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine(e);

                throw new NoInternetException();
            }
        }

        /// <summary>
        /// Changes the recipient email for the given form type
        /// </summary>
        /// <param name="formType">The type of form. Eg. gas.</param>
        /// <param name="newRecipientEmail">The new email those forms will be sent to.</param>
        /// <returns>True if recipient email is successfully changed.</returns>
        public static async Task<bool> SetNewRecipientEmail(string formType, string newRecipientEmail)
        {
            try
            {
                string url = _url + "ChangeRecipientEmail";

                IEnumerable<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("formType", formType),
                    new KeyValuePair<string, string>("newRecipientEmail", newRecipientEmail)
                };

                await new DataAccessLayer.HttpRequest().PostHttpRequest(parameters, url);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine(e);

                throw new NoInternetException();
            }
        }

        /// <summary>
        /// Gets the details of a given email from the database.
        /// </summary>
        /// <param name="email">The email of the account of the details wanted</param>
        /// <param name="choice">TO BE REMOVED. 0 = normal</param>
        /// <returns>Returns user details.</returns>
        public static async Task<User> GetUserDetails(string email)
        {
            User user = new User();
            try
            {
                string url = _url + "ReturnUser";
                IEnumerable<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("email", email),
                };

                string xmlString = await new DataAccessLayer.HttpRequest().PostHttpRequest(parameters, url);

                XmlRootAttribute xmlRoot = new XmlRootAttribute
                {
                    ElementName = "User", Namespace = "http://velocitysolutions.tk/", IsNullable = true
                };
                XmlSerializer xs = new XmlSerializer(typeof(User), xmlRoot);
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));
                user = (User) xs.Deserialize(ms);

                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine(e);
                // Removes the user password from the local device so that the automatic login doesn't happen again and again.
                CrossSecureStorage.Current.DeleteKey("Password");

                throw new NoInternetException();
            }
        }

        /// <summary>
        /// Gets a list of all possible form types
        /// </summary>
        /// <param name="choice">idk just do 0.</param>
        /// <returns>A list of all form types</returns>
        public static async Task<List<string>> GetFormTypes()
        {
            try
            {
                string url = _url + "GetFormTypes";

                string xmlString = await new DataAccessLayer.HttpRequest().GetHttpRequest(url);
                string namespaceToBeRemoved = " xmlns=\"http://velocitysolutions.tk/\"";
                xmlString = xmlString.Replace(namespaceToBeRemoved, "");

                XmlSerializer xs = new XmlSerializer(typeof(List<string>), new XmlRootAttribute("ArrayOfString"));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));
                List<string> formTypesList = (List<string>) xs.Deserialize(ms);

                return formTypesList;
            }
            catch (Exception e)
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine(e);

                throw new NoInternetException();
            }
        }

        /// <summary>
        /// Gets the recipient's email address for the given form type.
        /// </summary>
        /// <param name="formType">The form type.</param>
        /// <param name="choice">idk do 0</param>
        /// <returns></returns>
        public static async Task<String> GetRecipientEmail(String formType)
        {
            try
            {
                string url = _url + "GetFormEmailRecipient";
                IEnumerable<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("formType", formType),
                };

                string xmlString = await new DataAccessLayer.HttpRequest().PostHttpRequest(parameters, url);

                XmlRootAttribute xmlRoot = new XmlRootAttribute
                {
                    ElementName = "string", Namespace = "http://velocitysolutions.tk/", IsNullable = true
                };
                XmlSerializer xs = new XmlSerializer(typeof(String), xmlRoot);
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));
                string recipientEmail = (string) xs.Deserialize(ms);

                return recipientEmail;
            }
            catch (Exception e)
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine(e);
                // Calls the error handler function
                throw new NoInternetException();
            }
        }
    }

    /// <summary>
    /// Redirects users or smth
    /// </summary>
    /// <param name="choice"></param>
    /// <returns>True if succeeds.</returns>
    public class NoInternetException : Exception
    {
        public static string ErrorHeader = "Couldn't connect to server";
        public static string ErrorBody = "Please check your connected to the internet and try again";
        public static string ErrorButton = "Ok";

        /*
         * catch (NoInternetException)
                    {
                        await DisplayAlert(NoInternetException.ErrorHeader,
                            NoInternetException.ErrorBody,
                            NoInternetException.ErrorButton);
                        return;
                    }
        */

        /// <summary>
        /// This exception is thrown when a method that uses the internet fails without another appropriate exception.
        /// Implies internet cut out mid way through
        /// </summary>
        public NoInternetException()
        {

        }
    }
}
    