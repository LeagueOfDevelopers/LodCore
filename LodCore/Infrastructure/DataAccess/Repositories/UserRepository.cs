using System;
using System.Collections.Generic;
using System.Linq;
using Journalist;
using Journalist.Extensions;
using NHibernate.Linq;
using NotificationService;
using UserManagement.Domain;
using UserManagement.Infrastructure;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository, IUsersRepository
    {
        private readonly DatabaseSessionProvider _sessionProvider;

        public UserRepository(DatabaseSessionProvider sessionProvider)
        {
            Require.NotNull(sessionProvider, nameof(sessionProvider));
            _sessionProvider = sessionProvider;
        }

        public int CreateAccount(Account account)
        {
            Require.NotNull(account, nameof(account));

            using (var session = _sessionProvider.OpenSession())
            {
                var savedAccountId = (int) session.Save(account);
                // todo: perform check for NRE
                return savedAccountId;
            }
        }

        public void UpdateAccount(Account account)
        {
            Require.NotNull(account, nameof(account));

            using (var session = _sessionProvider.OpenSession())
            {
                session.Update(account);
            }
        }

        public Account GetAccount(int accountId)
        {
            Require.Positive(accountId, nameof(accountId));

            using (var session = _sessionProvider.OpenSession())
            {
                var account = session.Get<Account>(accountId);
                return account;
            }
        }

        public List<Account> GetAllAccounts(Func<Account, bool> predicate = null)
        {
            using (var session = _sessionProvider.OpenSession())
            {
                return predicate == null
                    ? session.Query<Account>().ToList()
                    : session.Query<Account>().Where(predicate).ToList();
            }
        }

        public int[] GetAllUsersIds()
        {
            using (var session = _sessionProvider.OpenSession())
            {
                return session.Query<Account>().SelectToArray(account => account.UserId);
            }
        }

        public int[] GetAllAdminIds()
        {
            using (var session = _sessionProvider.OpenSession())
            {
                return session.Query<Account>()
                    .Where(account => account.Role == AccountRole.Administrator)
                    .SelectToArray(account => account.UserId);
            }
        }
    }
}