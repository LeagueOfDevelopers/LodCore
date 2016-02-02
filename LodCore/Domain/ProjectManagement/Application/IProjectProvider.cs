using System;
using System.Collections.Generic;
using ProjectManagement.Domain;

namespace ProjectManagement.Application
{
    public interface IProjectProvider
    {
        List<Project> GetProjects(Func<Project, bool> predicate = null);

        List<Project> GetProjects(int pageNumber);

        Project GetProject(int projectId);

        void CreateProject(CreateProjectRequest request);

        void UpdateProject(Project project);

        void AddUserToProject(int projectId, int userId, string role);

        void RemoveUserFromProject(int projectId, int userId);
    }
}