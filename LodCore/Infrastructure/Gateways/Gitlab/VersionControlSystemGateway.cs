using System;
using System.Diagnostics;
using BinaryAnalysis.UnidecodeSharp;
using Journalist;
using NGitLab;
using NGitLab.Models;
using ProjectManagement.Application;
using ProjectManagement.Domain;
using ProjectManagement.Infrastructure;
using UserManagement.Application;
using UserManagement.Domain;
using UserManagement.Infrastructure;
using Project = ProjectManagement.Domain.Project;

namespace Gateways.Gitlab
{
    public class VersionControlSystemGateway : IVersionControlSystemGateway, IGitlabUserRegistrar
    {
        public VersionControlSystemGateway(GitlabSettings settings)
        {
            Require.NotNull(settings, nameof(settings));
            _settings = settings;
            _gitLabClient = GitLabClient.Connect(_settings.Host, _settings.ApiKey);
        }

        public VersionControlSystemInfo CreateRepositoryForProject(CreateProjectRequest request)
        {
            var project = new ProjectCreate
            {
                IssuesEnabled = false,
                Description = request.Info,
                MergeRequestsEnabled = false,
                Name = request.Name,
                VisibilityLevel = request.AccessLevel == AccessLevel.Public
                    ? VisibilityLevel.Public
                    : VisibilityLevel.Private
            };

            var createdProject = _gitLabClient.Projects.Create(project);
            return new VersionControlSystemInfo(createdProject.Id, new Uri(createdProject.WebUrl));
        }

        public VersionControlSystemInfo UpdateRepositoryForProject(Project projectToUpdate)
        {
            var project = new ProjectUpdate
            {
                Id = projectToUpdate.VersionControlSystemInfo.ProjectId,
                Name = projectToUpdate.Name,
                Description = projectToUpdate.Info,
                Public = projectToUpdate.AccessLevel == AccessLevel.Public,
                VisibilityLevel = projectToUpdate.AccessLevel == AccessLevel.Public ? VisibilityLevel.Public : VisibilityLevel.Private
            };

            var updatedProject = _gitLabClient.Projects.Update(project);
            return new VersionControlSystemInfo(updatedProject.Id, new Uri(updatedProject.WebUrl));
        }

        public void AddUserToRepository(Project project, int gitlabUserId)
        {
            Require.NotNull(project, nameof(project));
            Require.Positive(gitlabUserId, nameof(gitlabUserId));
            _gitLabClient.ProjectMembers.AddProjectMember(new ProjectMembershipCreateRequest
            {
                UserId = gitlabUserId,
                ProjectId = project.VersionControlSystemInfo.ProjectId,
                AccessLevel = DeveloperAccessLevel
            });
        }

        public void RemoveUserFromProject(Project project, int gitlabUserId)
        {
            Require.NotNull(project, nameof(project));
            Require.Positive(gitlabUserId, nameof(gitlabUserId));
            _gitLabClient.ProjectMembers.DeleteProjectMember(new RemoveProjectMembershipRequest
            {
                ProjectId = project.VersionControlSystemInfo.ProjectId,
                UserId = gitlabUserId
            });
        }

        public int RegisterUser(Account account)
        {
            var user = new UserUpsert
            {
                Email = account.Email.Address,
                IsAdmin = false,
                Password = account.Password.Value, //todo
                ProjectsLimit = 0,
                Name = account.Firstname + " " + account.Lastname,
                CanCreateGroup = false,
                Confirm = "no",
                Username = GetUserNameByLastName(account.Lastname)
            };

            var addedUser = _gitLabClient.Users.Create(user);
            return addedUser.Id;
        }

        private string GetUserNameByLastName(string lastName)
        {
            return lastName.Unidecode().Replace(@"'", string.Empty);
        }
        private readonly GitlabSettings _settings;
        private readonly GitLabClient _gitLabClient;

        //todo: extract it to enumeration
        private const int DeveloperAccessLevel = 30;
    }
}