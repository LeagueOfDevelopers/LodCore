using LodCoreLibraryOld.Domain.UserManagement;

namespace LodCoreLibraryOld.Infrastructure.DataAccess.Repositories
{
    public interface IValidationRequestsRepository
    {
        void SaveValidationRequest(MailValidationRequest request);

        MailValidationRequest GetMailValidationRequest(string token);

        void DeleteValidationToken(MailValidationRequest request);
    }
}