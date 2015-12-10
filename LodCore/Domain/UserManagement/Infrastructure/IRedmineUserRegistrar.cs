using UserManagement.Application;

namespace UserManagement.Infrastructure
{
    public interface IRedmineUserRegistrar
    {
        int RegisterUser(CreateAccountRequest createAccountRequest);
    }
}