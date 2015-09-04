using System;
using System.Threading.Tasks;

namespace UserManagement
{
    public interface IUserManager
    {
        Task<User[]> GetUserList(Predicate<User> criteria = null);

        Task<User> GetUser(uint userId);

        Task<UserSummary> GetUserSummary(uint userId);
        
        Task CreateUser(User user);

        Task UpdateUser(User user);
    }
}