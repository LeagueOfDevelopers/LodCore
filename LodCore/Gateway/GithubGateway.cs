using System;
using Journalist;
using Octokit;
using Common;

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

        // for registration with github request
        public string GetLinkToGithubLoginPageToSignUp(int id)
        {
            var redirectUri = new Uri($"{_githubGatewaySettings.GithubApiDefaultCallbackUri}signup/{id}");
            return CreateRequest(redirectUri);
        }

        // for binding github account
        public string GetLinkToGithubLoginPage(int id)
        {
            var redirectUri = new Uri($"{_githubGatewaySettings.GithubApiDefaultCallbackUri}auth/{id}");
            return CreateRequest(redirectUri);
        }

        // for login request
        public string GetLinkToGithubLoginPage()
        {
            var redirectUri = new Uri($"{_githubGatewaySettings.GithubApiDefaultCallbackUri}login");
            return CreateRequest(redirectUri);
        }

        public string GetTokenByCode(string code)
        {
            var token = _gitHubClient.Oauth.CreateAccessToken(new OauthTokenRequest(
                _githubGatewaySettings.ClientId,
                _githubGatewaySettings.ClientSecret,
                code)).Result;
            return token.AccessToken;
        }

        public User GetUserGithubProfileInformation(string token)
        {
            Require.NotEmpty(token, nameof(token));

            _gitHubClient.Credentials = new Credentials(token);
            var userInfo = _gitHubClient.User.Current().Result;
            return userInfo;
        }

        public EmailAddress GetUserGithubProfileEmailAddress(string token)
        {
            Require.NotEmpty(token, nameof(token));

            _gitHubClient.Credentials = new Credentials(token);
            var userEmail = _gitHubClient.User.Email.GetAll().Result[0];
            return userEmail;
        }

        public bool StateIsValid(string state)
        {
            Require.NotEmpty(state, nameof(state));
            Require.NotNull(state, nameof(state));

            return state == _csrf;
        }

        private string SetCsrfToken()
        {
            _csrf = TokenGenerator.GenerateToken();
            return _csrf;
        }

        private string CreateRequest(Uri redirectUri)
        {
            var request = new OauthLoginRequest(_githubGatewaySettings.ClientId)
            {
                Scopes = { "user" },
                State = SetCsrfToken(),
                RedirectUri = redirectUri
            };
            var githubLoginUrl = _gitHubClient.Oauth.GetGitHubLoginUrl(request);
            return githubLoginUrl.ToString();
        }

        const string applicationName = "LodSite";
        private readonly GitHubClient _gitHubClient;
        private string _csrf;
        private readonly GithubGatewaySettings _githubGatewaySettings;
    }
}
