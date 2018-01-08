using System.Web.Security;
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

        public string GetLinkToGithubLoginPage(int id)
        {
            var request = new OauthLoginRequest(_githubGatewaySettings.ClientId)
            {
                Scopes = { "read:user" },
                State = SetCsrfState(),
                RedirectUri = new Uri($"{_githubGatewaySettings.GithubApiDefaultCallbackUri}/{id}")
            };
            var githubLoginUrl = _gitHubClient.Oauth.GetGitHubLoginUrl(request);
            return githubLoginUrl.ToString();
        }

        public string GetTokenByCode(string code)
        {
            var token = _gitHubClient.Oauth.CreateAccessToken(new OauthTokenRequest(
                _githubGatewaySettings.ClientId,
                _githubGatewaySettings.ClientSecret,
                code)).Result;
            return token.AccessToken;
        }

        public string GetLinkToUserGithubProfile(string token)
        {
            Require.NotEmpty(token, nameof(token));

            _gitHubClient.Credentials = new Credentials(token);
            var userInfo = _gitHubClient.User.Current().Result;
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

        const string applicationName = "LodSite";
        private readonly GitHubClient _gitHubClient;
        private string _csrf;
        private readonly GithubGatewaySettings _githubGatewaySettings;
    }
}
