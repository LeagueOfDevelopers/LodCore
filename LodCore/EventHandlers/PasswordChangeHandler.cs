using UserManagement.Domain;
using UserManagement.Application;
using Common;
using RabbitMQEventBus;
using UserManagement.Infrastructure;
using DataAccess;

namespace EventHandlers
{
    public class PasswordChangeHandler : IPasswordChangeHandler
    {
        public PasswordChangeHandler(
            IMailer mailer,
            IPasswordManager passwordManager,
            ApplicationLocationSettings applicationLocationSettings,
            IEventBus eventBus,
            IUserRepository userRepository,
            DatabaseSessionProvider databaseSessionProvider)
        {
            _mailer = mailer;
            _passwordManager = passwordManager;
            _applicationLocationSettings = applicationLocationSettings;
            _eventBus = eventBus;
            _userRepository = userRepository;
            _databaseSessionProvider = databaseSessionProvider;

            BindToConsumer();
        }

        public void ChangePassword(PasswordChangeRequest request)
        {
            _databaseSessionProvider.OpenSession();
            var user = _userRepository.GetAccount(request.UserId);
            var link = $"{_applicationLocationSettings.FrontendAdress}/password/recovery/{request.Token}";
            _passwordManager.SavePasswordChangeRequest(request);
            _mailer.SendPasswordResetMail(link, user.Email);
            _databaseSessionProvider.CloseSession();
        }

        private void BindToConsumer()
        {
            _changePassword changePassword = ChangePassword;
            _eventBus.SetConsumer("ChangePassword", changePassword);
        }

        private delegate void _changePassword(PasswordChangeRequest passwordChangeRequest);
        private readonly IMailer _mailer;
        private readonly IPasswordManager _passwordManager;
        private readonly ApplicationLocationSettings _applicationLocationSettings;
        private readonly IEventBus _eventBus;
        private readonly IUserRepository _userRepository;
        private readonly DatabaseSessionProvider _databaseSessionProvider;
    }
}
