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

        List<Account> GetSomeAccounts(int skipCount, int takeCount, Func<Account, bool> criteria = null);

        List<Account> SearchAccounts(string searchString);
    }
}