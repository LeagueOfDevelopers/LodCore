using System.Linq;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UserManagement.Application;
using UserManagement.Domain;

namespace UserManagerTests
{
    [TestClass]
    public class UserManagerTests
    {
        [TestInitialize]
        public void Setup()
        {
            _userManager = new UserManager(
                new UserRepository(new DatabaseSessionProvider()), Mock.Of<IConfirmationService>());
        }

        [TestMethod]
        [TestCategory("Blackbox")]
        public void AccountCreationTest()
        {
            var request = new CreateAccountRequest(
                "vmargelov@gmail.com",
                "Margelov",
                "Vitaly",
                "JusticePassword");
            _userManager.CreateUser(request);
            var expectedAccount = _userManager.GetUserList(account => account.Email == request.Email);
            Assert.IsTrue(expectedAccount.FirstOrDefault() != null);
        }

        private IUserManager _userManager;
    }
}
