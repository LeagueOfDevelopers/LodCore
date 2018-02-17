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
        public GithubProjectController(
            IGithubGateway githubGateway,
            ApplicationLocationSettings applicationLocationSettings,
            IProjectProvider projectProvider)
        {
            Require.NotNull(githubGateway, nameof(githubGateway));
            Require.NotNull(applicationLocationSettings, nameof(applicationLocationSettings));
            Require.NotNull(projectProvider, nameof(projectProvider));

            _githubGateway = githubGateway;
            _applicationLocationSettings = applicationLocationSettings;
            _projectProvider = projectProvider;
        }

        [HttpGet]
        [Authorization(AccountRole.Administrator)]
        [Route("projects/github")]
        public IEnumerable<string> GetAllLeagueOfDevelopersRepositories()
        {
            return _githubGateway.GetLeagueOfDevelopersRepositories();
        }

        [HttpPost]
        [Authorization(AccountRole.Administrator)]
        [Route("projects/{projectId}/github")]
        public IHttpActionResult GetLinksToDefinedRepositories(int projectId, [FromUri] string[] repositoryNames)
        {
            var linksToGithubRepositories = _githubGateway.GetLinksToGithubRepositories(repositoryNames);
            //SaveLinksToDefinedGithubRepositories(projectId, linksToGithubRepositories);
            return Ok();
        } 

        private void SaveLinksToDefinedGithubRepositories(int projectId, IEnumerable<string> links)
        {
            var project = _projectProvider.GetProject(projectId);
            foreach (var link in links)
                project.LinksToGithubRepositories.Add(new Uri(link));
            _projectProvider.UpdateProject(project);
        }

        private readonly IGithubGateway _githubGateway;
        private readonly ApplicationLocationSettings _applicationLocationSettings;
        private readonly IProjectProvider _projectProvider;
    }
}
