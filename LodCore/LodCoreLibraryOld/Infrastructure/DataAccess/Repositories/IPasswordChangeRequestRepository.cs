using LodCoreLibraryOld.Domain.UserManagement;

namespace LodCoreLibraryOld.Infrastructure.DataAccess.Repositories
{
    public interface IPasswordChangeRequestRepository
    {
        void SavePasswordChangeRequest(PasswordChangeRequest request);

        PasswordChangeRequest GetPasswordChangeRequest(string token);

        PasswordChangeRequest GetPasswordChangeRequest(int userId);

        void DeletePasswordChangeRequest(PasswordChangeRequest request);
    }
}