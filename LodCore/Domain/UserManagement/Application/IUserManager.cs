using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Domain;

namespace UserManagement.Application
{
    public interface IUserManager
    {
        List<Account> GetUserList(Func<Account, bool> criteria = null);

        Account GetUser(int userId);
        
        void CreateUser(CreateAccountRequest request);

        Task UpdateUser(Account account);
    }
}
