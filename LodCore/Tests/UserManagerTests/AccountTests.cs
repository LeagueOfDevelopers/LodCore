using System.Net.Mail;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UserManagement.Domain;

namespace UserManagerTests
{
    [TestClass]
    public class AccountTests
    {
        [TestInitialize]
        public void Setup()
        {
            _passwordMock = new Mock<Password>();
            _profileMock = new Mock<Profile>();
        }

        [TestMethod]
        public void CreatingAccountCreateAccountWithValidEmail()
        {
            //arrange
            _passwordMock = new Mock<Password>();
            _profileMock = new Mock<Profile>();

            //act
            var account = new Account("Name", "Lastname", new MailAddress("itis@validmail.ru"),
                new Password("qwertyui"), AccountRole.User, ConfirmationStatus.Unconfirmed, _profileMock.Object, 42, 42);

            //assert
           Assert.IsTrue(account.Email.Address == "itis@validmail.ru"); 
        }

        private Mock<Password> _passwordMock;
        private Mock<Profile> _profileMock;
    }
}