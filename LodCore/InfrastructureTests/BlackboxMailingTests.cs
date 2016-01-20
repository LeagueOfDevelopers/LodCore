using System;
using System.Net.Mail;
using Mailing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NotificationService;
using OrderManagement.Domain.Events;
using OrderManagement.Infrastructure;
using UserManagement.Domain.Events;

namespace InfrastructureTests
{
    [TestClass]
    public class BlackboxMailingTests
    {
        private NotificationEmailDescriber _notificationEmailDescriber;

        private IOrderRepository

        private OrderPlaced @event;

        [TestInitialize]
        public void SetupMoqs()
        {
            @event = new OrderPlaced(42);
            _notificationEmailDescriber = new NotificationEmailDescriber();
        }

        [TestMethod]
        [Timeout(3000)]
        public void ConfirmationServiceSendsEmail()
        {
            try
            {
                //arrange
                var mailer = new Mailer(new MailerSettings(), _notificationEmailDescriberMock.Object);

                //act
                mailer.SendConfirmationMail("thisistoken", new MailAddress("test.lod@outlook.com"));
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }
        }

        [TestMethod]
        [Timeout(3000)]
        public void NotificationServiceSendsEmail()
        {
            try
            {
                //arrange
                var mailer = new Mailer(new MailerSettings(), _notificationEmailDescriberMock.Object);

                //act
                mailer.SendNotificationEmail(new MailAddress("test.lod@outlook.com"), new NewEmailConfirmedDeveloper(42));
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }
        }
    }
}