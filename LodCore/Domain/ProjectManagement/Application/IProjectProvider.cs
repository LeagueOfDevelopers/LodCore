using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ProjectManagement.Domain;

namespace ProjectManagement.Application
{
    public interface IProjectProvider
    {
        List<Project> GetProjects(Func<Project, bool> predicate = null);

        List<Project> GetProjects(int projectsToSkip, int projectsToReturn, Expression<Func<Project, bool>> predicate = null);

        Project GetProject(int projectId, List<IssueType> issueTypes = null, List<IssueStatus> statusList = null);

        int CreateProject(CreateProjectRequest request);

        void UpdateProject(Project project);

        void AddUserToProject(int projectId, int userId, string role);

        void RemoveUserFromProject(int projectId, int userId);
    }
}