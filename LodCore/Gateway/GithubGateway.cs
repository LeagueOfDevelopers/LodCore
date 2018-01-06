using System.Web.SessionState;
using System.Web.Security;
using System.Threading.Tasks;
using System;
using Journalist;
using Octokit;

namespace Gateway
{
    public class GithubGateway : IGithubGateway
    {
        public GithubGateway(GithubGatewaySettings githubGatewaySettings)
        {
            Require.NotNull(githubGatewaySettings, nameof(githubGatewaySettings));

            _githubGatewaySettings = githubGatewaySettings;
            _gitHubClient = new GitHubClient(new ProductHeaderValue(applicationName));
        }

        public string GetLinkToGithubLoginPage()
        {
            var request = new OauthLoginRequest(_githubGatewaySettings.ClientId)
            {
                Scopes = { "read:user" },
                State = SetCsrfState()
            };
            var githubLoginUrl = _gitHubClient.Oauth.GetGitHubLoginUrl(request);
            return githubLoginUrl.ToString();
        }

        public async Task<string> GetTokenByCode(string code)
        {
            var token = await _gitHubClient.Oauth.CreateAccessToken(new OauthTokenRequest(
                _githubGatewaySettings.ClientId,
                _githubGatewaySettings.ClientSecret,
                code));
            return token.AccessToken;
        }

        public bool StateIsValid(string state)
        {
            return !String.IsNullOrEmpty(state) && state == _csrf;
        }

        private string SetCsrfState()
        {
            //_session["CSRF:state"] = Membership.GeneratePassword(24, 1);
            //return _session["CSRF:state"].ToString();
            _csrf = Membership.GeneratePassword(24, 1);
            return _csrf;
        }

        const string applicationName = "LodCore";
        private readonly GitHubClient _gitHubClient;
        //private HttpSessionState _session { get; set; }
        private string _csrf;
        private readonly GithubGatewaySettings _githubGatewaySettings;
    }
}
