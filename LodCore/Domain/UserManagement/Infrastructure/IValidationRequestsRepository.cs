using UserManagement.Domain;

namespace UserManagement.Infrastructure
{
    public interface IValidationRequestsRepository
    {
        void SaveValidationRequest(MailValidationRequest request);

        int GetIdOfRequest(string token);
    }
}