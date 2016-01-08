using UserManagement.Domain;

namespace UserManagement.Infrastructure
{
    public interface IValidationRequestsRepository
    {
        void SaveValidationRequest(MailValidationRequest request);

        MailValidationRequest GetMailValidationRequest(string token);
    }
}