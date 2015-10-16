using System;
using System.Collections.Generic;
using System.Linq;
using Journalist;
using ProjectManagement.Application;
using ProjectManagement.Infrastructure;

namespace ProjectManagement.Domain
{
    public class ProjectProvider : IProjectProvider
    {
        public ProjectProvider(
            IProjectManagerGateway projectManagerGateway,
            IVersionControlSystemGateway versionControlSystemGateway, 
            IProjectRepository repository)
        {
            Require.NotNull(projectManagerGateway, nameof(projectManagerGateway));
            Require.NotNull(versionControlSystemGateway, nameof(versionControlSystemGateway));
            Require.NotNull(repository, nameof(repository));

            _projectManagerGateway = projectManagerGateway;
            _versionControlSystemGateway = versionControlSystemGateway;
            _repository = repository;
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
            _repository.SaveProject(project);
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
            project.ProjectUserIds.Add(userId);

            _projectManagerGateway.AddNewUserToProject(project, userId);
            _versionControlSystemGateway.AddUserToProject(project, userId);

            UpdateProject(project);
        }

        private readonly IProjectManagerGateway _projectManagerGateway;
        private readonly IVersionControlSystemGateway _versionControlSystemGateway;
        private readonly IProjectRepository _repository;
    }
}