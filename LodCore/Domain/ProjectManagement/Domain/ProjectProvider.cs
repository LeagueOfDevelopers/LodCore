using System;
using System.Collections.Generic;
using System.Linq;
using Journalist;
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
            IProjectRepository repository,
            ProjectsEventSink eventSink)
        {
            Require.NotNull(projectManagerGateway, nameof(projectManagerGateway));
            Require.NotNull(versionControlSystemGateway, nameof(versionControlSystemGateway));
            Require.NotNull(repository, nameof(repository));
            Require.NotNull(eventSink, nameof(eventSink));

            _projectManagerGateway = projectManagerGateway;
            _versionControlSystemGateway = versionControlSystemGateway;
            _repository = repository;
            _eventSink = eventSink;
        }

        public List<Project> GetProjects(Func<Project, bool> predicate = null)
        {
            return _repository.GetAllProjects(predicate).ToList();
        }

        public Project GetProject(int projectId)
        {
            Require.Positive(projectId, nameof(projectId));
            var project = _repository.GetProject(projectId);

            if (project == null)
            {
                throw new ProjectNotFoundException();
            }

            return project;
        }

        public void CreateProject(CreateProjectRequest request)
        {
            Require.NotNull(request, nameof(request));

            var vcsLink = _versionControlSystemGateway.CreateRepositoryForProject(request);
            var pmLink = _projectManagerGateway.CreateProject(request);
            if (vcsLink == null || pmLink == null)
            {
                throw new ProjectCreationFailedException("Failed to create repository or project");
            }

            var project = new Project(
                request.Name, 
                request.ProjectType, 
                request.Info, 
                ProjectStatus.Planned,
                request.AccessLevel, 
                vcsLink, 
                pmLink, 
                new List<Issue>(), 
                new List<int>());
            var projectId = _repository.SaveProject(project);

            _eventSink.SendNewProjectCreatedEvent(projectId);
        }

        public void UpdateProject(Project project)
        {
            Require.NotNull(project, nameof(project));

            _repository.UpdateProject(project);
        }

        public void AddUserToProject(int projectId, int userId)
        {
            Require.Positive(projectId, nameof(projectId));
            Require.Positive(userId, nameof(userId));

            var project = GetProject(projectId);
            if (project.ProjectUserIds.Contains(userId))
            {
                throw new InvalidOperationException("Attempt to add developer who is already on project");
            }

            project.ProjectUserIds.Add(userId);

            _projectManagerGateway.AddNewUserToProject(project, userId);
            _versionControlSystemGateway.AddUserToProject(project, userId);

            UpdateProject(project);

            _eventSink.SendNewDeveloperEvent(projectId, userId);
        }

        public void RemoveUserFromProject(int projectId, int userId)
        {
            Require.Positive(projectId, nameof(projectId));
            Require.Positive(userId, nameof(userId));

            var project = GetProject(projectId);
            if (!project.ProjectUserIds.Contains(userId))
            {
                throw new InvalidOperationException("Attempt to remove developer who is not on project");
            }

            project.ProjectUserIds.Remove(userId);

            _projectManagerGateway.RemoveUserFromProject(project, userId);
            _versionControlSystemGateway.RemoveUserFromProject(project, userId);

            UpdateProject(project);

            _eventSink.SendDeveloperHasLeftProjectEvent(projectId, userId);
        }

        private readonly IProjectManagerGateway _projectManagerGateway;
        private readonly IVersionControlSystemGateway _versionControlSystemGateway;
        private readonly IProjectRepository _repository;
        private readonly ProjectsEventSink _eventSink;
    }
}