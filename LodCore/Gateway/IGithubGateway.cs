using Octokit;
using System.Collections.Generic;

namespace Gateway
{
    public interface IGithubGateway
    {
        string GetLinkToGithubLoginPageToSignUp(int registeredUserId);

        string GetLinkToGithubLoginPage(int currentUserId);

        string GetLinkToGithubLoginPage();

        string GetTokenByCode(string code);

        User GetUserGithubProfileInformation(string token);

        EmailAddress GetUserGithubProfileEmailAddress(string token);

        void RevokeAccess(string token);

        IEnumerable<string> GetLeagueOfDevelopersRepositories();

        IEnumerable<string> GetLinksToGithubRepositories(string[] repositoryName);

        bool StateIsValid(string state);
    }
}
