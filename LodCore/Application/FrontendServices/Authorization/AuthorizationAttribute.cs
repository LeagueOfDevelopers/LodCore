using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using UserManagement.Domain;

namespace FrontendServices.Authorization
{
    public class AuthorizationAttribute : AuthorizeAttribute
    {
        public AuthorizationAttribute(AccountRole accountRole)
        {
            _accountRole = accountRole;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return Thread.CurrentPrincipal.IsInRole(_accountRole);
        }

        private readonly AccountRole _accountRole;
    }
}