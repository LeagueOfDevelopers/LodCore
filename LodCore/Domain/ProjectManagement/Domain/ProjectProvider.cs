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
using RabbitMQEventBus;

namespace ProjectManagement.Domain
{
    public class ProjectProvider : IProjectProvider
    {
        public ProjectProvider(
            IProjectRepository projectRepository,
            IEventSink eventSink,
            PaginationSettings paginationSettings, 
            IssuePaginationSettings issuePaginationSettings,
            IEventBus eventBus)
        {
            Require.NotNull(projectRepository, nameof(projectRepository));
            Require.NotNull(eventSink, nameof(eventSink));
            Require.NotNull(paginationSettings, nameof(paginationSettings));
            Require.NotNull(issuePaginationSettings, nameof(issuePaginationSettings));
            Require.NotNull(eventBus, nameof(eventBus));

            _projectRepository = projectRepository;
            _eventSink = eventSink;
            _paginationSettings = paginationSettings;
            _issuePaginationSettings = issuePaginationSettings;
            _eventBus = eventBus;
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

            var @event = new NewProjectCreated(projectId);

            _eventBus.PublishEvent("Notification", "new_project_created", @event);

            _eventSink.ConsumeEvent(@event);

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

            project.ProjectMemberships.Add(new ProjectMembership(
                userId, 
                role));

            UpdateProject(project);

            var @event = new NewDeveloperOnProject(userId, projectId);

            _eventBus.PublishEvent("Notification", "new_developer_on_project", @event);

            _eventSink.ConsumeEvent(@event);
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

            var @event = new DeveloperHasLeftProject(userId, projectId);

            _eventBus.PublishEvent("Notification", "developer_has_left_project", @event);

            _eventSink.ConsumeEvent(@event);
        }

        private readonly IEventSink _eventSink;
        private readonly PaginationSettings _paginationSettings;
        private readonly IssuePaginationSettings _issuePaginationSettings;
        private readonly IProjectRepository _projectRepository;
        private readonly IEventBus _eventBus;
    }
}