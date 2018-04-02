using System;
using Journalist;
using Octokit;
using Common;
using System.Collections.Generic;

namespace Gateway
{
    public class GithubGateway : IGithubGateway
    {
        public GithubGateway(GithubGatewaySettings githubGatewaySettings)
        {
            Require.NotNull(githubGatewaySettings, nameof(githubGatewaySettings));

            _githubGatewaySettings = githubGatewaySettings;
            _gitHubClient = new GitHubClient(new ProductHeaderValue(_githubGatewaySettings.OrganizationName));
        }

        // for registration with github request
        public string GetLinkToGithubLoginPageToSignUp(string frontendCallback, int id)
        {
            var redirectUri = $"{_githubGatewaySettings.GithubApiDefaultCallbackUri}signup/{id}";
            return CreateRequest(redirectUri, frontendCallback, "user");
        }

        // for binding github account
        public string GetLinkToGithubLoginPageToBind(string frontendCallback, int id)
        {
            var redirectUri = $"{_githubGatewaySettings.GithubApiDefaultCallbackUri}auth/{id}";
            return CreateRequest(redirectUri, frontendCallback, "user");
        }

        // for unbinding github account
        public string GetLinkToGithubLoginPageToUnlink(string frontendCallback)
        {
            var redirectUri = $"{_githubGatewaySettings.GithubApiDefaultCallbackUri}unlink";
            return CreateRequest(redirectUri, frontendCallback, "user");
        }

        // for login request
        public string GetLinkToGithubLoginPage(string frontendCallback)
        {
            var redirectUri = $"{_githubGatewaySettings.GithubApiDefaultCallbackUri}login";
            return CreateRequest(redirectUri, frontendCallback, "user");
        }

        // for collaborators management
        public string GetLinkToGithubLoginPage(string frontendCallback, int projectId, int developerId)
        {
            var redirectUri = $"{_githubGatewaySettings.GithubApiDefaultCallbackUri}repositories/{projectId}/developer/{developerId}";
            return CreateRequest(redirectUri, frontendCallback, "user", "admin:org", "repo");
        }

        public string GetLinkToGithubLoginPageToRemoveCollaborator(string frontendCallback, int projectId, int developerId)
        {
            var redirectUri = $"{_githubGatewaySettings.GithubApiDefaultCallbackUri}repositories/{projectId}/developer/{developerId}/delete";
            return CreateRequest(redirectUri, frontendCallback, "user", "admin:org", "repo");
        }

        // for repository creation
        public string GetLinktoGithubLoginPageToCreateRepository(string frontendCallback, string newRepositoryName)
        {
            var redirectUri = $"{_githubGatewaySettings.GithubApiDefaultCallbackUri}repositories/{newRepositoryName}";
            return CreateRequest(redirectUri, frontendCallback, "user", "admin:org", "repo");
        }

        public string GetToken(string code, string state)
        {
            if (!StateIsValid(state))
                throw new InvalidOperationException(
                    "Security fail: the received state value doesn't correspond to the expected");
            if (String.IsNullOrEmpty(code))
                throw new InvalidOperationException("Value of code can't be blank or null");
            var token = GetTokenByCode(code);
            return token;
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

        public void RevokeAccess(string token)
        {
            _gitHubClient.Credentials = new Credentials(token);
            _gitHubClient.Authorization.RevokeApplicationAuthentication(_githubGatewaySettings.ClientId, token);
        }

        public IEnumerable<GithubRepository> GetLeagueOfDevelopersRepositories()
        {
            var repositories = _gitHubClient.Repository.GetAllForOrg(_githubGatewaySettings.OrganizationName).Result;
            List<GithubRepository> githubRepositories = new List<GithubRepository>();
            foreach (var repo in repositories)
                githubRepositories.Add(new GithubRepository(repo.Name, repo.HtmlUrl));
            return githubRepositories;
        }

        public void AddCollaboratorToRepository(string token, UserManagement.Domain.Account user, ProjectManagement.Domain.Project project)
        {
            _gitHubClient.Credentials = new Credentials(token);
            if (user.Profile.LinkToGithubProfile == null)
                throw new NotFoundException("Link to GitHub profile is missed", System.Net.HttpStatusCode.NotFound);
            var repoLinks = project.LinksToGithubRepositories;
            if (repoLinks.Count != 0)
            {
                var userName = GetNameFromUri(user.Profile.LinkToGithubProfile);
                foreach (var link in repoLinks)
                {
                    var repoName = GetNameFromUri(link);
                    if (!_gitHubClient.Repository.Collaborator.IsCollaborator(_githubGatewaySettings.OrganizationName, repoName, userName).Result)
                        _gitHubClient.Repository.Collaborator.Add(_githubGatewaySettings.OrganizationName, repoName, userName);
                }
            }
        }

        public void RemoveCollaboratorFromRepository(string token, UserManagement.Domain.Account user, ProjectManagement.Domain.Project project)
        {
            _gitHubClient.Credentials = new Credentials(token);
            var repoLinks = project.LinksToGithubRepositories;
            var userName = GetNameFromUri(user.Profile.LinkToGithubProfile);
            foreach (var link in repoLinks)
            {
                var repoName = GetNameFromUri(link);
                if (_gitHubClient.Repository.Collaborator.IsCollaborator(_githubGatewaySettings.OrganizationName, repoName, userName).Result)
                   _gitHubClient.Repository.Collaborator.Delete(_githubGatewaySettings.OrganizationName, repoName, userName);
            }
        }

        public void CreateRepository(string token, string newRepositoryName)
        {
            _gitHubClient.Credentials = new Credentials(token);
            _gitHubClient.Repository.Create(_githubGatewaySettings.OrganizationName, new NewRepository(newRepositoryName));
        }

        private string GetTokenByCode(string code)
        {
            var token = _gitHubClient.Oauth.CreateAccessToken(new OauthTokenRequest(
                _githubGatewaySettings.ClientId,
                _githubGatewaySettings.ClientSecret,
                code)).Result;
            return token.AccessToken;
        }

        private bool StateIsValid(string state)
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

        private string CreateRequest(string redirectUri, string frontendCallback, params string[] scopes)
        {
            var request = new OauthLoginRequest(_githubGatewaySettings.ClientId)
            {
                Scopes = { String.Join(",", scopes) },
                State = SetCsrfToken(),
                RedirectUri = BindFrontendCallbackToUrl(redirectUri, frontendCallback)
            };
            var githubLoginUrl = _gitHubClient.Oauth.GetGitHubLoginUrl(request);
            return githubLoginUrl.ToString();
        }

        private Uri BindFrontendCallbackToUrl(string url, string frontendCallback)
        {
            if (url.Contains("?"))
                return new Uri($"{url}&frontend_callback={frontendCallback}");
            return new Uri($"{url}?frontend_callback={frontendCallback}");
        }

        private string GetNameFromUri(Uri uri)
        {
            return uri.AbsolutePath.Split('/')[uri.AbsolutePath.Split('/').Length - 1];
        }

        private readonly GitHubClient _gitHubClient;
        private string _csrf;
        private readonly GithubGatewaySettings _githubGatewaySettings;
    }
}
