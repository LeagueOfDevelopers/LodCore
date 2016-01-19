using System;
using System.Net.Mail;
using Mailing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NotificationService;
using UserManagement.Domain.Events;

namespace InfrastructureTests
{
    [TestClass]
    public class BlackboxMailingTests
    {
        private Mock<IEventInfo> _eventInfoMock;

        private Mock<INotificationEmailDescriber> _notificationEmailDescriberMock;

        [TestInitialize]
        public void SetupMoqs()
        {
            _notificationEmailDescriberMock = new Mock<INotificationEmailDescriber>();

            _notificationEmailDescriberMock.Setup(mock => mock.Describe(It.IsAny<IEventInfo>()))
                .Returns("Это описание любого события, создание в BlackBox тесте Mailing'а");
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