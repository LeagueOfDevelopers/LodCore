using Journalist;
using UserManagement.Application;
using UserManagement.Infrastructure;

namespace UserManagement.Domain
{
    public class PasswordRecoveryManager : IPasswordRecoveryManager
    {
        private readonly IPasswordChangeRequestRepository _passwordChangeRequestRepository;
        private readonly IUserManager _userManager;

        public PasswordRecoveryManager(IPasswordChangeRequestRepository passwordChangeRequestRepository, IUserManager userManager)
        {
            _passwordChangeRequestRepository = passwordChangeRequestRepository;
            _userManager = userManager;
        }

        public Account GetUserByPasswordRecoveryToken(string token)
        {
            var request = _passwordChangeRequestRepository.GetPasswordChangeRequest(token);

            return request == null 
                ? null 
                : _userManager.GetUser(request.UserId);
        }

        public void UpdateUserPassword(int userId, string newPassword)
        {
            throw new System.NotImplementedException();
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