using Journalist;
using UserManagement.Application;
using UserManagement.Infrastructure;

namespace UserManagement.Domain
{
    public class PasswordManager : IPasswordManager
    {
        private readonly IPasswordChangeRequestRepository _passwordChangeRequestRepository;
        private readonly IUserRepository _userRepository;

        public PasswordManager(IPasswordChangeRequestRepository passwordChangeRequestRepository, IUserRepository userRepository)
        {
            _passwordChangeRequestRepository = passwordChangeRequestRepository;
            _userRepository = userRepository;
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