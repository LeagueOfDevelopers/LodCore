using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using BinaryAnalysis.UnidecodeSharp;
using Journalist;
using Journalist.Collections;
using Newtonsoft.Json.Linq;
using ProjectManagement;
using ProjectManagement.Application;
using ProjectManagement.Domain;
using ProjectManagement.Infrastructure;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using UserManagement.Domain;
using UserManagement.Infrastructure;
using Issue = ProjectManagement.Domain.Issue;
using IssueStatus = ProjectManagement.IssueStatus;
using Project = Redmine.Net.Api.Types.Project;
using ProjectMembership = Redmine.Net.Api.Types.ProjectMembership;
using ProjectStatus = Redmine.Net.Api.Types.ProjectStatus;

namespace Gateways.Redmine
{
    public class ProjectManagerGateway : IProjectManagerGateway, IRedmineUserRegistrar
    {
        public ProjectManagerGateway(RedmineSettings redmineSettings)
        {
            Require.NotNull(redmineSettings, nameof(redmineSettings));

            _redmineSettings = redmineSettings;
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

        public Issue[] GetProjectIssues(int projectManagerProjectId, int countOfProjects, List<IssueType> issueTypes, List<IssueStatus> statusList)
        {
            Require.Positive(projectManagerProjectId, nameof(projectManagerProjectId));

            var parameters = new NameValueCollection
            {
                {"project_id", projectManagerProjectId.ToString()},
                {"sort", "desc:created_on" },
                {"limit", countOfProjects.ToString()}
            };

            if (issueTypes != null)
            {
                var issueTypesInts = issueTypes.Select(e => (int)e);
                parameters.Add( "tracker_id", string.Join("|", issueTypesInts));
            }

            if (statusList!=null)
            {
                var statusListInts = statusList.Select(e => (int)e);
                parameters.Add( "status_id", string.Join("|", statusListInts));
            }

            var paramsQuerry = string.Join("&",
                parameters.AllKeys.Select(a => a + "=" + HttpUtility.UrlEncode(parameters[a])));

            var client = new HttpClient();
            var address = _redmineSettings.RedmineHost + "/issues.json";
            var authHeaderByteArray = Encoding.ASCII.GetBytes($"{_redmineSettings.ApiKey}:pass");
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authHeaderByteArray));

            var response = client.GetAsync(address + "?" + paramsQuerry).Result;
            var returnedIssuesAsString = response.Content.ReadAsStringAsync().Result;

            var issues = JObject.Parse(returnedIssuesAsString)["issues"].ToObject<global::Redmine.Net.Api.Types.Issue[]>();
            issues = issues ?? new global::Redmine.Net.Api.Types.Issue[0];
            var lodIssue = issues.Select(IssueMapper.ToLodIssue);

            return lodIssue.ToArray();
        }

        public RedmineProjectInfo CreateProject(ProjectActionRequest actionRequest)
        {
            Require.NotNull(actionRequest, nameof(actionRequest));

            var project = new Project
            {
                Name = actionRequest.Name,
                Identifier = GetProjectIdentifier(actionRequest.Name),
                Status = ProjectStatus.Active,
                Description = actionRequest.Info,
                IsPublic = actionRequest.AccessLevel == AccessLevel.Public
            };

            var readyProject = _redmineManager.CreateObject(project);
            return new RedmineProjectInfo(
                readyProject.Id, 
                new Uri($"{_redmineSettings.RedmineHost}/projects/{readyProject.Identifier}"),
                readyProject.Identifier);
        }

        public int RegisterUser(Account account)
        {
            Require.NotNull(account, nameof(account));
            
            var user = new RedmineUser
            {
                Email = account.Email.Address,
                Login = GetLoginForLastname(account.Lastname),
                FirstName = account.Firstname,
                LastName = account.Lastname,
                Password = account.Password.Value,
                MustChangePassword = false
            };

            var client = new HttpClient();
            var address = _redmineSettings.RedmineHost + "/users.json";
            var authHeaderByteArray = Encoding.ASCII.GetBytes($"{_redmineSettings.ApiKey}:pass");
            client.DefaultRequestHeaders.Authorization 
                = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authHeaderByteArray));
            var response = client.PostAsync(
                address, 
                user, 
                new XmlMediaTypeFormatter {UseXmlSerializer = true}).Result;
            var createdUserString = response.Content.ReadAsStringAsync().Result;
            var createdUser = JObject.Parse(createdUserString);
            return (int)createdUser["user"]["id"];
        }

        private static string GetProjectIdentifier(string projectName)
        {
            return projectName.Unidecode().Replace(" ", string.Empty).ToLower();
        }

        private static string GetLoginForLastname(string lastName)
        {
            return lastName.Unidecode().Replace(@"'", string.Empty);
        }

        private readonly RedmineManager _redmineManager;
        private readonly RedmineSettings _redmineSettings;
    }
}