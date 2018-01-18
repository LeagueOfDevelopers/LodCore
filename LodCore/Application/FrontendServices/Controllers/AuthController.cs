using System;
using System.Net;
using System.Web.Http;
using Gateway;
using UserManagement.Domain;
using Journalist;
using FrontendServices.Authorization;
using UserManagement.Application;
using Serilog;

namespace FrontendServices.Controllers
{
    public class AuthController : ApiController
    {
        public AuthController(
            IGithubGateway githubGateway, 
            IUserManager userManager,
            ProfileSettings profileSettings)
        {
            Require.NotNull(githubGateway, nameof(githubGateway));
            Require.NotNull(userManager, nameof(userManager));
            Require.NotNull(profileSettings, nameof(profileSettings));

            _githubGateway = githubGateway;
            _userManager = userManager;
            _profileSettings = profileSettings;
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
            try
            {
                if (!_githubGateway.StateIsValid(state))
                    throw new InvalidOperationException(
                        "Security fail: the received state value doesn't correspond to the expected");
                if (String.IsNullOrEmpty(code))
                    throw new InvalidOperationException("Value of code can't be blank or null");
            }
            catch(InvalidOperationException ex)
            {
                Log.Warning(ex, ex.Message);
                return StatusCode(HttpStatusCode.BadGateway);
            }
            var token = _githubGateway.GetTokenByCode(code);
            var link = new Uri(_githubGateway.GetLinkToUserGithubProfile(token));
            SaveLinkToGithubProfile(link, userId);
            return Redirect(_profileSettings.FrontendProfileUri);
        }

        private void SaveLinkToGithubProfile(Uri link, int userId)
        {
            var user = _userManager.GetUser(userId);
            user.Profile.LinkToGithubProfile = link;
            _userManager.UpdateUser(user);
        }

        private readonly IGithubGateway _githubGateway;
        private readonly IUserManager _userManager;
        private readonly ProfileSettings _profileSettings;
    }
}
