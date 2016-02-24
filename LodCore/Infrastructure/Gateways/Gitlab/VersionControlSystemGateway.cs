using BinaryAnalysis.UnidecodeSharp;
using Journalist;
using NGitLab;
using NGitLab.Models;
using ProjectManagement.Application;
using ProjectManagement.Domain;
using ProjectManagement.Infrastructure;
using UserManagement.Application;
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

        public int CreateRepositoryForProject(CreateProjectRequest request)
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
            return createdProject.Id;
        }

        public void AddUserToRepository(Project project, int gitlabUserId)
        {
            Require.NotNull(project, nameof(project));
            Require.Positive(gitlabUserId, nameof(gitlabUserId));
            _gitLabClient.ProjectMembers.AddProjectMember(new ProjectMembershipCreateRequest
            {
                UserId = gitlabUserId,
                ProjectId = project.VersionControlSystemId,
                AccessLevel = DeveloperAccessLevel
            });
        }

        public void RemoveUserFromProject(Project project, int gitlabUserId)
        {
            Require.NotNull(project, nameof(project));
            Require.Positive(gitlabUserId, nameof(gitlabUserId));
            _gitLabClient.ProjectMembers.DeleteProjectMember(new RemoveProjectMembershipRequest
            {
                ProjectId = project.VersionControlSystemId,
                UserId = gitlabUserId
            });
        }

        public int RegisterUser(CreateAccountRequest request)
        {
            var user = new UserUpsert
            {
                Email = request.Email,
                IsAdmin = false,
                Password = request.Password,
                ProjectsLimit = 0,
                Name = request.Firstname + " " + request.Lastname,
                CanCreateGroup = false,
                Confirm = "no",
                Username = request.Lastname.Unidecode()
            };
            var addedUser = _gitLabClient.Users.Create(user);
            return addedUser.Id;
        }

        private readonly GitlabSettings _settings;
        private readonly GitLabClient _gitLabClient;

        //todo: extract it to enumeration
        private const int DeveloperAccessLevel = 30;
    }
}