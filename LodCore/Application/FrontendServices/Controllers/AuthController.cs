using System;
using System.Net;
using System.Web.Http;
using Gateway;
using UserManagement.Domain;
using Journalist;
using FrontendServices.Authorization;
using FrontendServices.Models;
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
            IAuthorizer authorizer,
            ApplicationLocationSettings applicationLocationSettings)
        {
            Require.NotNull(githubGateway, nameof(githubGateway));
            Require.NotNull(userManager, nameof(userManager));
            Require.NotNull(profileSettings, nameof(profileSettings));
            Require.NotNull(authorizer, nameof(authorizer));
            Require.NotNull(applicationLocationSettings, nameof(applicationLocationSettings));

            _githubGateway = githubGateway;
            _userManager = userManager;
            _profileSettings = profileSettings;
            _authorizer = authorizer;
            _applicationLocationSettings = applicationLocationSettings;
        }

        [HttpPost]
        [Route("signup/github")]
        public string GetRedirectionToAuthenticationGitHubFormToSignUp([FromBody] RegisterDeveloperWithGithubRequest request)
        {
            var createAccountRequest = new CreateAccountRequest(
                request.LastName,
                request.FirstName,
                new Profile
                {
                    InstituteName = request.InstituteName,
                    PhoneNumber = request.PhoneNumber,
                    Specialization = request.StudyingProfile,
                    StudentAccessionYear = request.AccessionYear,
                    IsGraduated = request.IsGraduated,
                    StudyingDirection = request.Department,
                    VkProfileUri = request.VkProfileUri == null ? null : new Uri(request.VkProfileUri)
                });
            var userId = _userManager.CreateUserTemplate(createAccountRequest);
            var githubLoginUrl = _githubGateway.GetLinkToGithubLoginPageToSignUp(userId);
            return githubLoginUrl;
        }

        [HttpGet]
        [Route("github/callback/signup/{userId}")]
        public IHttpActionResult SignUpWithGitHub(int userId, string code, string state)
        {
            var githubAccessToken = GetToken(code, state);
            var user = _userManager.GetUserByGithubAccessToken(githubAccessToken);
            if (user != null)
                throw new AccountAlreadyExistsException();
            user = _userManager.GetUser(userId);
            SaveUserGithubProfileAttributes(githubAccessToken, user.UserId);
            return Redirect(_applicationLocationSettings.FrontendAdress);
        }

        [HttpGet]
        [Route("login/github")]
        public string GetRedirectionToAuthenticationGitHubFormToSignIn()
        {
            var githubLoginUrl = _githubGateway.GetLinkToGithubLoginPage();
            return githubLoginUrl;
        }

        [HttpPost]
        [Route("login/github/{userId}")]
        public AuthorizationTokenInfo LoginWithGithub(int userId)
        {
            var user = _userManager.GetUser(userId);
            var link = user.Profile.LinkToGithubProfile.ToString();
            var token = _authorizer.AuthorizeWithGithub(link);
            return token;
        }

        [HttpGet]
        [Route("github/callback/login")]
        public IHttpActionResult GetRedirectionToAuthorizationWithGithub(string code, string state)
        {
            var githubAccessToken = GetToken(code, state);
            var link = _githubGateway.GetLinkToUserGithubProfile(githubAccessToken);
            var user = _userManager.GetUserList(u => u.Profile.LinkToGithubProfile == new Uri(link))[0];
            if (user == null)
                throw new AccountNotFoundException();
            return Redirect(new Uri($"{_applicationLocationSettings.FrontendAdress}/login/github/{user.UserId}"));
        }

        [HttpGet]
        [Authorization(AccountRole.User)]
        [Route("auth/github")]
        public string GetRedirectionToAuthenticationGitHubFormToBindAccount()
        {
            var currentUserId = User.Identity.GetId();
            var githubLoginUrl = _githubGateway.GetLinkToGithubLoginPage(currentUserId);
            return githubLoginUrl;
        }

        [HttpGet]
        [Route("github/callback/auth/{userId}")]
        public IHttpActionResult GetAuthenticationTokenByCode(int userId, string code, string state)
        {
            var token = GetToken(code, state); 
            SaveUserGithubProfileAttributes(token, userId);
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

        private void SaveUserGithubProfileAttributes(string githubAccesstoken, int userId)
        {
            var user = _userManager.GetUser(userId);
            var link = new Uri(_githubGateway.GetLinkToUserGithubProfile(githubAccesstoken));
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
