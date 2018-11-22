using System.Linq;
using Journalist;
using LodCoreLibraryOld.Common;
using LodCoreLibraryOld.Domain.UserManagement;
using NHibernate.Linq;

namespace LodCoreLibraryOld.Infrastructure.DataAccess.Repositories
{
    public class ValidationRequestsRepository : IValidationRequestsRepository
    {
        private readonly IDatabaseSessionProvider _sessionProvider;

        public ValidationRequestsRepository(IDatabaseSessionProvider sessionProvider)
        {
            Require.NotNull(sessionProvider, nameof(sessionProvider));
            _sessionProvider = sessionProvider;
        }

        public void SaveValidationRequest(MailValidationRequest request)
        {
            Require.NotNull(request, nameof(request));

            var session = _sessionProvider.GetCurrentSession();
            session.Save(request);
        }

        public MailValidationRequest GetMailValidationRequest(string token)
        {
            Require.NotNull(token, nameof(token));

            var session = _sessionProvider.GetCurrentSession();
            var request = session.Get<MailValidationRequest>(token);
            return request;
        }

        public void DeleteValidationToken(MailValidationRequest request)
        {
            Require.NotNull(request, nameof(request));

            var session = _sessionProvider.GetCurrentSession();
            session.Delete(request);
        }

        public MailValidationRequest GetMailValidationRequest(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var session = _sessionProvider.GetCurrentSession();
            var request = session.Query<MailValidationRequest>().Single(r => r.UserId == userId);
            return request;
        }
    }
}