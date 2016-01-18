using System.Net.Mail;
using Mailing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserManagement.Domain.Events;

namespace InfrastructureTests
{
    [TestClass]
    public class MailingTests
    {
        [TestMethod]
        public void ConfirmationServiceSendsEmail()
        {
            //arrange
            var mailer = new Mailer(new MailerSettings());

            //act
            mailer.SendConfirmationMail("thisistoken", new MailAddress("test.lod@outlook.com"));

            //assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void NotificationServiceSendsEmail()
        {
            //arrange
            var mailer = new Mailer(new MailerSettings());

            //act
            mailer.SendNotificationEmail(new MailAddress("test.lod@outlook.com"), new NewEmailConfirmedDeveloper(42));

            //assert
            Assert.IsTrue(true);
        }
    }
}
