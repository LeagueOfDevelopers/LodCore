using System;
using System.Collections.Generic;
using UserManagement.Domain;

namespace UserManagement.Application
{
    public interface IUserManager
    {
        List<Account> GetUserList(Func<Account, bool> criteria = null);

        List<Account> GetUserList(int pageNumber, Func<Account, bool> criteria = null);

        List<Account> GetUserList(string searchString);

        Account GetUser(int userId);

        void CreateUser(CreateAccountRequest request);

        void UpdateUser(Account account);

        void InitiatePasswordChangingProcedure(int userId);
    }
}