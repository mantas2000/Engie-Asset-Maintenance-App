////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: Email.cs
//FileType: Visual C# Source file
//Author : Joel Carter
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : All email functionality for app
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using engie_maintenance_app.WebServices;
using Plugin.SecureStorage;


namespace engie_maintenance_app.EmailService
{
    public class Email
    {
        private const string _emailSenderAddress = "maint-app@velocitysolutions.tk";
        private const string _emailSenderName = "ENGIE Maintenance App";
        
        // Sets SES login details.
        private const string _smtpUsername = "AKIA5ZSFVR4NQHKG2G6Y";
        private const string _smtpPassword = "BDxHmSRmUASRLphY1/RQWNke4SEan8V9ut21stKWyLcU";
        
        // Sets the AWS host server address.
        private const string _host = "email-smtp.eu-west-2.amazonaws.com";
        private const int _port = 587;

        /// <summary>
        /// Send given MailMessage using SMTP.
        /// </summary>
        /// <param name="email">The email to be sent.</param>
        public static void SendEmail(MailMessage email)
        {
            using (var client = new SmtpClient(_host, _port))
            {
                // Creates credential object by passing it the login details.
                client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                client.EnableSsl = true;
                
                // Attempts to send the emails and shows status in  console.
                try
                {
                    Console.WriteLine("Attempting to send email");
                    client.Send(email);
                    Console.WriteLine("Email sent successfully!");
                }
                catch (Exception exception)
                {
                    Console.WriteLine("The email could not be sent.");
                    Console.WriteLine("Error message: "+ exception.Message);
                    throw exception;
                }
            }
        }

        /// <summary>
        /// Sends a form using SendEmail.
        /// </summary>
        /// <param name="filePath">The filepatch of the form to be sent.</param>
        /// <returns>Returns false upon failure.</returns>
        public static async Task<bool> SendForm(string filePath)
        {
            try
            {
                // Sets details for email.
                string formType = FormMethods.GetFormTypeFromPath(filePath);
                string recipientAddress;
                try
                {
                    recipientAddress = await WebServices.WebServices.GetRecipientEmail(formType);
                }
                catch (NoInternetException e)
                {
                    throw e;
                }
                string engineersName = CrossSecureStorage.Current.GetValue("Username");
                string engineersEmail = CrossSecureStorage.Current.GetValue("Email");
                string engineerType = CrossSecureStorage.Current.GetValue("EngineerType");

                // Creates and builds a new MailMessage object ready to be sent.
                MailMessage email = new MailMessage
                {
                    IsBodyHtml = true,
                    From = new MailAddress(_emailSenderAddress, _emailSenderName),
                    Subject = "An engineer just submitted these forms",
                    Body = "Please find the forms completed by "+engineerType+" engineer: " + engineersName + ", email id: "+engineersEmail+" attached"
                };
                email.To.Add(new MailAddress(recipientAddress));

                if (filePath != null)
                {
                    email.Attachments.Add(new Attachment(filePath));
                }
                SendEmail(email);

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sends given verification code to given email using SendEmail.
        /// </summary>
        /// <param name="registrationEmail">The email to be sent the verification code.</param>
        /// <param name="code">The code to be sent.</param>
        public static void SendVerificationCode(string registrationEmail, string code)
        {
            MailMessage email = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(_emailSenderAddress, _emailSenderName),
                Subject = "Email Verification Code",
                Body = "Your email verification code is: " + code
            };
            email.To.Add(new MailAddress(registrationEmail));
            
            SendEmail(email);
        }
    }
}