using Common;
using RabbitMQEventBus;
using UserManagement.Application;
using UserManagement.Domain;
using UserManagement.Infrastructure;

namespace NotificationService
{
    public class PasswordChangeHandler : IEventConsumer<PasswordChangeRequest>
    {
        public PasswordChangeHandler(
            UserManagement.Application.IMailer mailer,
            IPasswordManager passwordManager,
            ApplicationLocationSettings applicationLocationSettings,
            IUserRepository userRepository)
        {
            _mailer = mailer;
            _passwordManager = passwordManager;
            _applicationLocationSettings = applicationLocationSettings;
            _userRepository = userRepository;
        }

	    public void Consume(PasswordChangeRequest request)
	    {
			var user = _userRepository.GetAccount(request.UserId);
		    var link = $"{_applicationLocationSettings.FrontendAdress}/password/recovery/{request.Token}";
		    _passwordManager.SavePasswordChangeRequest(request);
		    _mailer.SendPasswordResetMail(link, user.Email);
		}

        private readonly UserManagement.Application.IMailer _mailer;
        private readonly IPasswordManager _passwordManager;
        private readonly ApplicationLocationSettings _applicationLocationSettings;
        private readonly IUserRepository _userRepository;
    }
}
