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
using Serilog;
using System.Web;
using System.Net;

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
            var queryString = Request.RequestUri.Query;
            string frontCallback;
            try
            {
                frontCallback = QueryStringParser.ParseQueryStringToGetParameter(queryString, frontendCallbackComponentName);
            }
            catch (HttpParseException ex)
            {
                Log.Error("There is no parameter with key={0} in query string={1}. {2} StackTrace: {3}",
                    frontendCallbackComponentName, queryString, ex.Message, ex.StackTrace);
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return _githubGateway.GetLinkToGithubLoginPage(frontCallback, projectId, developerId);
        }

        [HttpGet]
        [Authorization(AccountRole.Administrator)]
        [Route("github/repositories/{projectId}/developer/{developerId}/delete")]
        public string GetLinkToGithubLoginPageToCheckAuthenticationForDeletion(int projectId, int developerId)
        {
            var queryString = Request.RequestUri.Query;
            string frontCallback;
            try
            {
                frontCallback = QueryStringParser.ParseQueryStringToGetParameter(queryString, frontendCallbackComponentName);
            }
            catch (HttpParseException ex)
            {
                Log.Error("There is no parameter with key={0} in query string={1}. {2} StackTrace: {3}",
                    frontendCallbackComponentName, queryString, ex.Message, ex.StackTrace);
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return _githubGateway.GetLinkToGithubLoginPageToRemoveCollaborator(frontCallback, projectId, developerId);
        }

        [HttpGet]
        [Authorization(AccountRole.Administrator)]
        [Route("github/repositories/{newRepositoryName}")]
        public string GetLinkToGithubLoginPageToCreateRepository(string newRepositoryName, string frontend_callback)
        {
            var queryString = Request.RequestUri.Query;
            string frontCallback;
            try
            {
                frontCallback = QueryStringParser.ParseQueryStringToGetParameter(queryString, frontendCallbackComponentName);
            }
            catch (HttpParseException ex)
            {
                Log.Error("There is no parameter with key={0} in query string={1}. {2} StackTrace: {3}",
                    frontendCallbackComponentName, queryString, ex.Message, ex.StackTrace);
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return _githubGateway.GetLinktoGithubLoginPageToCreateRepository(frontCallback, newRepositoryName);
        }
        
        [HttpGet]
        [Route("github/callback/repositories/{projectId}/developer/{developerId}")]
        public IHttpActionResult AddCollaboratorToProjectRepositories(int projectId, int developerId, string frontend_callback, string code, string state)
        {
            var project = _projectProvider.GetProject(projectId);
            var developer = _userManager.GetUser(developerId);
            var success = true;
            try
            {
                var githubAccessToken = _githubGateway.GetToken(code, state);
                _githubGateway.AddCollaboratorToRepository(githubAccessToken, developer, project);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to add collaborator with id={0} to project with id={1}: {2}. StackTrace: {3}",
                    developerId.ToString(), projectId.ToString(), ex.Message, ex.StackTrace);
                success = false;
            }
            return Redirect(ResponseSuccessMarker.MarkRedirectUrlSuccessAs(frontend_callback, success));
        }

        [HttpGet]
        [Route("github/callback/repositories/{projectId}/developer/{developerId}/delete")]
        public IHttpActionResult RemoveCollaboratorFromProjectRepositories(int projectId, int developerId, string frontend_callback, string code, string state)
        {
            var project = _projectProvider.GetProject(projectId);
            var developer = _userManager.GetUser(developerId);
            var success = true;
            try
            {
                var githubAccessToken = _githubGateway.GetToken(code, state);
                _githubGateway.RemoveCollaboratorFromRepository(githubAccessToken, developer, project);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to remove collaborator with id={0} from project with id={1}: {2}. StackTrace: {3}", 
                    developerId.ToString(), projectId.ToString(), ex.Message, ex.StackTrace);
                success = false;
            }
            return Redirect(ResponseSuccessMarker.MarkRedirectUrlSuccessAs(frontend_callback, success));
        }

        [HttpGet]
        [Route("github/callback/repositories/{newRepositoryName}")]
        public IHttpActionResult CreateRepository(string newRepositoryName, string frontend_callback, string code, string state)
        {
            var success = true;
            var newRepositoryUrl = "";
            try
            {
                var githubAccessToken = _githubGateway.GetToken(code, state);
                newRepositoryUrl = _githubGateway.CreateRepository(githubAccessToken, newRepositoryName);
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Failed to remove create repository with name={0}: {1}. StackTrace: {2}",
                    newRepositoryName, ex.Message, ex.StackTrace);
                success = false;
            }
            return Redirect(ResponseSuccessMarker.MarkRedirectUrlSuccessAs($"{frontend_callback}&repository_link={newRepositoryUrl}", success));
        }

        private const string frontendCallbackComponentName = "frontend_callback";
        private readonly IGithubGateway _githubGateway;
        private readonly IProjectProvider _projectProvider;
        private readonly IUserManager _userManager;
        private readonly ApplicationLocationSettings _applicationLocationSettings;
    }
}
