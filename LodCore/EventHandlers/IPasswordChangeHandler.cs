using UserManagement.Domain;

namespace EventHandlers
{
    public interface IPasswordChangeHandler
    {
        void ChangePassword(PasswordChangeRequest passwordChangeRequest);
    }
}
