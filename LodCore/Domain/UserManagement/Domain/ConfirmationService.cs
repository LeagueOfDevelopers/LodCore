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
    public class ConfirmationService : IConfirmationService
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

            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            _mailer.SendConfirmationMail(token, _userRepository.GetAccount(userId).Email);

            var request = new MailValidationRequest(userId, token);
            _validationRequestsRepository.SaveValidationRequest(request);
        }

        public void ConfirmEmail(string confirmationToken)
        {
            var validationRequest = _validationRequestsRepository.GetMailValidatoinRequest(confirmationToken);
            if (validationRequest == null)
            {
                throw new TokenNotFoundException();
            }
            var userAccount = _userRepository.GetAccount(validationRequest.UserId);

            userAccount.ConfirmationStatus = ConfirmationStatus.EmailConfirmed;

            _userRepository.UpdateAccount(userAccount);
        }

        public void ConfirmProfile(int userId)
        {
            var userAccount = _userRepository.GetAccount(userId);

            userAccount.ConfirmationStatus = ConfirmationStatus.FullyConfirmed;

            _userRepository.UpdateAccount(userAccount);
        }

        private readonly IMailer _mailer;
        private readonly IUserRepository _userRepository;
        private readonly IValidationRequestsRepository _validationRequestsRepository;
    }
}
