using System.Web.Http;
using Journalist;

namespace FrontendServices.Controllers.Admin
{
    public class ProjectsController : ApiController
    {
        public ProjectsController(IPermissionFilterFactory permissionFilterFactory)
        {
            Require.NotNull(permissionFilterFactory, nameof(permissionFilterFactory));
            _permissionFilterFactory = permissionFilterFactory;
        }

        private IPermissionFilterFactory _permissionFilterFactory;
    }
}
