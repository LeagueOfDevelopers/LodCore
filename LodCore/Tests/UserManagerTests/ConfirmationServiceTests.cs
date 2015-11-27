using System;
using DataAccess.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using  UserManagement.Domain;

namespace UserManagerTests
{
    [TestClass]
    public class ConfirmationServiceTests
    {
        [TestMethod]
        public void ConfirmationServiceSaveTheToken()
        {
            var random = new Random();
            var userId = random.Next(1, 500);

            _confirmationService.SetupEmailConfirmation(userId);
            
        }

        [TestMethod]
        public void ConfirmationServiceConfirmsEmail()
        {
            var random = new Random();
            var userId = random.Next(1, 500);

            _confirmationService.SetupEmailConfirmation(userId);

            //_confirmationService.ConfirmEmail(/*тут нужен токен, но где его сука взять?*/);

            Assert.IsTrue(_userRepository.GetAccount(userId).ConfirmationStatus == ConfirmationStatus.EmailConfirmed);
        }

        [TestMethod]
        public void ConfirmationServiceConfirmsProfile()
        {
            
        }

        private ConfirmationService _confirmationService;
        private UserRepository _userRepository;
    }
}
