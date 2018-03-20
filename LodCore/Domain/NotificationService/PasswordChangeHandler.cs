using Common;
using UserManagement.Application;
using UserManagement.Domain;
using UserManagement.Infrastructure;

namespace NotificationService
{
    public class PasswordChangeHandler : IEventConsumer<PasswordChangeRequest>
    {
        public PasswordChangeHandler(
            IMailer mailer,
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
		    _mailer.SendPasswordResetMail(user.Firstname, link, user.Email);
		}

        private readonly IMailer _mailer;
        private readonly IPasswordManager _passwordManager;
        private readonly ApplicationLocationSettings _applicationLocationSettings;
        private readonly IUserRepository _userRepository;
    }
}
