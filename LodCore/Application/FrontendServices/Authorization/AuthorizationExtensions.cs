using System.Security.Principal;
using UserManagement.Domain;

namespace FrontendServices.Authorization
{
    public static class AuthorizationExtensions
    {
        public static bool IsInRole(this IPrincipal principal, AccountRole role)
        {
            var lodPrincipal = principal as LodPrincipal;
            return lodPrincipal?.IsInRole(role) ?? false;
        }
    }
}