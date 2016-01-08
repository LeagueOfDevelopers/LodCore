using System;
using Journalist;
using NotificationService;
using UserManagement.Application;
using UserManagement.Domain.Events;
using UserManagement.Infrastructure;

namespace UserManagement.Domain
{
    public class ConfirmationService : IConfirmationService
    {
        private readonly IMailer _mailer;
        private readonly IEventSink _userManagementEventSink;
        private readonly IUserRepository _userRepository;
        private readonly IValidationRequestsRepository _validationRequestsRepository;

        public ConfirmationService(IUserRepository userRepository, IMailer mailer,
            IValidationRequestsRepository validationRequestsRepository, IEventSink userManagementEventSink)
        {
            _userRepository = userRepository;
            _mailer = mailer;
            _validationRequestsRepository = validationRequestsRepository;
            _userManagementEventSink = userManagementEventSink;
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
            Require.NotNull(confirmationToken, nameof(confirmationToken));

            var validationRequest = _validationRequestsRepository.GetMailValidationRequest(confirmationToken);
            if (validationRequest == null)
            {
                throw new TokenNotFoundException();
            }
            var userAccount = _userRepository.GetAccount(validationRequest.UserId);

            userAccount.ConfirmationStatus = ConfirmationStatus.EmailConfirmed;

            _userRepository.UpdateAccount(userAccount);

            _userManagementEventSink.ConsumeEvent(new NewEmailConfirmedDeveloper(userAccount.UserId));
        }

        public void ConfirmProfile(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var userAccount = _userRepository.GetAccount(userId);

            if (userAccount == null)
            {
                throw new AccountNotFoundException();
            }

            userAccount.ConfirmationStatus = ConfirmationStatus.FullyConfirmed;

            _userRepository.UpdateAccount(userAccount);

            _userManagementEventSink.ConsumeEvent(new NewFullConfirmedDeveloper(userId));
        }
    }
}