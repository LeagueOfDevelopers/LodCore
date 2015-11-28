using System;
using System.Collections.Generic;
using UserManagement.Domain;

namespace UserManagement.Application
{
    public interface IUserManager
    {
        List<Account> GetUserList(Func<Account, bool> criteria = null);

        Account GetUser(int userId);

        void CreateUser(CreateAccountRequest request);

        void UpdateUser(Account account);
    }
}