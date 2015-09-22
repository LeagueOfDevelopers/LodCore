using System.Threading.Tasks;

namespace UserManagement.Application
{
    public interface IConfirmationService
    {
        Task SetupEmailConfirmation(int userId);

        Task ConfirmEmail(string confirmationToken);

        Task ConfirmProfile(uint userId);
    }
}