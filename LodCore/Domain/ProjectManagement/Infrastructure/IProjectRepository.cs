using System;
using ProjectManagement.Domain;

namespace ProjectManagement.Infrastructure
{
    public interface IProjectRepository
    {
        Project[] GetAllProjects(Func<Project, bool> criteria = null);

        Project GetProject(int projectId);

        int SaveProject(Project project);

        void UpdateProject(Project project);
    }
}