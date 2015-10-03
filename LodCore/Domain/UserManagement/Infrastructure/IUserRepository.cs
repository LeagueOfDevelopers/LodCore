using System;
using System.Collections.Generic;
using UserManagement.Domain;

namespace UserManagement.Infrastructure
{
    public interface IUserRepository
    {
        int CreateAccount(Account account);

        void UpdateAccount(Account account);

        Account GetAccount(int accountId);

        List<Account> GetAllAccounts(Func<Account, bool> predicate = null);
    }
}