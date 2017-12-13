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
            IProjectRepository projectRepository,
            IEventSink eventSink,
            IUserRepository userRepository,
            PaginationSettings paginationSettings, IssuePaginationSettings issuePaginationSettings)
        {
            Require.NotNull(projectRepository, nameof(projectRepository));
            Require.NotNull(eventSink, nameof(eventSink));
            Require.NotNull(userRepository, nameof(userRepository));
            Require.NotNull(paginationSettings, nameof(paginationSettings));

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
                project => project.ProjectStatus == ProjectStatus.Done // well, that's a funny workaround
                    ? 1 // i can't figure out, how to sort it other way
                    : project.ProjectStatus == ProjectStatus.InProgress // even simple switch clause doesn't work here
                        ? 2 // proper way to do this is rearrange enum values and change database accordingly
                        : project.ProjectStatus == ProjectStatus.Planned ? 3 : 4, // but it would be tough to do this all the time i want to change order
                predicate); // so let it be so
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

           /* var issues = _projectManagerGateway.GetProjectIssues(project.RedmineProjectInfo.ProjectId,
                _issuePaginationSettings.NumberOfIssues, issueTypes, statusList);

            foreach (var issue in issues)
            {
                project.Issues.Add(issue);
            } */

            return project;
        } 

        public int CreateProject(CreateProjectRequest request)
        {
            Require.NotNull(request, nameof(request));

            var project = new Project(
                request.Name,
                new HashSet<ProjectType>(request.ProjectTypes),
                request.Info,
                request.ProjectStatus,
                request.LandingImage,
                request.AccessLevel,
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

            UpdateProject(project);

            _eventSink.ConsumeEvent(new DeveloperHasLeftProject(userId, projectId));
        }

        private readonly IEventSink _eventSink;
        private readonly IUserRepository _userRepository;
        private readonly PaginationSettings _paginationSettings;
        private readonly IssuePaginationSettings _issuePaginationSettings;

        private readonly IProjectRepository _projectRepository;
    }
}