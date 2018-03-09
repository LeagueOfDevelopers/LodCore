using FrontendServices.Authorization;
using Gateway;
using Journalist;
using System.Collections.Generic;
using System.Web.Http;
using UserManagement.Domain;
using ProjectManagement.Application;
using UserManagement.Application;

namespace FrontendServices.Controllers
{
    public class GithubProjectController : ApiController
    {
        public GithubProjectController(
            IGithubGateway githubGateway,
            IProjectProvider projectProvider,
            IUserManager userManager)
        {
            Require.NotNull(githubGateway, nameof(githubGateway));
            Require.NotNull(projectProvider, nameof(projectProvider));
            Require.NotNull(userManager, nameof(userManager));

            _githubGateway = githubGateway;
            _projectProvider = projectProvider;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorization(AccountRole.Administrator)]
        [Route("github/repositories")]
        public IEnumerable<GithubRepository> GetAllLeagueOfDevelopersRepositories()
        {
            return _githubGateway.GetLeagueOfDevelopersRepositories();
        }

        [HttpPost]
        [Authorization(AccountRole.Administrator)]
        [Route("github/repositories/{projectId}/developer/{developerId}")]
        public IHttpActionResult AddCollaboratorToProjectRepositories(int projectId, int developerId)
        {
            var project = _projectProvider.GetProject(projectId);
            var developer = _userManager.GetUser(developerId);
            _githubGateway.AddCollaboratorToRepository(developer, project);
            return Ok();
        }

        [HttpDelete]
        [Authorization(AccountRole.Administrator)]
        [Route("github/repositories/{projectId}/developer/{developerId}")]
        public IHttpActionResult RemoveCollaboratorFromProjectRepositories(int projectId, int developerId)
        {
            var project = _projectProvider.GetProject(projectId);
            var developer = _userManager.GetUser(developerId);
            _githubGateway.RemoveCollaboratorFromRepository(developer, project);
            return Ok();
        }

        private readonly IGithubGateway _githubGateway;
        private readonly IProjectProvider _projectProvider;
        private readonly IUserManager _userManager;
    }
}
