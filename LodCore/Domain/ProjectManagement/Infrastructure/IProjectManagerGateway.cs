using System.Collections.Generic;
using ProjectManagement.Application;
using ProjectManagement.Domain;

namespace ProjectManagement.Infrastructure
{
    public interface IProjectManagerGateway
    {
        void AddNewUserToProject(int redmineProjectId, int redmineUserId);

        void RemoveUserFromProject(int redmineProjectId, int redmineUserId);

        Issue[] GetProjectIssues(int projectManagerProjectId, int countOfProjects, List<IssueType> issueTypes, List<IssueStatus> statusList);

        RedmineProjectInfo CreateProject(ProjectActionRequest actionRequest);
    }
}