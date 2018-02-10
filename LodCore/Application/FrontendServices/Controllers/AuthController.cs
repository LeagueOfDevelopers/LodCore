using System;
using System.Net;
using System.Web.Http;
using Gateway;
using UserManagement.Domain;
using Journalist;
using FrontendServices.Authorization;
using FrontendServices.Models;
using UserManagement.Application;
using Common;
using System.Net.Mail;

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
            try
            {
                var githubAccessToken = GetToken(code, state);
                var userInfo = _githubGateway.GetUserGithubProfileInformation(githubAccessToken);
                var user = _userManager.GetUserByLinkToGithubProfile(userInfo.HtmlUrl);
                if (user != null)
                    throw new AccountAlreadyExistsException();
                user = _userManager.GetUser(userId);
                user.GithubAccessToken = githubAccessToken;
                user.Profile.LinkToGithubProfile = new Uri(userInfo.HtmlUrl);
                user.Email = new MailAddress(_githubGateway.GetUserGithubProfileEmailAddress(githubAccessToken).Email);
                _userManager.UpdateUser(user);
            }
            catch (Exception)
            {
                return Redirect($"{_applicationLocationSettings.FrontendAdress}/error/registration");
            }
            return Redirect($"{_applicationLocationSettings.FrontendAdress}/success/github");
        }

        [HttpGet]
        [Route("login/github")]
        public string GetRedirectionToAuthenticationGitHubFormToSignIn()
        {
            var githubLoginUrl = _githubGateway.GetLinkToGithubLoginPage();
            return githubLoginUrl;
        }

        [HttpGet]
        [Route("github/callback/login")]
        public IHttpActionResult GetRedirectionToAuthorizationWithGithub(string code, string state)
        {
            string encodedToken;
            try
            {
                var githubAccessToken = GetToken(code, state);
                var link = _githubGateway.GetUserGithubProfileInformation(githubAccessToken).HtmlUrl;
                var user = _userManager.GetUserByLinkToGithubProfile(link);
                AuthorizationTokenInfo token;
                if (user == null)
                    throw new AccountNotFoundException();
                token = _authorizer.AuthorizeWithGithub(link);
                encodedToken = Encoder.Encode(token);
            }
            catch (Exception)
            {
                return Redirect($"{_applicationLocationSettings.FrontendAdress}/error/login");
            }
            return Redirect(new Uri($"{_applicationLocationSettings.FrontendAdress}/login/github/{encodedToken}"));
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
            var userInfo = _githubGateway.GetUserGithubProfileInformation(token);
            var user = _userManager.GetUser(userId);
            user.GithubAccessToken = token;
            user.Profile.LinkToGithubProfile = new Uri(userInfo.HtmlUrl);
            _userManager.UpdateUser(user);
            return Redirect(_profileSettings.FrontendProfileUri);
        }

        [HttpPost]
        [Authorization(AccountRole.User)]
        [Route("unlink/github")]
        public IHttpActionResult UnlinkGithubProfile()
        {
            var currentUserId = User.Identity.GetId();
            var user = _userManager.GetUser(currentUserId);
            if (user.Password != null)
            {
                _githubGateway.RevokeAccess(user.GithubAccessToken);
                user.GithubAccessToken = null;
                user.Profile.LinkToGithubProfile = null;
                _userManager.UpdateUser(user);
                return Ok();
            }
            return Conflict();
        }

        private string GetToken(string code, string state)
        {
            if (!_githubGateway.StateIsValid(state))
                throw new InvalidOperationException(
                    "Security fail: the received state value doesn't correspond to the expected");
            if (String.IsNullOrEmpty(code))
                throw new InvalidOperationException("Value of code can't be blank or null");
            var token = _githubGateway.GetTokenByCode(code);
            return token;
        }

        private readonly IGithubGateway _githubGateway;
        private readonly IUserManager _userManager;
        private readonly ProfileSettings _profileSettings;
        private readonly IAuthorizer _authorizer;
        private readonly ApplicationLocationSettings _applicationLocationSettings;
    }
}
