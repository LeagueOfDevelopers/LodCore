using UserManagement.Domain;

namespace UserManagement.Infrastructure
{
    internal interface IValidationRequestsRepository
    {
        void SaveValidationRequest(MailValidationRequest request);

        int GetIdOfRequest(string token);
    }
}