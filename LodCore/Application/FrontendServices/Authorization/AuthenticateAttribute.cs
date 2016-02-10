using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Journalist;
using UserManagement.Application;

namespace FrontendServices.Authorization
{
    public class AuthenticateAttribute : IAuthenticationFilter
    {
        public AuthenticateAttribute(IAuthorizer authorizer)
        {
            Require.NotNull(authorizer, nameof(authorizer));

            _authorizer = authorizer;
        }

        public bool AllowMultiple => false;

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var tokenString = context?.Request?.Headers?.Authorization?.Parameter;
            if (string.IsNullOrEmpty(tokenString))
            {
                SetupUnauthenticated();
                return Task.CompletedTask;
            }

            var tokenInfo = _authorizer.GetTokenInfo(tokenString);
            if (tokenInfo == null)
            {
                SetupUnauthenticated();
                return Task.CompletedTask;
            }
            
            var identity = new LodIdentity(tokenInfo.UserId, true);
            var principal = new LodPrincipal(tokenInfo.Role, identity);

            Thread.CurrentPrincipal = principal;
            context.Principal = principal;

            return Task.CompletedTask;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void SetupUnauthenticated()
        {
            Thread.CurrentPrincipal = LodPrincipal.EmptyPrincipal;
        }

        private readonly IAuthorizer _authorizer;
    }
}