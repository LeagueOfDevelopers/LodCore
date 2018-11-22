using System;
using System.Web.Http;
using Journalist;
using LodCoreApiOld.Models;
using System.Net.Mail;
using Serilog;
using System.Web;
using System.Net;
using LodCoreLibraryOld.Infrastructure.Gateway;
using LodCoreLibraryOld.Facades;
using LodCoreLibraryOld.Domain.UserManagement;
using LodCoreLibraryOld.Common;
using LodCoreLibraryOld.Infrastructure.EventBus;
using LodCoreLibraryOld.Domain.Exceptions;
using LodCoreLibraryOld.Domain.NotificationService;

namespace LodCoreApiOld.Controllers
{
    public class AuthController : ApiController
    {
        public AuthController(
            IGithubGateway githubGateway, 
            IUserManager userManager,
            ProfileSettings profileSettings,
            IAuthorizer authorizer,
            ApplicationLocationSettings applicationLocationSettings,
            IEventPublisher eventPublisher,
            IConfirmationService confirmationService)
        {
            Require.NotNull(githubGateway, nameof(githubGateway));
            Require.NotNull(userManager, nameof(userManager));
            Require.NotNull(profileSettings, nameof(profileSettings));
            Require.NotNull(authorizer, nameof(authorizer));
            Require.NotNull(applicationLocationSettings, nameof(applicationLocationSettings));
            Require.NotNull(eventPublisher, nameof(eventPublisher));
            Require.NotNull(confirmationService, nameof(confirmationService));

            _githubGateway = githubGateway;
            _userManager = userManager;
            _profileSettings = profileSettings;
            _authorizer = authorizer;
            _applicationLocationSettings = applicationLocationSettings;
            _eventPublisher = eventPublisher;
            _confirmationService = confirmationService;
        }

        [HttpPost]
        [Route("signup/github")]
        public string GetRedirectionToAuthenticationGitHubFormToSignUp([FromBody] RegisterDeveloperWithGithubRequest request)
        {
            var queryString = Request.RequestUri.Query;
            string frontCallback;
            try
            {
                frontCallback = QueryStringParser.ParseQueryStringToGetParameter(queryString, frontendCallbackComponentName);
            }
            catch(HttpParseException ex)
            {
                Log.Error("There is no parameter with key={0} in query string={1}. {2} StackTrace: {3}", 
                    frontendCallbackComponentName, queryString, ex.Message, ex.StackTrace);
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
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
            var githubLoginUrl = _githubGateway.GetLinkToGithubLoginPageToSignUp(frontCallback, userId);
            return githubLoginUrl;
        }

        [HttpGet]
        [Route("github/callback/signup/{userId}")]
        public IHttpActionResult SignUpWithGitHub(int userId, string frontend_callback, string code, string state)
        {
            Account user;
            try
            {
                var githubAccessToken = _githubGateway.GetToken(code, state);
                var userInfo = _githubGateway.GetUserGithubProfileInformation(githubAccessToken);
                user = _userManager.GetUserByLinkToGithubProfile(userInfo.HtmlUrl);
                if (user != null)
                    throw new AccountAlreadyExistsException();
                user = _userManager.GetUser(userId);
                user.Profile.LinkToGithubProfile = new Uri(userInfo.HtmlUrl);
                user.Email = new MailAddress(_githubGateway.GetUserGithubProfileEmailAddress(githubAccessToken).Email);
                _userManager.UpdateUser(user);
            }
            catch (Exception ex)
            {
                Log.Error("Failed to register user with template id={0} via github. {1} StackTrace: {2}",
                    userId.ToString(), ex.Message, ex.StackTrace);
                return Redirect(ResponseSuccessMarker.MarkRedirectUrlSuccessAs(frontend_callback, false));
            }
            var @event = new NewEmailConfirmedDeveloper(user.UserId, user.Firstname, user.Lastname);
            _eventPublisher.PublishEvent(@event);
            return Redirect(ResponseSuccessMarker.MarkRedirectUrlSuccessAs(frontend_callback, true));
        }

        [HttpGet]
        [Route("login/github")]
        public string GetRedirectionToAuthenticationGitHubFormToSignIn()
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
            var githubLoginUrl = _githubGateway.GetLinkToGithubLoginPage(frontCallback);
            return githubLoginUrl;
        }

        [HttpGet]
        [Route("github/callback/login")]
        public IHttpActionResult GetRedirectionToAuthorizationWithGithub(string frontend_callback, string code, string state)
        {
            string encodedToken = "";
            string link = "";
            var success = true;
            try
            {
                var githubAccessToken = _githubGateway.GetToken(code, state);
                link = _githubGateway.GetUserGithubProfileInformation(githubAccessToken).HtmlUrl;
                var user = _userManager.GetUserByLinkToGithubProfile(link);
                AuthorizationTokenInfo token;
                if (user == null)
                    throw new AccountNotFoundException();
                token = _authorizer.AuthorizeWithGithub(link);
                encodedToken = Encoder.Encode(token);
            }
            catch (AccountNotFoundException ex)
            {
                Log.Error("Failed to get user with linkToGithubProfile={0}. {1} StackTrace: {2}", link, ex.Message, ex.StackTrace);
                success = false;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "StackTrace:" + ex.StackTrace);
                success = false;
            }
            return Redirect(ResponseSuccessMarker.MarkRedirectUrlSuccessAs($"{frontend_callback}/{encodedToken}", success));
        }

        [HttpGet]
        [Route("auth/github/{id}")]
        public string GetRedirectionToAuthenticationGitHubFormToBindAccount(int id)
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
            var githubLoginUrl = _githubGateway.GetLinkToGithubLoginPageToBind(frontCallback, id);
            return githubLoginUrl;
        }

        [HttpGet]
        [Route("github/callback/auth/{id}")]
        public IHttpActionResult GetAuthenticationTokenByCode(int id, string frontend_callback, string code, string state)
        {
            try
            {
                var token = _githubGateway.GetToken(code, state);
                var userInfo = _githubGateway.GetUserGithubProfileInformation(token);
                var user = _userManager.GetUser(id);
                user.Profile.LinkToGithubProfile = new Uri(userInfo.HtmlUrl);
                _userManager.UpdateUser(user);
                _confirmationService.SetupEmailConfirmation(id);
                return Redirect(ResponseSuccessMarker.MarkRedirectUrlSuccessAs(frontend_callback, true));
            }
            catch (Exception ex)
            {
                Log.Error("Failed to bind GitHub account to user with id={0}. Registration failed. {1} StackTrace: {2}",
                    id.ToString(), ex.Message, ex.StackTrace);
                return Redirect(ResponseSuccessMarker.MarkRedirectUrlSuccessAs(frontend_callback, false));
            }
        }

        private const string frontendCallbackComponentName = "frontend_callback";
        private readonly IGithubGateway _githubGateway;
        private readonly IUserManager _userManager;
        private readonly ProfileSettings _profileSettings;
        private readonly IAuthorizer _authorizer;
        private readonly ApplicationLocationSettings _applicationLocationSettings;
        private readonly IEventPublisher _eventPublisher;
        private readonly IConfirmationService _confirmationService;
    }
}
