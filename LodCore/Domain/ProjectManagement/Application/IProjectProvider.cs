using System.Collections.Generic;
using ProjectManagement.Domain;

namespace ProjectManagement.Application
{
    public interface IProjectProvider
    {
        List<Project> GetProjects();
        
        Project GetProject(int projectId);

        void CreateProject(Project project);

        void UpdateProject(Project project);

        void AddUserToProject(int projectId, int userId);
    }
}
