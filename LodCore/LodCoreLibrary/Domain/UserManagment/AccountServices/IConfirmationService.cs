namespace LodCoreLibrary.Domain.UserManagement
{
    public interface IConfirmationService
    {
        void SetupEmailConfirmation(int userId);

        void ConfirmEmail(string confirmationToken);

        void ConfirmProfile(int userId);
    }
}