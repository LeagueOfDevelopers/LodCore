using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NotificationService;
using ProjectManagement.Domain.Events;
using UserPresentaton;

namespace NotificationServiceTests
{
    [TestClass]
    public class EventSinkBaseTests
    {
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

            _distributionPolicyFactoryMock.Setup(factory => factory.GetAllPolicy())
                .Returns(new DistributionPolicy(new[] {42, 52, 77}));

            _projectsEventSink = new ProjectsEventSink(
                _distributionPolicyFactoryMock.Object,
                _eventRepositoryMock.Object,
                _mailerMock.Object,
                _userPresentationProviderMock.Object);

            //act
            _projectsEventSink.ConsumeEvent(new NewDeveloperOnProject(11, 10));
            _projectsEventSink.ConsumeEvent(new NewDeveloperOnProject(12, 10));
            _projectsEventSink.ConsumeEvent(new NewDeveloperOnProject(13, 10));

            //assert
            _mailerMock.Verify(mailer => mailer.SendNotificationEmail(new [] {42, 55}, It.IsAny<NewDeveloperOnProject>()));
        }

        private Mock<IUserPresentationProvider> _userPresentationProviderMock;
        private Mock<IDistributionPolicyFactory> _distributionPolicyFactoryMock;
        private Mock<IEventRepository> _eventRepositoryMock;
        private Mock<IMailer> _mailerMock;



        private ProjectsEventSink _projectsEventSink;
    }
}
