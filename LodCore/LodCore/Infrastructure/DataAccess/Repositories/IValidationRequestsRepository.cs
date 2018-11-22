using LodCore.Domain.UserManagement;

namespace LodCore.Infrastructure.DataAccess.Repositories
{
    public interface IValidationRequestsRepository
    {
        void SaveValidationRequest(MailValidationRequest request);

        MailValidationRequest GetMailValidationRequest(string token);

        void DeleteValidationToken(MailValidationRequest request);
    }
}