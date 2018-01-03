using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Gateway;
using UserManagement.Domain;
using Journalist;
using FrontendServices.Authorization;

namespace FrontendServices.Controllers
{
    public class AuthController : ApiController
    {
        public AuthController(GithubGateway githubGateway)
        {
            Require.NotNull(githubGateway, nameof(githubGateway));

            _githubGateway = githubGateway;
        }

        [HttpGet]
        [Authorization(AccountRole.User)]
        [Route("auth/github")]
        public void GetRedirectionToAuthenticationGitHubForm()
        {

        }

        [HttpGet]
        [Authorization(AccountRole.User)]
        [Route("auth/github/callback/{code}")]
        public void GetAuthenticationTokenByCode()
        {

        }

        private readonly GithubGateway _githubGateway;
    }
}
