using FrontendServices.Authorization;
using Gateway;
using Journalist;
using System.Collections.Generic;
using System.Web.Http;
using UserManagement.Domain;
using ProjectManagement.Application;
using UserManagement.Application;
using Common;
using System;

namespace FrontendServices.Controllers
{
    public class GithubProjectController : ApiController
    {
        public GithubProjectController(
            IGithubGateway githubGateway,
            IProjectProvider projectProvider,
            IUserManager userManager,
            ApplicationLocationSettings applicationLocationSettings)
        {
            Require.NotNull(githubGateway, nameof(githubGateway));
            Require.NotNull(projectProvider, nameof(projectProvider));
            Require.NotNull(userManager, nameof(userManager));
            Require.NotNull(applicationLocationSettings, nameof(applicationLocationSettings));

            _githubGateway = githubGateway;
            _projectProvider = projectProvider;
            _userManager = userManager;
            _applicationLocationSettings = applicationLocationSettings;
        }

        [HttpGet]
        [Authorization(AccountRole.Administrator)]
        [Route("github/repositories")]
        public IEnumerable<GithubRepository> GetAllLeagueOfDevelopersRepositories()
        {
            return _githubGateway.GetLeagueOfDevelopersRepositories();
        }

        [HttpGet]
        [Authorization(AccountRole.Administrator)]
        [Route("github/repositories/{projectId}/developer/{developerId}")]
        public string GetLinkToGithubLoginPageToCheckAuthentication(int projectId, int developerId)
        {
            return _githubGateway.GetLinkToGithubLoginPage(projectId, developerId);
        }

        [HttpGet]
        [Route("github/callback/repositories/{projectId}/developer/{developerId}")]
        public IHttpActionResult AddCollaboratorToProjectRepositories(int projectId, int developerId, string code, string state)
        {
            var project = _projectProvider.GetProject(projectId);
            var developer = _userManager.GetUser(developerId);
            try
            {
                var githubAccessToken = _githubGateway.GetToken(code, state);
                _githubGateway.AddCollaboratorToRepository(githubAccessToken, developer, project);
            }
            catch (Exception)
            {
                return Redirect($"{_applicationLocationSettings.FrontendAdress}/error/admin");
            }
            return Redirect($"{_applicationLocationSettings.FrontendAdress}/success/admin");
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
        private readonly ApplicationLocationSettings _applicationLocationSettings;
    }
}
