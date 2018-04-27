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

        Account GetUserByLinkToGithubProfile(string linkToGithubProfile);

        int CreateUserTemplate(CreateAccountRequest accountRequest);

        int CreateUser(CreateAccountRequest request);

        void UpdateUser(Account account);

        void ChangeUserPassword(int userId, string newPassword);

        void InitiatePasswordChangingProcedure(int userId);
    }
}