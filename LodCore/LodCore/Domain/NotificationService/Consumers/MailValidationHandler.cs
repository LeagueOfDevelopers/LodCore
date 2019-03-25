using System;
using LodCore.Domain.UserManagement;
using LodCore.Infrastructure.DataAccess.Repositories;
using LodCore.Infrastructure.Mailing;

namespace LodCore.Domain.NotificationService
{
    public class MailValidationHandler : IEventConsumer<MailValidationRequest>
    {
        private readonly ConfirmationSettings _confirmationSettings;


        private readonly IMailer _mailer;
        private readonly IUserRepository _userRepository;
        private readonly IValidationRequestsRepository _validationRequestsRepository;

        public MailValidationHandler(
            IMailer mailer,
            IValidationRequestsRepository validationRequestsRepository,
            ConfirmationSettings confirmationSettings,
            IUserRepository userRepository)
        {
            _mailer = mailer;
            _validationRequestsRepository = validationRequestsRepository;
            _confirmationSettings = confirmationSettings;
            _userRepository = userRepository;
        }

        public void Consume(MailValidationRequest request)
        {
            _validationRequestsRepository.SaveValidationRequest(request);
            var user = _userRepository.GetAccount(request.UserId);
            var confirmationLink = new Uri(
                _confirmationSettings.FrontendMailConfirmationUri,
                request.Token);
            _mailer.SendConfirmationMail(user.Firstname, confirmationLink.AbsoluteUri,
                user.Email);
        }
    }
}