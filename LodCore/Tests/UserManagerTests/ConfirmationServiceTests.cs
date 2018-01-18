using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UserManagement.Domain;
using UserManagement.Infrastructure;
using Common;

namespace UserManagerTests
{
    [TestClass]
    public class ConfirmationServiceTests
    {
        private ConfirmationService _confirmationService;
        private Mock<IUserRepository> _userRepoStub;
        private Mock<IValidationRequestsRepository> _validationRequesRepoStub;
        private Mock<IEventPublisher> _eventBus;

        [TestInitialize]
        public void Setup()
        {
            _userRepoStub = new Mock<IUserRepository>();
            _validationRequesRepoStub = new Mock<IValidationRequestsRepository>();
            _eventBus = new Mock<IEventPublisher>();

            _confirmationService = new ConfirmationService(
                _userRepoStub.Object,
                _validationRequesRepoStub.Object,
                _eventBus.Object);
        }

        [TestMethod]
        public void ConfirmationServiceSaveTheToken()
        {
            //arrange
            var userId = 42;
            _userRepoStub.Setup(rep => rep.GetAccount(42))
                .Returns((new Mock<Account>()).Object);

            //act
            _confirmationService.SetupEmailConfirmation(userId);

            //assert
            _eventBus.Verify(mock => mock.PublishEvent(It.IsAny<MailValidationRequest>()), Times.Once);
        }

        [TestMethod]
        public void ConfirmationServiceConfirmsEmail()
        {
            //arrange
            var mailValidationRq = new MailValidationRequest(42, "thisistoken");

            _validationRequesRepoStub.Setup(rep => rep.GetMailValidationRequest(It.IsAny<string>()))
                .Returns(mailValidationRq);

            var testAccMock = new Mock<Account>();
            testAccMock.Setup(acc => acc.UserId).Returns(42);
            testAccMock.Setup(acc => acc.ConfirmationStatus).Returns(ConfirmationStatus.Unconfirmed);

            _userRepoStub.Setup(rep => rep.GetAccount(mailValidationRq.UserId)).Returns(testAccMock.Object);

            //act
            _confirmationService.ConfirmEmail(mailValidationRq.Token);

            //assert
            testAccMock.VerifySet(mock => mock.ConfirmationStatus = ConfirmationStatus.EmailConfirmed);
        }

        [TestMethod]
        public void ConfirmationServiceConfirmsProfile()
        {
            var mailValidationRq = new MailValidationRequest(42, "thisistoken");

            _validationRequesRepoStub.Setup(rep => rep.GetMailValidationRequest(It.IsAny<string>()))
                .Returns(mailValidationRq);

            var testAccMock = new Mock<Account>();
            testAccMock.Setup(acc => acc.UserId).Returns(42);
            testAccMock.Setup(acc => acc.Password).Returns(Password.FromPlainString("f34password"));
            testAccMock.Setup(acc => acc.ConfirmationStatus).Returns(ConfirmationStatus.Unconfirmed);

            _userRepoStub.Setup(rep => rep.GetAccount(mailValidationRq.UserId)).Returns(testAccMock.Object);

            //act
            _confirmationService.ConfirmProfile(mailValidationRq.UserId);

            //assert
            testAccMock.VerifySet(mock => mock.ConfirmationStatus = ConfirmationStatus.FullyConfirmed);
        }
    }
}