using System;
using Mailing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UserManagerTests
{
    [TestClass]
    public class MailerTests
    {
        private Mailer _mailer;

        [TestInitialize]
        public void Setup()
        {
            _mailer = new Mailer(new MailerSettings());
        }

        [TestMethod]
        public void SendConfirmationMailSendsMail()
        {
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            _mailer.SendConfirmationMail(token, "boris.valdman@live.ru");
        }
    }
}