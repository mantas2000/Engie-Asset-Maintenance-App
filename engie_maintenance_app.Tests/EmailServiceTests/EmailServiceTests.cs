using System;
using NUnit.Framework;
using Xamarin.Forms;
using engie_maintenance_app.EmailService;

namespace engie_maintenance_app.Tests.EmailServiceTests
{
    [TestFixture]
    public class EmailServiceTests
    {
        /// <summary>
        /// Gets the form type from the given path (Not the file at that path)
        /// </summary>
        [Test]
        public void GetFormTypeTest()
        {
            var expectedResult = "Gas";

            var actualResult = FormMethods.GetFormTypeFromPath("/data/user/0/velocitysolutions.tk.engie_maintenance_app/files/137/Gas_webservicestest20210124093900585.pdf");

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
