using System;
using Journalist;
using UserManagement.Domain;
using UserManagement.Infrastructure;

namespace DataAccess.Repositories
{
    public class ValidationRequestsRepository : IValidationRequestsRepository
    {
        public ValidationRequestsRepository(DatabaseSessionProvider sessionProvider)
        {
            Require.NotNull(sessionProvider, nameof(sessionProvider));
            _sessionProvider = sessionProvider;
        }

        public void SaveValidationRequest(MailValidationRequest request)
        {
            Require.NotNull(request, nameof(request));

            using (var session = _sessionProvider.OpenSession())
            {
                session.Save(request);
            }
        }

        public MailValidationRequest GetMailValidatoinRequest(string token)
        {
            Require.NotNull(token, nameof(token));

            using (var session = _sessionProvider.OpenSession())
            {
                var request = session.Get<MailValidationRequest>(token);
                return request;
            }
        }

        private readonly DatabaseSessionProvider _sessionProvider;
    }
}
