using System.Security.Principal;
using Journalist;
using UserManagement.Domain;

namespace FrontendServices.Authorization
{
    public class LodPrincipal : IPrincipal
    {
        private readonly AccountRole _accountRole;

        public LodPrincipal(AccountRole accountRole, IIdentity identity)
        {
            Require.NotNull(identity, nameof(identity));

            _accountRole = accountRole;
            Identity = identity;
        }

        public bool IsEmpty { get; private set; }

        public static IPrincipal EmptyPrincipal
            => new LodPrincipal(AccountRole.User, LodIdentity.EmptyIdentity) {IsEmpty = true};

        public bool IsInRole(string role)
        {
            return !IsEmpty && _accountRole.ToString("G").Equals(role);
        }

        public IIdentity Identity { get; }

        public bool IsInRole(AccountRole role)
        {
            return (_accountRole == AccountRole.Administrator || _accountRole == role) && !IsEmpty;
        }
    }
}