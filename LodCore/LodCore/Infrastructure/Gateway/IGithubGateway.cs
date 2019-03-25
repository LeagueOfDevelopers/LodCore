using System;
using System.Collections.Generic;
using Octokit;
using Account = LodCore.Domain.UserManagement.Account;
using Project = LodCore.Domain.ProjectManagment.Project;

namespace LodCore.Infrastructure.Gateway
{
    public interface IGithubGateway
    {
        string GetLinkToGithubLoginPageToSignUp(string frontendCallback, int registeredUserId);

        string GetLinkToGithubLoginPageToBind(string frontendCallback, int currentUserId);

        string GetLinkToGithubLoginPageToUnlink(string frontendCallback);

        string GetLinkToGithubLoginPage(string frontendCallback);

        string GetLinkToGithubLoginPage(string frontendCallback, int projectId, string developerIds);

        string GetLinkToGithubLoginPageToRemoveCollaborator(string frontendCallback, int projectId, int developerId);

        string GetLinktoGithubLoginPageToCreateRepository(string frontendCallback, string newRepositoryName);

        string GetToken(string code, string state);

        User GetUserGithubProfileInformation(string token);

        EmailAddress GetUserGithubProfileEmailAddress(string token);

        void RevokeAccess(string token);

        IEnumerable<GithubRepository> GetLeagueOfDevelopersRepositories();

        void AddCollaboratorToRepository(string githubAccessToken, Array developerIds, Project project);

        void RemoveCollaboratorFromRepository(string token, Account user,
            Project project);

        string CreateRepository(string token, string newRepositoryName);
    }
}