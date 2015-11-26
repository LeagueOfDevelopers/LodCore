using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Journalist;
using UserManagement.Application;
using UserManagement.Infrastructure;

namespace UserManagement.Domain
{
    class ConfirmationService : IConfirmationService
    {
        public ConfirmationService( IUserRepository userRepository, IMailer mailer, IValidationRequestsRepository validationRequestsRepository)
        {
            _userRepository = userRepository;
            _mailer = mailer;
            _validationRequestsRepository = validationRequestsRepository;
        }

        public void SetupEmailConfirmation(int userId)
        {
            Require.Positive(userId, nameof(userId));

            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            _mailer.SendConfirmationMail(token, _userRepository.GetAccount(userId).Email);

            MailValidationRequest request = new MailValidationRequest(userId, token);
            _validationRequestsRepository.SaveValidationRequest(request);
        }

        public void ConfirmEmail(string confirmationToken)
        {
            int userId = _validationRequestsRepository.GetIdOfRequest(confirmationToken);
            Account userAccount = _userRepository.GetAccount(userId);

            userAccount.ConfirmationStatus = ConfirmationStatus.EmailConfirmed;

            _userRepository.UpdateAccount(userAccount);
        }

        public void ConfirmProfile(int userId)
        {
            Account userAccount = _userRepository.GetAccount(userId);

            userAccount.ConfirmationStatus = ConfirmationStatus.FullyConfirmed;

            _userRepository.UpdateAccount(userAccount);
        }

        private IMailer _mailer;
        private IUserRepository _userRepository;
        private IValidationRequestsRepository _validationRequestsRepository;
    }
}
