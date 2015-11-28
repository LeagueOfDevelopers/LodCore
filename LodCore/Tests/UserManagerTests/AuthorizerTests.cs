using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using UserManagement.Domain;
using UserManagement.Infrastructure;

namespace UserManagerTests
{
    [TestClass]
    public class AuthorizerTests
    {
        [TestMethod]
        public void CheckAuthorizationForNonExistantTokenReturnsFalse()
        {
            var userRepoStub = new Mock<IUserRepository>();
            var authorizer = new Authorizer(TimeSpan.FromHours(10), userRepoStub.Object);

            var isAuthorised = authorizer.CheckAuthorized("wrongtoken", 1);

            Assert.IsFalse(isAuthorised);
        }

        [TestMethod]
        public void AuthorizeCausesCheckAuthorizeReturnTrue()
        {
            var accountStub = new Mock<Account>();
            accountStub.Setup(acc => acc.UserId).Returns(3);
            accountStub.Setup(acc => acc.Email).Returns("v@a.x");
            accountStub.Setup(acc => acc.PasswordHash).Returns("Smth");
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock
                .Setup(mock => mock.GetAllAccounts(It.IsAny<Func<Account, bool>>()))
                .Returns(new List<Account> {accountStub.Object});
            var authorizer = new Authorizer(TimeSpan.FromHours(10), userRepoMock.Object);

            var token = authorizer.Authorize(accountStub.Object.Email, accountStub.Object.PasswordHash);
            var isAuthorized = authorizer.CheckAuthorized(token.Token, accountStub.Object.UserId);

            Assert.IsTrue(isAuthorized);
        }
    }
}