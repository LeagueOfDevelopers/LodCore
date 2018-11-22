using LodCoreLibraryOld.Domain.UserManagement;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace LodCoreApiOld.Authorization
{
    public class AuthorizationAttribute : AuthorizeAttribute
    {
        private readonly AccountRole _accountRole;

        public AuthorizationAttribute(AccountRole accountRole)
        {
            _accountRole = accountRole;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return Thread.CurrentPrincipal.IsInRole(_accountRole);
        }
    }
}