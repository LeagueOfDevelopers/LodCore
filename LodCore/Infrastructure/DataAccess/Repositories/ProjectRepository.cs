using System;
using NotificationService;
using ProjectManagement.Domain;
using ProjectManagement.Infrastructure;

namespace DataAccess.Repositories
{
    public class ProjectRepository : IProjectRepository, IProjectRelativesRepository
    {
        public Project[] GetAllProjects(Func<Project, bool> criteria = null)
        {
            throw new NotImplementedException();
        }

        public Project GetProject(int projectId)
        {
            throw new NotImplementedException();
        }

        public int SaveProject(Project project)
        {
            throw new NotImplementedException();
        }

        public void UpdateProject(Project project)
        {
            throw new NotImplementedException();
        }

        public int[] GetAllProjectRelativeIds(int projectId)
        {
            return GetProject(projectId).ProjectUserIds.ToArray();
        }
    }
}