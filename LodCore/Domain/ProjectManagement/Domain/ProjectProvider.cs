using System;
using System.Collections.Generic;
using System.Linq;
using Journalist;
using NotificationService;
using ProjectManagement.Application;
using ProjectManagement.Domain.Events;
using ProjectManagement.Infrastructure;

namespace ProjectManagement.Domain
{
    public class ProjectProvider : IProjectProvider
    {
        public ProjectProvider(
            IProjectManagerGateway projectManagerGateway,
            IVersionControlSystemGateway versionControlSystemGateway,
            IProjectRepository projectRepository,
            IEventSink eventSink,
            IUserRepository userRepository)
        {
            Require.NotNull(projectManagerGateway, nameof(projectManagerGateway));
            Require.NotNull(versionControlSystemGateway, nameof(versionControlSystemGateway));
            Require.NotNull(projectRepository, nameof(projectRepository));
            Require.NotNull(eventSink, nameof(eventSink));
            Require.NotNull(userRepository, nameof(userRepository));

            _projectManagerGateway = projectManagerGateway;
            _versionControlSystemGateway = versionControlSystemGateway;
            _projectRepository = projectRepository;
            _eventSink = eventSink;
            _userRepository = userRepository;
        }

        public List<Project> GetProjects(Func<Project, bool> predicate = null)
        {
            return _projectRepository.GetAllProjects(predicate).ToList();
        }

        public Project GetProject(int projectId)
        {
            Require.Positive(projectId, nameof(projectId));
            var project = _projectRepository.GetProject(projectId);
            if (project == null)
            {
                throw new ProjectNotFoundException();
            }

            var issues = _projectManagerGateway.GetProjectIssues(project.ProjectManagementSystemId);
            project.Issues.AddRange(issues);
            return project;
        }

        public void CreateProject(CreateProjectRequest request)
        {
            Require.NotNull(request, nameof(request));

            var versionControlSystemId = _versionControlSystemGateway.CreateRepositoryForProject(request);
            var projectManagementSystemId = _projectManagerGateway.CreateProject(request);

            var project = new Project(
                request.Name,
                request.ProjectType,
                request.Info,
                ProjectStatus.Planned,
                request.LandingImageUri,
                request.AccessLevel,
                versionControlSystemId,
                projectManagementSystemId,
                null,
                null,
                null);
            var projectId = _projectRepository.SaveProject(project);

            _eventSink.ConsumeEvent(new NewProjectCreated(projectId));
        }

        public void UpdateProject(Project project)
        {
            Require.NotNull(project, nameof(project));

            _projectRepository.UpdateProject(project);
        }

        public void AddUserToProject(int projectId, int userId, string role)
        {
            Require.Positive(projectId, nameof(projectId));
            Require.Positive(userId, nameof(userId));

            var project = GetProject(projectId);
            if (project.ProjectMemberships.Any(developer => developer.DeveloperId == userId))
            {
                throw new InvalidOperationException("Attempt to add developer who is already on project");
            }

            var redmineUserId = _userRepository.GetUserRedmineId(userId);
            var gitlabUserId = _userRepository.GetUserGitlabId(userId);

            project.ProjectMemberships.Add(new ProjectMembership(
                userId, 
                role,
                project));

            _projectManagerGateway.AddNewUserToProject(project.ProjectManagementSystemId, redmineUserId);
            _versionControlSystemGateway.AddUserToRepository(project, gitlabUserId);

            UpdateProject(project);

            _eventSink.ConsumeEvent(new NewDeveloperOnProject(userId, projectId));
        }

        public void RemoveUserFromProject(int projectId, int userId)
        {
            Require.Positive(projectId, nameof(projectId));
            Require.Positive(userId, nameof(userId));

            var project = GetProject(projectId);
            if (project.ProjectMemberships.All(developer => developer.DeveloperId != userId))
            {
                throw new InvalidOperationException("Attempt to remove developer who is not on project");
            }

            var developerToDelete = project.ProjectMemberships
                .SingleOrDefault(developer => developer.DeveloperId == userId);
            project.ProjectMemberships.Remove(developerToDelete);

            var redmineUserId = _userRepository.GetUserRedmineId(userId);
            var gitlabUserId = _userRepository.GetUserGitlabId(userId);

            _projectManagerGateway.RemoveUserFromProject(project.ProjectManagementSystemId, redmineUserId);
            _versionControlSystemGateway.RemoveUserFromProject(project, gitlabUserId);

            UpdateProject(project);

            _eventSink.ConsumeEvent(new DeveloperHasLeftProject(userId, projectId));
        }

        private readonly IEventSink _eventSink;
        private readonly IUserRepository _userRepository;

        private readonly IProjectManagerGateway _projectManagerGateway;
        private readonly IProjectRepository _projectRepository;
        private readonly IVersionControlSystemGateway _versionControlSystemGateway;
    }
}