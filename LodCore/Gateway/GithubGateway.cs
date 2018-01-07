using System.Web.Security;
using System.Threading.Tasks;
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

        public async Task<string> GetLinkToUserGithubProfile(string token)
        {
            Require.NotEmpty(token, nameof(token));

            _gitHubClient.Credentials = new Credentials(token);
            var userInfo = await _gitHubClient.User.Current();
            return userInfo.HtmlUrl;
        }

        public bool StateIsValid(string state)
        {
            Require.NotEmpty(state, nameof(state));
            Require.NotNull(state, nameof(state));

            return state == _csrf;
        }

        private string SetCsrfState()
        {
            _csrf = Membership.GeneratePassword(24, 0);
            return _csrf;
        }

        const string applicationName = "LodCore";
        private readonly GitHubClient _gitHubClient;
        private string _csrf;
        private readonly GithubGatewaySettings _githubGatewaySettings;
    }
}
