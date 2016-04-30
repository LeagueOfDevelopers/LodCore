using System;
using System.Text.RegularExpressions;
using Common;
using Journalist;
using NotificationService;
using UserManagement.Application;
using UserManagement.Domain.Events;
using UserManagement.Infrastructure;
using IMailer = UserManagement.Application.IMailer;

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
            IGitlabUserRegistrar gitlabUserRegistrar, 
            IRedmineUserRegistrar redmineUserRegistrar)
        {
            Require.NotNull(userRepository, nameof(userRepository));
            Require.NotNull(mailer, nameof(mailer));
            Require.NotNull(validationRequestsRepository, nameof(validationRequestsRepository));
            Require.NotNull(userManagementEventSink, nameof(userManagementEventSink));
            Require.NotNull(confirmationSettings, nameof(confirmationSettings));
            Require.NotNull(gitlabUserRegistrar, nameof(gitlabUserRegistrar));
            Require.NotNull(redmineUserRegistrar, nameof(redmineUserRegistrar));

            _userRepository = userRepository;
            _mailer = mailer;
            _validationRequestsRepository = validationRequestsRepository;
            _userManagementEventSink = userManagementEventSink;
            _confirmationSettings = confirmationSettings;
            _gitlabUserRegistrar = gitlabUserRegistrar;
            _redmineUserRegistrar = redmineUserRegistrar;
        }

        public void SetupEmailConfirmation(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var token = Extensions.GenerateToken();
            var request = new MailValidationRequest(userId, token);
            _validationRequestsRepository.SaveValidationRequest(request);

            var confirmationLink = new Uri(
                _confirmationSettings.FrontendMailConfirmationUri,
                token);
            _mailer.SendConfirmationMail(confirmationLink.AbsoluteUri, _userRepository.GetAccount(userId).Email);
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

            if (userAccount.ConfirmationStatus == ConfirmationStatus.FullyConfirmed)
            {
                throw new InvalidOperationException("User is already confirmed");
            }

            userAccount.ConfirmationStatus = ConfirmationStatus.FullyConfirmed;

            try
            {
                if (_confirmationSettings.GitlabAccountCreationEnabled)
                {
                    CreateGitlabAccount(userAccount);    
                }

                if (_confirmationSettings.RedmineAccountCreationEnabled)
                {
                    CreateRedmineAccount(userAccount);
                }
            }
            catch (Exception exception)
            {
                throw new ConfirmationFailedException(exception.Message);
            }

            userAccount.Password = userAccount.Password.GetHashed();
            _userRepository.UpdateAccount(userAccount);

            _userManagementEventSink.ConsumeEvent(new NewFullConfirmedDeveloper(userId));
        }

        private void CreateGitlabAccount(Account account)
        {
            var gitlabUserId = _gitlabUserRegistrar.RegisterUser(account);
            account.GitlabUserId = gitlabUserId;
        }

        private void CreateRedmineAccount(Account account)
        {
            var redmineUserId = _redmineUserRegistrar.RegisterUser(account);
            account.RedmineUserId = redmineUserId;
        }

        private readonly IMailer _mailer;
        private readonly IEventSink _userManagementEventSink;
        private readonly IUserRepository _userRepository;
        private readonly IValidationRequestsRepository _validationRequestsRepository;
        private readonly ConfirmationSettings _confirmationSettings;
        private readonly IGitlabUserRegistrar _gitlabUserRegistrar;
        private readonly IRedmineUserRegistrar _redmineUserRegistrar;
    }
}