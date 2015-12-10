using System;
using System.Collections.Specialized;
using System.Linq;
using Journalist;
using ProjectManagement.Application;
using ProjectManagement.Domain;
using ProjectManagement.Infrastructure;
using Redmine.Net.Api;
using ProjectStatus = Redmine.Net.Api.Types.ProjectStatus;

namespace Gateways
{
    public class ProjectManagerGateway : IProjectManagerGateway
    {
        public ProjectManagerGateway(RedmineManager redmineManager)
        {
            Require.NotNull(redmineManager, nameof(redmineManager));

            _redmineManager = redmineManager;
        }

        public void AddNewUserToProject(int redmineProjectId, int redmineUserId)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserFromProject(int redmineProjectId, int redmineUserId)
        {
            Require.NotNull(redmineProjectId, nameof(redmineProjectId));
            Require.Positive(redmineUserId, nameof(redmineUserId));

            throw new NotImplementedException();
        }

        public Issue[] GetProjectIssues(int projectManagerProjectId)
        {
            Require.Positive(projectManagerProjectId, nameof(projectManagerProjectId));

            var parameters = new NameValueCollection {{"project_id", projectManagerProjectId.ToString()}};
            var issues = _redmineManager.GetObjectList<Redmine.Net.Api.Types.Issue>(parameters);
            var lodIssue = issues.Select(IssueMapper.ToLodIssue);
            return lodIssue.ToArray();
        }

        public int CreateProject(CreateProjectRequest request)
        {
            Require.NotNull(request, nameof(request));

            var project = new Redmine.Net.Api.Types.Project
            {
                Name = request.Name,
                Identifier = request.Name,
                Status = ProjectStatus.Active,
                Description = request.Info,
                IsPublic = request.AccessLevel == AccessLevel.Public
            };

            var readyProject = _redmineManager.CreateObject(project);
            return readyProject.Id;
        }

        private readonly RedmineManager _redmineManager;
    }
}