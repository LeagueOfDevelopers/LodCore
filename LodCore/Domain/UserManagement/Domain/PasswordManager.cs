using Journalist;
using UserManagement.Application;
using UserManagement.Infrastructure;

namespace UserManagement.Domain
{
    public class PasswordManager : IPasswordManager
    {
        private readonly IPasswordChangeRequestRepository _passwordChangeRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGitlabUserRegistrar _gitlabUserRegistrar;
        private readonly IRedmineUserRegistrar _redmineUserRegistrar;

        public PasswordManager(IPasswordChangeRequestRepository passwordChangeRequestRepository, IUserRepository userRepository, IGitlabUserRegistrar gitlabUserRegistrar, IRedmineUserRegistrar redmineUserRegistrar)
        {
            _passwordChangeRequestRepository = passwordChangeRequestRepository;
            _userRepository = userRepository;
            _gitlabUserRegistrar = gitlabUserRegistrar;
            _redmineUserRegistrar = redmineUserRegistrar;
        }

        public Account GetUserByPasswordRecoveryToken(string token)
        {
            var request = _passwordChangeRequestRepository.GetPasswordChangeRequest(token);

            return request == null 
                ? null 
                : _userRepository.GetAccount(request.UserId);
        }

        public void UpdateUserPassword(int userId, string newPassword)
        {
            Require.Positive(userId, nameof(userId));
            Require.NotEmpty(newPassword, nameof(newPassword));

            var account = _userRepository.GetAccount(userId);

            _redmineUserRegistrar.ChangeUserPassword(account, newPassword);
            _gitlabUserRegistrar.ChangeUserPassword(account, newPassword);
        }

        public void SavePasswordChangeRequest(PasswordChangeRequest request)
        {
            Require.NotNull(request, nameof(request));

            _passwordChangeRequestRepository.SavePasswordChangeRequest(request);
        }

        public PasswordChangeRequest GetPasswordChangeRequest(string token)
        {
            Require.NotEmpty(token, nameof(token));

            return _passwordChangeRequestRepository.GetPasswordChangeRequest(token);
        }

        public PasswordChangeRequest GetPasswordChangeRequest(int userId)
        {
            Require.Positive(userId, nameof(userId));

            return _passwordChangeRequestRepository.GetPasswordChangeRequest(userId);
        }

        public void DeletePasswordChangeRequest(PasswordChangeRequest request)
        {
            Require.NotNull(request, nameof(request));

            _passwordChangeRequestRepository.DeletePasswordChangeRequest(request);
        }
    }
}