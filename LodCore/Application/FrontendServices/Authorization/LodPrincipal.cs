using System.Security.Principal;
using Journalist;
using UserManagement.Domain;

namespace FrontendServices.Authorization
{
    public class LodPrincipal : IPrincipal
    {
        public LodPrincipal(AccountRole accountRole, IIdentity identity)
        {
            Require.NotNull(identity, nameof(identity));

            _accountRole = accountRole;
            Identity = identity;
        }

        public bool IsInRole(string role)
        {
            return !IsEmpty && _accountRole.ToString("G").Equals(role);
        }

        public bool IsInRole(AccountRole role)
        {
            return (_accountRole == AccountRole.Administrator || _accountRole == role) && !IsEmpty;
        }

        public bool IsEmpty { get; private set; }

        public static IPrincipal EmptyPrincipal 
            => new LodPrincipal(AccountRole.User, LodIdentity.EmptyIdentity) { IsEmpty = true };

        public IIdentity Identity { get; }

        private readonly AccountRole _accountRole;
    }
}