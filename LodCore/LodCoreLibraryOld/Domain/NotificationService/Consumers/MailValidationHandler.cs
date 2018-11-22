using LodCoreLibraryOld.Domain.UserManagement;
using LodCoreLibraryOld.Infrastructure.DataAccess.Repositories;
using LodCoreLibraryOld.Infrastructure.Mailing;
using System;

namespace LodCoreLibraryOld.Domain.NotificationService
{
    public class MailValidationHandler : IEventConsumer<MailValidationRequest>
    {
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
		
		
        private readonly IMailer _mailer;
        private readonly IValidationRequestsRepository _validationRequestsRepository;
        private readonly ConfirmationSettings _confirmationSettings;
        private readonly IUserRepository _userRepository;
    }
}
