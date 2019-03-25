using System;
using System.Collections.Generic;
using LodCoreLibraryOld.Domain.UserManagement;

namespace LodCoreLibraryOld.Infrastructure.DataAccess.Repositories
{
    public interface IUserRepository
    {
        int CreateAccount(Account account);

        void UpdateAccount(Account account);

        Account GetAccount(int accountId);

        Account GetAccountByLinkToGithubProfile(string linkToGithubProfile);

        List<Account> GetAllAccounts(Func<Account, bool> predicate = null);

        List<Account> GetSomeAccounts<TComparable>(int skipCount, int takeCount, Func<Account, TComparable> orderer,
            Func<Account, bool> criteria = null);
    }
}