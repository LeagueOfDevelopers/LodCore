using System;
using Journalist;
using LodCoreLibraryOld.Common;
using LodCoreLibraryOld.Domain.Exceptions;
using LodCoreLibraryOld.Domain.NotificationService;
using LodCoreLibraryOld.Infrastructure.DataAccess.Repositories;
using LodCoreLibraryOld.Infrastructure.EventBus;

namespace LodCoreLibraryOld.Domain.UserManagement
{
    public class ConfirmationService : IConfirmationService
    {
        private readonly IEventPublisher _eventPublisher;

        private readonly IUserRepository _userRepository;
        private readonly IValidationRequestsRepository _validationRequestsRepository;

        public ConfirmationService(
            IUserRepository userRepository,
            IValidationRequestsRepository validationRequestsRepository,
            IEventPublisher eventPublisher)
        {
            Require.NotNull(userRepository, nameof(userRepository));
            Require.NotNull(validationRequestsRepository, nameof(validationRequestsRepository));

            _userRepository = userRepository;
            _validationRequestsRepository = validationRequestsRepository;
            _eventPublisher = eventPublisher;
        }

        public void SetupEmailConfirmation(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var token = TokenGenerator.GenerateToken();
            var @event = new MailValidationRequest(userId, token);

            _eventPublisher.PublishEvent(@event);
        }

        public void ConfirmEmail(string confirmationToken)
        {
            Require.NotNull(confirmationToken, nameof(confirmationToken));

            var validationRequest = _validationRequestsRepository.GetMailValidationRequest(confirmationToken);
            if (validationRequest == null) throw new TokenNotFoundException();
            var userAccount = _userRepository.GetAccount(validationRequest.UserId);

            if (userAccount.ConfirmationStatus != ConfirmationStatus.Unconfirmed)
            {
                _validationRequestsRepository.DeleteValidationToken(validationRequest);
                throw new InvalidOperationException("Trying to confirm already confirmed profile ");
            }

            userAccount.ConfirmationStatus = ConfirmationStatus.EmailConfirmed;

            _userRepository.UpdateAccount(userAccount);

            _validationRequestsRepository.DeleteValidationToken(validationRequest);

            var @event =
                new NewEmailConfirmedDeveloper(userAccount.UserId, userAccount.Firstname, userAccount.Lastname);

            _eventPublisher.PublishEvent(@event);
        }

        public void ConfirmProfile(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var userAccount = _userRepository.GetAccount(userId);

            if (userAccount == null) throw new AccountNotFoundException();

            if (userAccount.ConfirmationStatus == ConfirmationStatus.FullyConfirmed)
                throw new InvalidOperationException("User is already confirmed");

            userAccount.ConfirmationStatus = ConfirmationStatus.FullyConfirmed;

            var @event = new NewFullConfirmedDeveloper(userAccount.UserId, userAccount.Firstname, userAccount.Lastname);

            _userRepository.UpdateAccount(userAccount);

            _eventPublisher.PublishEvent(@event);
        }
    }
}