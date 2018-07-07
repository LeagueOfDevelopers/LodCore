using LodCoreLibrary.Domain.NotificationService;
using LodCoreLibrary.Domain.UserManagement;
using LodCoreLibrary.Infrastructure.DataAccess.Repositories;
using LodCoreLibrary.Infrastructure.Mailing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace NotificationServiceTests
{
    [TestClass]
    public class EventSinkBaseTests
    {
        private Mock<IDistributionPolicyFactory> _distributionPolicyFactoryMock;
        private Mock<IEventRepository> _eventRepositoryMock;
        private Mock<IMailer> _mailerMock;

        private ProjectsEventSink<NewDeveloperOnProject> _projectsEventSink;

        private Mock<IUserPresentationProvider> _userPresentationProviderMock;

        [TestInitialize]
        public void Setup()
        {
            _userPresentationProviderMock = new Mock<IUserPresentationProvider>();

            _distributionPolicyFactoryMock = new Mock<IDistributionPolicyFactory>();

            _eventRepositoryMock = new Mock<IEventRepository>();

            _mailerMock = new Mock<IMailer>();
        }

        [TestMethod]
        public void SendOutEmailsAboutEventUsesNotificationSettings()
        {
            //arrange
            _userPresentationProviderMock.Setup(provider => provider.GetUserEventSettings(42, It.IsAny<string>()))
                .Returns(NotificationSettingValue.SendNotificationAndMail);

            _userPresentationProviderMock.Setup(provider => provider.GetUserEventSettings(52, It.IsAny<string>()))
                .Returns(NotificationSettingValue.SendNotificationAndMail);

            _userPresentationProviderMock.Setup(provider => provider.GetUserEventSettings(77, It.IsAny<string>()))
                .Returns(NotificationSettingValue.SendOnlyNotification);

            _distributionPolicyFactoryMock.Setup(factory => factory.GetAdminRelatedPolicy())
                .Returns(new DistributionPolicy(new[] {42}));

            _distributionPolicyFactoryMock.Setup(factory => factory.GetProjectRelatedPolicy(It.IsAny<int>()))
                .Returns(new DistributionPolicy(new[] {52, 77}));

            _projectsEventSink = new ProjectsEventSink<NewDeveloperOnProject>(
                _distributionPolicyFactoryMock.Object,
                _eventRepositoryMock.Object,
                _mailerMock.Object,
                _userPresentationProviderMock.Object);

            var developerOnProjectEvent = new NewDeveloperOnProject(11, 10, "firstName", "lastName", "projectName");

            //act
            _projectsEventSink.Consume(developerOnProjectEvent);

            //assert
            _mailerMock.Verify(mailer => mailer.SendNotificationEmail(new[] {52, 42}, developerOnProjectEvent),
                Times.Once);
        }
    }
}