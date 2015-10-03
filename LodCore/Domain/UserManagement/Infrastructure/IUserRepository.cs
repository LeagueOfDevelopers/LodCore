using System;
using UserManagement.Domain;

namespace UserManagement.Infrastructure
{
    public interface IUserRepository
    {
        void CreateAccount(Account account);

        void UpdateAccount(Account account);

        Account GetAccount(int accountId);

        Account GetAllAccounts(Func<bool> predicate = null);
    }
}