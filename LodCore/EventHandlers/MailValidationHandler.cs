using System;
using UserManagement.Application;
using UserManagement.Infrastructure;
using UserManagement.Domain;
using DataAccess.Repositories;
using RabbitMQEventBus;

namespace EventHandlers
{
    public class MailValidationHandler : IMailValidationHandler
    {
        public MailValidationHandler(IMailer mailer, 
            IValidationRequestsRepository validationRequestsRepository,
            ConfirmationSettings confirmationSettings,
            IUserRepository userRepository)
        {
            _mailer = mailer;
            _validationRequestsRepository = validationRequestsRepository;
            _confirmationSettings = confirmationSettings;
            _userRepository = userRepository;
            _validateMail validateMail = ValidateMail;
            EventBus.SetConsumer("ValidateMail", validateMail);
        }
        
        public void ValidateMail(MailValidationRequest request)
        {
          //  _validationRequestsRepository.SaveValidationRequest(request);

            var confirmationLink = new Uri(
                _confirmationSettings.FrontendMailConfirmationUri,
                request.Token);
            _mailer.SendConfirmationMail(confirmationLink.AbsoluteUri,
                                         _userRepository.GetAccount(request.UserId).Email);
        }

        private delegate void _validateMail(MailValidationRequest mailValidationRequest);
        private readonly IMailer _mailer;
        private readonly IValidationRequestsRepository _validationRequestsRepository;
        private readonly ConfirmationSettings _confirmationSettings;
        private readonly IUserRepository _userRepository;  
    }
}
