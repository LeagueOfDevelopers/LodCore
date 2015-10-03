using System;
using System.Collections.Generic;
using Journalist;
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
            throw new NotImplementedException();
        }

        public void UpdateAccount(Account account)
        {
            throw new NotImplementedException();
        }

        public Account GetAccount(int accountId)
        {
            throw new NotImplementedException();
        }

        public List<Account> GetAllAccounts(Func<Account, bool> predicate = null)
        {
            throw new NotImplementedException();
        }

        private DatabaseSessionProvider _sessionProvider;
    }
}