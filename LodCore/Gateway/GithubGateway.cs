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

        public void RevokeAccess(string token)
        {
            _gitHubClient.Credentials = new Credentials(token);
            _gitHubClient.Authorization.ResetApplicationAuthentication(_githubGatewaySettings.ClientId, token);
        }

        public IEnumerable<GithubRepository> GetLeagueOfDevelopersRepositories()
        {
            var repositories = _gitHubClient.Repository.GetAllForOrg(_githubGatewaySettings.OrganizationName).Result;
            List<GithubRepository> githubRepositories = new List<GithubRepository>();
            foreach (var repo in repositories)
                githubRepositories.Add(new GithubRepository(repo.Name, repo.HtmlUrl));
            return githubRepositories;
        }

        public void AddCollaboratorToRepository(UserManagement.Domain.Account user, ProjectManagement.Domain.Project project)
        {
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

        public void RemoveCollaboratorFromRepository(UserManagement.Domain.Account user, ProjectManagement.Domain.Project project)
        {
            var repoLinks = project.LinksToGithubRepositories;
            var userName = GetNameFromUri(user.Profile.LinkToGithubProfile);
            foreach (var link in repoLinks)
            {
                var repoName = GetNameFromUri(link);
                if (_gitHubClient.Repository.Collaborator.IsCollaborator(_githubGatewaySettings.OrganizationName, repoName, userName).Result)
                    _gitHubClient.Repository.Collaborator.Delete(_githubGatewaySettings.OrganizationName, repoName, userName);
            }
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

        private string GetNameFromUri(Uri uri)
        {
            return uri.AbsolutePath.Split('/')[uri.AbsolutePath.Split('/').Length - 1];
        }

        private readonly GitHubClient _gitHubClient;
        private string _csrf;
        private readonly GithubGatewaySettings _githubGatewaySettings;
    }
}
