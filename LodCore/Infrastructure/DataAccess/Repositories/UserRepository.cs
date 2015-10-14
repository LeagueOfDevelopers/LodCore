using System;
using System.Collections.Generic;
using System.Linq;
using Journalist;
using NHibernate.Exceptions;
using NHibernate.Linq;
using UserManagement.Domain;
using UserManagement.Infrastructure;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
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
                var savedAccount = session.Save(account) as Account;
                // todo: perform check for NRE
                return savedAccount.UserId;
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

        private readonly DatabaseSessionProvider _sessionProvider;
    }
}