using LodCoreLibraryOld.Domain.ProjectManagment;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LodCoreLibraryOld.Facades
{
    public interface IProjectProvider
    {
        List<Project> GetProjects(Func<Project, bool> predicate = null);

        List<Project> GetProjects(int projectsToSkip, int projectsToReturn, Expression<Func<Project, bool>> predicate = null);

        Project GetProject(int projectId, List<IssueType> issueTypes = null, List<IssueStatus> statusList = null);

        int CreateProject(CreateProjectRequest request);

        void UpdateProject(Project project);

        void AddUserToProject(int projectId, int userId, string role, string firstName, string lastName, string projectName);

        void RemoveUserFromProject(int projectId, int userId, string firstName, string lastName, string projectName);
    }
}