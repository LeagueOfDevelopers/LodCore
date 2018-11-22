using System;
using System.Security.Principal;
using Serilog;
using LodCoreLibraryOld.Domain.UserManagement;

namespace LodCoreApiOld.Authorization
{
    public static class AuthorizationExtensions
    {
        public static bool IsInRole(this IPrincipal principal, AccountRole role)
        {
            var lodPrincipal = principal as LodPrincipal;
            return lodPrincipal?.IsInRole(role) ?? false;
        }

        public static int GetId(this IIdentity identity)
        {
            var lodIdentity = identity as LodIdentity;
            if (lodIdentity != null)
            {
                return lodIdentity.UserId;
            }
            var ex = new ArgumentException("Identity is not lod identity");
            Log.Warning(ex, ex.Message);
            throw ex;
        }

        public static void AssertResourceOwnerOrAdmin(this IPrincipal principal, int identityId)
        {
            var lodPrincipal = principal as LodPrincipal;
            if (lodPrincipal?.Identity.GetId() != identityId && !principal.IsInRole(AccountRole.Administrator))
            {
                var ex = new UnauthorizedAccessException();
                Log.Warning(ex, ex.Message);
                throw ex;
            }
        }

        public static void AssertResourceOwner(this IPrincipal principal, int identityId)
        {
            var lodPrincipal = principal as LodPrincipal;
            if (lodPrincipal?.Identity.GetId() != identityId)
            {
                var ex = new UnauthorizedAccessException();
                Log.Warning(ex, ex.Message);
                throw ex;
            }
        }
    }
}