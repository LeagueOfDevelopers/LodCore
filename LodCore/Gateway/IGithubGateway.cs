using Octokit;
using System.Collections.Generic;

namespace Gateway
{
    public interface IGithubGateway
    {
        string GetLinkToGithubLoginPageToSignUp(int registeredUserId);

        string GetLinkToGithubLoginPage(int currentUserId);

        string GetLinkToGithubLoginPageToUnlink();

        string GetLinkToGithubLoginPage();

        string GetLinkToGithubLoginPage(int projectId, int developerId);

        string GetLinkToGithubLoginPageToRemoveCollaborator(int projectId, int developerId);

        string GetLinktoGithubLoginPageToCreateRepository(string newRepositoryName);

        string GetToken(string code, string state);

        User GetUserGithubProfileInformation(string token);

        EmailAddress GetUserGithubProfileEmailAddress(string token);

        void RevokeAccess(string token);

        IEnumerable<GithubRepository> GetLeagueOfDevelopersRepositories();

        void AddCollaboratorToRepository(string githubAccessToken, UserManagement.Domain.Account user, 
                                                                   ProjectManagement.Domain.Project project);

        void RemoveCollaboratorFromRepository(string token, UserManagement.Domain.Account user, 
                                                            ProjectManagement.Domain.Project project);

        void CreateRepository(string token, string newRepositoryName);
    }
}
