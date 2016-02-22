using UserManagement.Application;
using UserManagement.Domain;

namespace UserManagement.Infrastructure
{
    public interface IRedmineUserRegistrar
    {
        int RegisterUser(Account account);
    }
}