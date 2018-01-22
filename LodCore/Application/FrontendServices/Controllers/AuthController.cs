using System;
using System.Net;
using System.Web.Http;
using Gateway;
using UserManagement.Domain;
using Journalist;
using FrontendServices.Authorization;
using UserManagement.Application;
using Serilog;
using Common;

namespace FrontendServices.Controllers
{
    public class AuthController : ApiController
    {
        public AuthController(
            IGithubGateway githubGateway, 
            IUserManager userManager,
            ProfileSettings profileSettings,
            IAuthorizer authorizer)
        {
            Require.NotNull(githubGateway, nameof(githubGateway));
            Require.NotNull(userManager, nameof(userManager));
            Require.NotNull(profileSettings, nameof(profileSettings));
            Require.NotNull(authorizer, nameof(authorizer));

            _githubGateway = githubGateway;
            _userManager = userManager;
            _profileSettings = profileSettings;
            _authorizer = authorizer;
        }

        [HttpGet]
        [Route("login/github")]
        public string GetRedirectionToAuthenticationGitHubFormToAuthorize()
        {
            var githubLoginUrl = _githubGateway.GetLinkToGithubLoginPage();
            return githubLoginUrl;
        }

        [HttpGet]
        [Route("login/github/callback")]
        public IHttpActionResult AuthorizeWithGithub(string code, string state)
        {
            var githubAccessToken = GetToken(code, state);
            var user = _userManager.GetUserByGithubAccessToken(githubAccessToken);
            if (user == null)
            {
                var userId = _userManager.CreateUserTemplate();
                var link = new Uri(_githubGateway.GetLinkToUserGithubProfile(githubAccessToken));
                SaveTokenAndLinkToGithubProfile(link, githubAccessToken, userId);
                return Redirect(_applicationLocationSettings.FrontendAdress);
            }
            var token = _authorizer.AuthorizeByGithubAccessToken(githubAccessToken);
            return RedirectToRoute(_profileSettings.FrontendProfileUri, token);
        }

        [HttpGet]
        [Authorization(AccountRole.User)]
        [Route("auth/github")]
        public string GetRedirectionToAuthenticationGitHubForm()
        {
            var currentUserId = User.Identity.GetId();
            var githubLoginUrl = _githubGateway.GetLinkToGithubLoginPage(currentUserId);
            return githubLoginUrl;
        }

        [HttpGet]
        [Route("auth/github/callback/{userId}")]
        public IHttpActionResult GetAuthenticationTokenByCode(int userId, string code, string state)
        {
            var token = GetToken(code, state);
            var link = new Uri(_githubGateway.GetLinkToUserGithubProfile(token));
            SaveTokenAndLinkToGithubProfile(link, token, userId);
            return Redirect(_profileSettings.FrontendProfileUri);
        }

        private string GetToken(string code, string state)
        {
            try
            {
                if (!_githubGateway.StateIsValid(state))
                    throw new InvalidOperationException(
                        "Security fail: the received state value doesn't correspond to the expected");
                if (String.IsNullOrEmpty(code))
                    throw new InvalidOperationException("Value of code can't be blank or null");
            }
            catch (InvalidOperationException ex)
            {
                Log.Warning(ex, ex.Message);
                throw new HttpResponseException(HttpStatusCode.BadGateway);
            }
            var token = _githubGateway.GetTokenByCode(code);
            return token;
        }

        private void SaveTokenAndLinkToGithubProfile(Uri link, string githubAccesstoken, int userId)
        {
            var user = _userManager.GetUser(userId);
            user.Profile.LinkToGithubProfile = link;
            user.GithubAccessToken = githubAccesstoken;
            _userManager.UpdateUser(user);
        }

        private readonly IGithubGateway _githubGateway;
        private readonly IUserManager _userManager;
        private readonly ProfileSettings _profileSettings;
        private readonly IAuthorizer _authorizer;
        private readonly ApplicationLocationSettings _applicationLocationSettings;
    }
}
