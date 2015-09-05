using System.Threading.Tasks;
using UserManagement;

namespace AccountService
{
    public interface IAccountService
    {
        Task<bool> CheckExists(User user);

        Task CreateAccount(AccountCredentials credentials);

        Task ChangePassword(uint userId);
    }

}
