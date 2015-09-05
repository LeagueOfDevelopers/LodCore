using System;
using System.Threading.Tasks;

namespace UserManagement.Confirmation
{
    public interface IConfirmationService
    {
        Task SetupEmailConfirmation(uint userId);

        Task ConfirmEmail(string confirmationToken);

        Task ConfirmProfile(uint userId);
    }
}