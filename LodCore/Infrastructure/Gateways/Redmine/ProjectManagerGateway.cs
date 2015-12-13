using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using BinaryAnalysis.UnidecodeSharp;
using Journalist;
using ProjectManagement.Application;
using ProjectManagement.Domain;
using ProjectManagement.Infrastructure;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using Issue = ProjectManagement.Domain.Issue;
using ProjectStatus = Redmine.Net.Api.Types.ProjectStatus;

namespace Gateways.Redmine
{
    public class ProjectManagerGateway : IProjectManagerGateway
    {
        public ProjectManagerGateway(RedmineSettings redmineSettings)
        {
            Require.NotNull(redmineSettings, nameof(redmineSettings));

            _redmineManager = new RedmineManager(redmineSettings.RedmineHost, redmineSettings.ApiKey);
        }

        public void AddNewUserToProject(int redmineProjectId, int redmineUserId)
        {
            Require.NotNull(redmineProjectId, nameof(redmineProjectId));
            Require.Positive(redmineUserId, nameof(redmineUserId));

            var membership = new global::Redmine.Net.Api.Types.ProjectMembership
            {
                User = new IdentifiableName {Id = redmineUserId},
                Roles = new List<MembershipRole>
                {
                    new MembershipRole
                    {
                        Id = MembershipRoles.DeveloperRoleId
                    }
                },
                Project = new IdentifiableName {Id = redmineProjectId}
            };

            _redmineManager.CreateObject(membership, redmineProjectId.ToString());
        }

        public void RemoveUserFromProject(int redmineProjectId, int redmineUserId)
        {
            Require.NotNull(redmineProjectId, nameof(redmineProjectId));
            Require.Positive(redmineUserId, nameof(redmineUserId));

            var projectMemberships =
                _redmineManager.GetObjectList<global::Redmine.Net.Api.Types.ProjectMembership>(
                    new NameValueCollection {{"project_id", redmineProjectId.ToString()}});
            var membershipToDelete = projectMemberships.SingleOrDefault(
                membership => membership.User.Id == redmineUserId);
            if (membershipToDelete == null)
            {
                throw new InvalidOperationException("Attempt to delete user that is not on project");
            }

            _redmineManager.DeleteObject<global::Redmine.Net.Api.Types.ProjectMembership>(
                membershipToDelete.Id.ToString(),
                null);
        }

        public Issue[] GetProjectIssues(int projectManagerProjectId)
        {
            Require.Positive(projectManagerProjectId, nameof(projectManagerProjectId));

            var parameters = new NameValueCollection {{"project_id", projectManagerProjectId.ToString()}};
            var issues = _redmineManager.GetObjectList<global::Redmine.Net.Api.Types.Issue>(parameters);
            var lodIssue = issues.Select(IssueMapper.ToLodIssue);
            return lodIssue.ToArray();
        }

        public int CreateProject(CreateProjectRequest request)
        {
            Require.NotNull(request, nameof(request));

            var project = new global::Redmine.Net.Api.Types.Project
            {
                Name = request.Name,
                Identifier = request.Name.Unidecode().ToLower(),
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