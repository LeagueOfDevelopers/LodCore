using Common;
using FrontendServices.Authorization;
using Gateway;
using Journalist;
using ProjectManagement.Application;
using System;
using System.Collections.Generic;
using System.Web.Http;
using UserManagement.Domain;

namespace FrontendServices.Controllers
{
    public class GithubProjectController : ApiController
    {
        public GithubProjectController(IGithubGateway githubGateway)
        {
            Require.NotNull(githubGateway, nameof(githubGateway));

            _githubGateway = githubGateway;
        }

        [HttpGet]
        [Authorization(AccountRole.Administrator)]
        [Route("github/repositories")]
        public IEnumerable<GithubRepository> GetAllLeagueOfDevelopersRepositories()
        {
            return _githubGateway.GetLeagueOfDevelopersRepositories();
        }

        private readonly IGithubGateway _githubGateway;
    }
}
