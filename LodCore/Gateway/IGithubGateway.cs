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

        IEnumerable<GithubRepository> GetLeagueOfDevelopersRepositories();

        void AddCollaboratorToRepository(UserManagement.Domain.Account user, ProjectManagement.Domain.Project project);

        void RemoveCollaboratorFromRepository(UserManagement.Domain.Account user, ProjectManagement.Domain.Project project);

        bool StateIsValid(string state);
    }
}
