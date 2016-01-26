using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using BinaryAnalysis.UnidecodeSharp;
using Journalist;
using Journalist.Collections;
using ProjectManagement.Application;
using ProjectManagement.Domain;
using ProjectManagement.Infrastructure;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using UserManagement.Application;
using UserManagement.Infrastructure;
using Issue = ProjectManagement.Domain.Issue;
using Project = Redmine.Net.Api.Types.Project;
using ProjectMembership = Redmine.Net.Api.Types.ProjectMembership;
using ProjectStatus = Redmine.Net.Api.Types.ProjectStatus;

namespace Gateways.Redmine
{
    public class ProjectManagerGateway : IProjectManagerGateway, IRedmineUserRegistrar
    {
        private readonly RedmineManager _redmineManager;

        public ProjectManagerGateway(RedmineSettings redmineSettings)
        {
            Require.NotNull(redmineSettings, nameof(redmineSettings));

            _redmineManager = new RedmineManager(redmineSettings.RedmineHost, redmineSettings.ApiKey);
        }

        public void AddNewUserToProject(int redmineProjectId, int redmineUserId)
        {
            Require.NotNull(redmineProjectId, nameof(redmineProjectId));
            Require.Positive(redmineUserId, nameof(redmineUserId));

            var membership = new ProjectMembership
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
                _redmineManager.GetObjectList<ProjectMembership>(
                    new NameValueCollection {{"project_id", redmineProjectId.ToString()}});
            var membershipToDelete = projectMemberships.SingleOrDefault(
                membership => membership.User.Id == redmineUserId);
            if (membershipToDelete == null)
            {
                throw new InvalidOperationException("Attempt to delete user that is not on project");
            }

            _redmineManager.DeleteObject<ProjectMembership>(
                membershipToDelete.Id.ToString(),
                null);
        }

        public Issue[] GetProjectIssues(int projectManagerProjectId)
        {
            Require.Positive(projectManagerProjectId, nameof(projectManagerProjectId));

            var parameters = new NameValueCollection {{"project_id", projectManagerProjectId.ToString()}};
            IList<global::Redmine.Net.Api.Types.Issue> issues = null;
            try
            {
                issues = _redmineManager.GetObjectList<global::Redmine.Net.Api.Types.Issue>(parameters);
            }
            catch (RedmineException)
            {
                // todo: add logging here
                return EmptyArray.Get<Issue>();
            }
            if (issues == null)
            {
                return EmptyArray.Get<Issue>();
            }

            var lodIssue = issues.Select(IssueMapper.ToLodIssue);
            return lodIssue.ToArray();
        }

        public int CreateProject(CreateProjectRequest request)
        {
            Require.NotNull(request, nameof(request));

            var project = new Project
            {
                Name = request.Name,
                Identifier = GetProjectIdentifier(request.Name),
                Status = ProjectStatus.Active,
                Description = request.Info,
                IsPublic = request.AccessLevel == AccessLevel.Public
            };

            var readyProject = _redmineManager.CreateObject(project);
            return readyProject.Id;
        }

        public int RegisterUser(CreateAccountRequest createAccountRequest)
        {
            Require.NotNull(createAccountRequest, nameof(createAccountRequest));

            var user = new User
            {
                Email = createAccountRequest.Email,
                FirstName = createAccountRequest.Firstname,
                LastName = createAccountRequest.Lastname,
                Password = createAccountRequest.Password,
                MustChangePassword = false
            };

            var createdUser = _redmineManager.CreateObject(user);
            return createdUser.Id;
        }

        private static string GetProjectIdentifier(string projectName)
        {
            return projectName.Unidecode().Replace(" ", string.Empty).ToLower();
        }
    }
}