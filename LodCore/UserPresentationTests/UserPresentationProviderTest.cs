using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UserPresentaton;

namespace UserPresentationTests
{
    [TestClass]
    public class UserPresentationProviderTest
    {
        private static readonly Dictionary<string, NotificationType> NotificationSettings =
            Enum.GetValues(typeof (NotificationType)).Cast<NotificationType>().ToDictionary(type => type.ToString());

        private Mock<NotificationSetting> _notificationSettingMock;
        private Mock<INotificationSettingsRepository> _notificationSettingsRepositoryMock;

        private UserPresentationProvider _userPresentationProvider;

        [TestInitialize]
        public void Setup()
        {
            _notificationSettingsRepositoryMock = new Mock<INotificationSettingsRepository>();
            _notificationSettingMock = new Mock<NotificationSetting>();
        }

        [TestMethod]
        public void GetUserEventSettingsReturnsUser()
        {
            //arrange
            _notificationSettingMock.Setup(mock => mock.UserId).Returns(42);
            _notificationSettingMock.Setup(mock => mock.NotificationType).Returns(NotificationSettings["OrderPlaced"]);

            _notificationSettingsRepositoryMock.Setup(
                mock => mock.ReadNotificationSettingByCriteria(It.IsAny<Func<NotificationSetting, bool>>()))
                .Returns(_notificationSettingMock.Object);

            _userPresentationProvider = new UserPresentationProvider(_notificationSettingsRepositoryMock.Object);


            //act
            var res = _userPresentationProvider.GetUserEventSettings(42, "OrderPlaced");

            //assert
            Assert.IsTrue(res.UserId == 42 && res.NotificationType == NotificationSettings["OrderPlaced"]);
        }

        [TestMethod]
        public void NotificationSettingsDictonaryWorksCorrectly()
        {
            Assert.IsTrue(NotificationSettings["NewDeveloperOnProject"] == NotificationType.NewDeveloperOnProject);
        }
    }
}