using LodCoreLibrary.Domain.UserManagement;

namespace LodCoreLibrary.Infrastructure.DataAccess.Repositories
{
    public interface IValidationRequestsRepository
    {
        void SaveValidationRequest(MailValidationRequest request);

        MailValidationRequest GetMailValidationRequest(string token);

        void DeleteValidationToken(MailValidationRequest request);
    }
}