using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using engie_maintenance_app.Security;
using NUnit.Framework;
using Xamarin.Forms;
using engie_maintenance_app.WebServices;

namespace engie_maintenance_app.Tests.OtherTests
{
    [TestFixture]
    public class OtherTests
    {
        /// <summary>
        /// Test to change the recipient email
        /// </summary>
        [Test]
        public async Task SetNewRecipientEmail()
        { 
            // New email address 
            var email = "muhammedalbalushi@icloud.com";
            
            // List of form types we have
            List<string> formTypes = new List<string>()
            {
                "Water",
                "Electrical",
                "Gas",
                "Mechanical"                    
            };
            
            // sets the email address for each form type
            foreach (var formType in formTypes)
            {
                await WebServices.WebServices.SetNewRecipientEmail(formType,email);
            }
            
            // tests if the email has been changed for all form types 
            foreach (var formType in formTypes)
            {
                // gets the recipient email from db
               var actualEmail =  WebServices.WebServices.GetRecipientEmail(formType).Result;
               // tests if the actual email for the recipient is changed to our email 
               Assert.AreEqual(email, actualEmail);
            }
        }
    }
}