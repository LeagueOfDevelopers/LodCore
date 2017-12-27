using System;
using Common;
using Journalist;
using NotificationService;
using UserManagement.Application;
using UserManagement.Domain.Events;
using UserManagement.Infrastructure;
using IMailer = UserManagement.Application.IMailer;
using RabbitMQEventBus;

namespace UserManagement.Domain
{
    public class ConfirmationService : IConfirmationService
    {
        public ConfirmationService(
            IUserRepository userRepository,
            IMailer mailer,
            IValidationRequestsRepository validationRequestsRepository, 
            IEventSink userManagementEventSink, 
            ConfirmationSettings confirmationSettings,
            IEventBus eventBus)
        {
            Require.NotNull(userRepository, nameof(userRepository));
            Require.NotNull(mailer, nameof(mailer));
            Require.NotNull(validationRequestsRepository, nameof(validationRequestsRepository));
            Require.NotNull(userManagementEventSink, nameof(userManagementEventSink));
            Require.NotNull(confirmationSettings, nameof(confirmationSettings));

            _userRepository = userRepository;
            _mailer = mailer;
            _validationRequestsRepository = validationRequestsRepository;
            _userManagementEventSink = userManagementEventSink;
            _confirmationSettings = confirmationSettings;
            _eventBus = eventBus;
        }

        public void SetupEmailConfirmation(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var token = TokenGenerator.GenerateToken();
            var @event = new MailValidationRequest(userId, token);

            _eventBus.PublishEvent("MailValidationRequest", "validate_mail", @event);
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

            if (userAccount.ConfirmationStatus != ConfirmationStatus.Unconfirmed)
            {
                _validationRequestsRepository.DeleteValidationToken(validationRequest);
                throw new InvalidOperationException("Trying to confirm already confirmed profile ");    
            }

            userAccount.ConfirmationStatus = ConfirmationStatus.EmailConfirmed;

            _userRepository.UpdateAccount(userAccount);

            _validationRequestsRepository.DeleteValidationToken(validationRequest);

            var @event = new NewEmailConfirmedDeveloper(userAccount.UserId);

            _eventBus.PublishEvent("Notification", "new_email_confirmed_developer", @event);
        }

        public void ConfirmProfile(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var userAccount = _userRepository.GetAccount(userId);

            if (userAccount == null)
            {
                throw new AccountNotFoundException();
            }

            if (userAccount.ConfirmationStatus == ConfirmationStatus.FullyConfirmed)
            {
                throw new InvalidOperationException("User is already confirmed");
            }

            userAccount.ConfirmationStatus = ConfirmationStatus.FullyConfirmed;

            userAccount.Password = userAccount.Password.GetHashed();
            _userRepository.UpdateAccount(userAccount);

            var @event = new NewFullConfirmedDeveloper(userAccount.UserId);

            _eventBus.PublishEvent("Notification", "new_full_confirmed_developer", @event);
        }

        private readonly IMailer _mailer;
        private readonly IEventSink _userManagementEventSink;
        private readonly IUserRepository _userRepository;
        private readonly IValidationRequestsRepository _validationRequestsRepository;
        private readonly ConfirmationSettings _confirmationSettings;
        private readonly IEventBus _eventBus;
    }
}