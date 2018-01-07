using System;
using System.Net;
using System.Web.Http;
using Gateway;
using UserManagement.Domain;
using Journalist;
using FrontendServices.Authorization;
using UserManagement.Application;

namespace FrontendServices.Controllers
{
    public class AuthController : ApiController
    {
        public AuthController(IGithubGateway githubGateway, IUserManager userManager)
        {
            Require.NotNull(githubGateway, nameof(githubGateway));
            Require.NotNull(userManager, nameof(userManager));

            _githubGateway = githubGateway;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorization(AccountRole.User)]
        [Route("auth/github")]
        public IHttpActionResult GetRedirectionToAuthenticationGitHubForm()
        {
            var githubLoginUrl = _githubGateway.GetLinkToGithubLoginPage();
            return Redirect(githubLoginUrl);
        }

        [HttpGet]
        [Authorization(AccountRole.User)]
        [Route("auth/github/callback")]
        public IHttpActionResult GetAuthenticationTokenByCode(string code, string state)
        {
            if (!_githubGateway.StateIsValid(state))
                throw new InvalidOperationException("Security fail: the received state value doesn't correspond to the expected");
            if (String.IsNullOrEmpty(code))
                throw new HttpResponseException(HttpStatusCode.BadGateway);
            var token = _githubGateway.GetTokenByCode(code).Result;
            var link = _githubGateway.GetLinkToUserGithubProfile(token).Result;
            SaveLinkToGithubProfile(link);
            return Ok(link);
        }

        private void SaveLinkToGithubProfile(string link)
        {
            var user = _userManager.GetUser(User.Identity.GetId());
            user.LinkToGithubProfile = link;
            _userManager.UpdateUser(user);
        }

        private readonly IGithubGateway _githubGateway;
        private readonly IUserManager _userManager;
    }
}
