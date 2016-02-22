using UserManagement.Domain;

namespace UserManagement.Infrastructure
{
    public interface IGitlabUserRegistrar
    {
        int RegisterUser(Account account);
    }
}