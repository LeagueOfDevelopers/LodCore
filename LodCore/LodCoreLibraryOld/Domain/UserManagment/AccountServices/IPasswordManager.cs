namespace LodCoreLibraryOld.Domain.UserManagement
{
    public interface IPasswordManager
    {
        Account GetUserByPasswordRecoveryToken(string token);
        void UpdateUserPassword(int userId, string newPassword);

        void SavePasswordChangeRequest(PasswordChangeRequest request);

        PasswordChangeRequest GetPasswordChangeRequest(string token);

        PasswordChangeRequest GetPasswordChangeRequest(int userId);

        void DeletePasswordChangeRequest(PasswordChangeRequest request);
    }
}