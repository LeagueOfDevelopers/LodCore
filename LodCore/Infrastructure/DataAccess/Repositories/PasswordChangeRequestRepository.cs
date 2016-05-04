using System.Linq;
using Journalist;
using NHibernate.Linq;
using UserManagement.Domain;
using UserManagement.Infrastructure;

namespace DataAccess.Repositories
{
    public class PasswordChangeRequestRepository : IPasswordChangeRequestRepository
    {
        private readonly DatabaseSessionProvider _sessionProvider;

        public PasswordChangeRequestRepository(DatabaseSessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }

        public void SavePasswordChangeRequest(PasswordChangeRequest request)
        {
            Require.NotNull(request, nameof(request));

            var session = _sessionProvider.GetCurrentSession();

            session.Save(request);
        }

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

            return session.QueryOver<PasswordChangeRequest>().Where(request => request.UserId == userId).List().SingleOrDefault();
        }

        public void DeletePasswordChangeRequest(PasswordChangeRequest request)
        {
            Require.NotNull(request, nameof(request));

            var session = _sessionProvider.GetCurrentSession();

            session.Delete(request);
        }
    }
}
