using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Journalist;
using LodCoreLibraryOld.Common;
using LodCoreLibraryOld.Domain.Exceptions;
using LodCoreLibraryOld.Domain.NotificationService;
using LodCoreLibraryOld.Domain.ProjectManagment;
using LodCoreLibraryOld.Infrastructure.DataAccess.Repositories;
using LodCoreLibraryOld.Infrastructure.EventBus;

namespace LodCoreLibraryOld.Facades
{
    public class ProjectProvider : IProjectProvider
    {
        private readonly IEventPublisher _eventPublisher;

        private readonly IProjectRepository _projectRepository;

        public ProjectProvider(
            IProjectRepository projectRepository,
            IEventPublisher eventPublisher
        )
        {
            Require.NotNull(projectRepository, nameof(projectRepository));
            Require.NotNull(eventPublisher, nameof(eventPublisher));

            _projectRepository = projectRepository;
            _eventPublisher = eventPublisher;
        }

        public List<Project> GetProjects(Func<Project, bool> predicate = null)
        {
            var allProjects = _projectRepository.GetAllProjects(predicate);
            return allProjects.ToList();
        }

        public List<Project> GetProjects(int projectsToSkip, int projectsToReturn,
            Expression<Func<Project, bool>> predicate = null)
        {
            var requiredProjects = _projectRepository.GetSomeProjects(
                projectsToSkip,
                projectsToReturn,
                project => project.ProjectStatus == ProjectStatus.Done // well, that's a funny workaround
                    ? 1 // i can't figure out, how to sort it other way
                    : project.ProjectStatus == ProjectStatus.InProgress // even simple switch clause doesn't work here
                        ? 2 // proper way to do this is rearrange enum values and change database accordingly
                        : project.ProjectStatus == ProjectStatus.Planned
                            ? 3
                            : 4, // but it would be tough to do this all the time i want to change order
                predicate); // so let it be so
            return requiredProjects.ToList();
        }

        public Project GetProject(int projectId, List<IssueType> issueTypes = null, List<IssueStatus> statusList = null)
        {
            Require.Positive(projectId, nameof(projectId));
            var project = _projectRepository.GetProject(projectId);
            if (project == null) throw new ProjectNotFoundException();

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
                null,
                request.Screenshots != null
                    ? new HashSet<Image>(request.Screenshots)
                    : null,
                request.Links != null
                    ? new HashSet<ProjectLink>(request.Links)
                    : null,
                request.LinksToGithubRepositories != null
                    ? new HashSet<Uri>(request.LinksToGithubRepositories)
                    : null);
            var projectId = _projectRepository.SaveProject(project);

            var @event = new NewProjectCreated(projectId, project.Name);

            _eventPublisher.PublishEvent(@event);

            return projectId;
        }

        public void UpdateProject(Project project)
        {
            Require.NotNull(project, nameof(project));

            _projectRepository.UpdateProject(project);
        }

        public void AddUserToProject(int projectId, int userId, string role, string firstName, string lastName,
            string projectName)
        {
            Require.Positive(projectId, nameof(projectId));
            Require.Positive(userId, nameof(userId));

            var project = GetProject(projectId);
            if (project.ProjectMemberships.Any(developer => developer.DeveloperId == userId))
                throw new InvalidOperationException("Attempt to add developer who is already on project");

            project.ProjectMemberships.Add(new ProjectMembership(
                userId,
                role));

            UpdateProject(project);

            var @event = new NewDeveloperOnProject(userId, projectId, firstName, lastName, projectName);

            _eventPublisher.PublishEvent(@event);
        }

        public void RemoveUserFromProject(int projectId, int userId, string firstName, string lastName,
            string projectName)
        {
            Require.Positive(projectId, nameof(projectId));
            Require.Positive(userId, nameof(userId));

            var project = GetProject(projectId);
            if (project.ProjectMemberships.All(developer => developer.DeveloperId != userId))
                throw new InvalidOperationException("Attempt to remove developer who is not on project");

            var developerToDelete = project.ProjectMemberships
                .SingleOrDefault(developer => developer.DeveloperId == userId);
            project.ProjectMemberships.Remove(developerToDelete);

            UpdateProject(project);

            var @event = new DeveloperHasLeftProject(userId, projectId, firstName, lastName, projectName);

            _eventPublisher.PublishEvent(@event);
        }
    }
}