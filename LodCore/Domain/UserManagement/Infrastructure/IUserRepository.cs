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

        Account GetAccountByGithubAccessToken(string githubAccessToken);

        List<Account> GetAllAccounts(Func<Account, bool> predicate = null);

        List<Account> GetSomeAccounts<TComparable>(int skipCount, int takeCount, Func<Account, TComparable> orderer, Func<Account, bool> criteria = null);
    }
}