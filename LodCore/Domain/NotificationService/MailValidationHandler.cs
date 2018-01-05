using System;
using UserManagement.Application;
using UserManagement.Domain;
using UserManagement.Infrastructure;
using Common;

namespace NotificationService
{
    public class MailValidationHandler : IEventConsumer<MailValidationRequest>
    {
        public MailValidationHandler(
			UserManagement.Application.IMailer mailer, 
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

			var confirmationLink = new Uri(
				_confirmationSettings.FrontendMailConfirmationUri,
				request.Token);
			_mailer.SendConfirmationMail(confirmationLink.AbsoluteUri,
				_userRepository.GetAccount(request.UserId).Email);
		}
		
		
        private readonly UserManagement.Application.IMailer _mailer;
        private readonly IValidationRequestsRepository _validationRequestsRepository;
        private readonly ConfirmationSettings _confirmationSettings;
        private readonly IUserRepository _userRepository;
    }
}
