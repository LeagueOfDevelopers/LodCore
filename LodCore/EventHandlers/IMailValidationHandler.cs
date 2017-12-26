using UserManagement.Domain;

namespace EventHandlers
{
    public interface IMailValidationHandler
    {
        void ValidateMail(MailValidationRequest mailValidationRequest);
    }
}
