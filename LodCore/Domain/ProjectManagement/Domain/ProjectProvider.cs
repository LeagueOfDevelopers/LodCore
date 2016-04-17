using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Common;
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
            IUserRepository userRepository,
            PaginationSettings paginationSettings, IssuePaginationSettings issuePaginationSettings)
        {
            Require.NotNull(projectManagerGateway, nameof(projectManagerGateway));
            Require.NotNull(versionControlSystemGateway, nameof(versionControlSystemGateway));
            Require.NotNull(projectRepository, nameof(projectRepository));
            Require.NotNull(eventSink, nameof(eventSink));
            Require.NotNull(userRepository, nameof(userRepository));
            Require.NotNull(paginationSettings, nameof(paginationSettings));

            _projectManagerGateway = projectManagerGateway;
            _versionControlSystemGateway = versionControlSystemGateway;
            _projectRepository = projectRepository;
            _eventSink = eventSink;
            _userRepository = userRepository;
            _paginationSettings = paginationSettings;
            _issuePaginationSettings = issuePaginationSettings;
        }

        public List<Project> GetProjects(Func<Project, bool> predicate = null)
        {
            var allProjects = _projectRepository.GetAllProjects(predicate);
            return allProjects.ToList();
        }

        public List<Project> GetProjects(int pageNumber, Expression<Func<Project, bool>> predicate = null)
        {
            var projectsToSkip = pageNumber*_paginationSettings.PageSize;
            var requiredProjects = _projectRepository.GetSomeProjects(
                projectsToSkip,
                _paginationSettings.PageSize,
                predicate);
            return requiredProjects.ToList();
        }

        public Project GetProject(int projectId, List<IssueType> issueTypes = null, List<IssueStatus> statusList = null)
        {
            Require.Positive(projectId, nameof(projectId));
            var project = _projectRepository.GetProject(projectId);
            if (project == null)
            {
                throw new ProjectNotFoundException();
            }

            var issues = _projectManagerGateway.GetProjectIssues(project.RedmineProjectInfo.ProjectId,
                _issuePaginationSettings.NumberOfIssues, issueTypes, statusList);

            foreach (var issue in issues)
            {
                project.Issues.Add(issue);
            }

            return project;
        }

        public int CreateProject(CreateProjectRequest request)
        {
            Require.NotNull(request, nameof(request));

            var versionControlSystemInfo = _versionControlSystemGateway.CreateRepositoryForProject(request);
            var projectManagementSystemId = _projectManagerGateway.CreateProject(request);

            var project = new Project(
                request.Name,
                new HashSet<ProjectType>(request.ProjectTypes),
                request.Info,
                request.ProjectStatus,
                request.LandingImage,
                request.AccessLevel,
                versionControlSystemInfo,
                projectManagementSystemId,
                null,
                null,
                request.Screenshots != null 
                ? new HashSet<Image>(request.Screenshots) 
                : null );
            var projectId = _projectRepository.SaveProject(project);

            _eventSink.ConsumeEvent(new NewProjectCreated(projectId));

            return projectId;
        }

        public void UpdateProject(Project project)
        {
            Require.NotNull(project, nameof(project));

            _versionControlSystemGateway.UpdateRepositoryForProject(project);

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
                role));

            if (redmineUserId != 0)
            {
                _projectManagerGateway.AddNewUserToProject(
                    project.RedmineProjectInfo.ProjectId, 
                    redmineUserId);
            }

            if (gitlabUserId != 0)
            {
                _versionControlSystemGateway.AddUserToRepository(project, gitlabUserId);
            }

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

            if (redmineUserId != 0)
            {
                _projectManagerGateway.RemoveUserFromProject(
                    project.RedmineProjectInfo.ProjectId, 
                    redmineUserId);
            }

            if (gitlabUserId != 0)
            {
                _versionControlSystemGateway.RemoveUserFromProject(
                    project, 
                    gitlabUserId);
            }

            UpdateProject(project);

            _eventSink.ConsumeEvent(new DeveloperHasLeftProject(userId, projectId));
        }

        private readonly IEventSink _eventSink;
        private readonly IUserRepository _userRepository;
        private readonly PaginationSettings _paginationSettings;
        private readonly IssuePaginationSettings _issuePaginationSettings;

        private readonly IProjectManagerGateway _projectManagerGateway;
        private readonly IProjectRepository _projectRepository;
        private readonly IVersionControlSystemGateway _versionControlSystemGateway;
    }
}