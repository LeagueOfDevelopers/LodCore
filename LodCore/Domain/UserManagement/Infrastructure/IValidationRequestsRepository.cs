using UserManagement.Domain;

namespace UserManagement.Infrastructure
{
    public interface IValidationRequestsRepository
    {
        void SaveValidationRequest(MailValidationRequest request);

        MailValidationRequest GetMailValidatoinRequest(string token);
    }
}