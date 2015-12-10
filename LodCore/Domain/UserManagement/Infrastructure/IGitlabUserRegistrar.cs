using UserManagement.Application;

namespace UserManagement.Infrastructure
{
    public interface IGitlabUserRegistrar
    {
        int RegisterUser(CreateAccountRequest request);
    }
}