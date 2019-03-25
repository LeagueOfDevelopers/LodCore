using System.Linq;
using Journalist;
using LodCoreLibraryOld.Common;
using LodCoreLibraryOld.Domain.UserManagement;

namespace LodCoreLibraryOld.Infrastructure.DataAccess.Repositories
{
    public class PasswordChangeRequestRepository : IPasswordChangeRequestRepository
    {
        private readonly IDatabaseSessionProvider _sessionProvider;

        public PasswordChangeRequestRepository(IDatabaseSessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }

        public void SavePasswordChangeRequest(PasswordChangeRequest request)
        {
            Require.NotNull(request, nameof(request));

            var session = _sessionProvider.GetCurrentSession();

            session.Save(request);
        }

//    No needness in the function because of queue
        public PasswordChangeRequest GetPasswordChangeRequest(string token)
        {
            Require.NotNull(token, nameof(token));

            var session = _sessionProvider.GetCurrentSession();

            return session.Get<PasswordChangeRequest>(token);
        }

        public PasswordChangeRequest GetPasswordChangeRequest(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var session = _sessionProvider.GetCurrentSession();

            return session.QueryOver<PasswordChangeRequest>().Where(request => request.UserId == userId).List()
                .SingleOrDefault();
        }

        public void DeletePasswordChangeRequest(PasswordChangeRequest request)
        {
            Require.NotNull(request, nameof(request));

            var session = _sessionProvider.GetCurrentSession();

            session.Delete(request);
        }
    }
}