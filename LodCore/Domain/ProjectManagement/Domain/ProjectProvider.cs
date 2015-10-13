using System.Collections.Generic;
using System.Linq;
using Journalist;
using ProjectManagement.Application;
using ProjectManagement.Infrastructure;

namespace ProjectManagement.Domain
{
    public class ProjectProvider : IProjectProvider
    {
        public ProjectProvider(IProjectManagerGateway gateway, IProjectRepository repository)
        {
            Require.NotNull(gateway, nameof(gateway));
            Require.NotNull(repository, nameof(repository));

            _gateway = gateway;
            _repository = repository;
        }

        public List<Project> GetProjects()
        {
            return _repository.GetAllProjects().ToList();
        }

        public Project GetProject(int projectId)
        {
            throw new System.NotImplementedException();
        }

        public void CreateProject(Project project)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateProject(Project project)
        {
            throw new System.NotImplementedException();
        }

        public void AddUserToProject(int projectId, int userId)
        {
            throw new System.NotImplementedException();
        }

        private IProjectManagerGateway _gateway;
        private IProjectRepository _repository;
    }
}