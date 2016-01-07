using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Journalist;
using UserManagement.Application;

namespace FrontendServices.Controllers
{
    public class DevelopersController : ApiController
    {
        public DevelopersController(IUserManager userManager)
        {
            Require.NotNull(userManager, nameof(userManager));
            _userManager = userManager;
        }

        private IUserManager _userManager;
    }
}
