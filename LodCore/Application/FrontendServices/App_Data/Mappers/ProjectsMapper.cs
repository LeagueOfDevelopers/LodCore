using System;
using System.Collections.Generic;
using System.Linq;
using FrontendServices.Models;
using Gateways.Gitlab;
using Gateways.Redmine;
using Journalist;
using ProjectManagement.Domain;
using UserManagement.Application;
using ProjectMembership = ProjectManagement.Domain.ProjectMembership;
using ProjectMembershipDto = FrontendServices.Models.ProjectMembership;

namespace FrontendServices.App_Data.Mappers
{
    public class ProjectsMapper
    {
        private readonly GitlabSettings _gitlabSettings;
        private readonly RedmineSettings _redmineSettings;

        private readonly IUserManager _userManager;

        public ProjectsMapper(
            IUserManager userManager,
            RedmineSettings redmineSettings,
            GitlabSettings gitlabSettings)
        {
            Require.NotNull(userManager, nameof(userManager));
            Require.NotNull(redmineSettings, nameof(redmineSettings));
            Require.NotNull(gitlabSettings, nameof(gitlabSettings));

            _userManager = userManager;
            _redmineSettings = redmineSettings;
            _gitlabSettings = gitlabSettings;
        }

        public IndexPageProject ToIndexPageProject(Project project)
        {
            Require.NotNull(project, nameof(project));

            return new IndexPageProject(project.ProjectId, project.LandingImageUri, project.Name);
        }

        public ProjectPreview ToProjectPreview(Project project)
        {
            Require.NotNull(project, nameof(project));

            return new ProjectPreview(
                project.ProjectId,
                project.LandingImageUri,
                project.Name,
                project.ProjectStatus,
                project.ProjectTypes.ToArray());
        }

        public AdminProject ToAdminProject(Project project)
        {
            var redmineUri = new Uri(new Uri(_redmineSettings.RedmineHost),
                project.ProjectManagementSystemId.ToString());
            var gitlabUri = new Uri(new Uri(_gitlabSettings.Host),
                project.VersionControlSystemId.ToString());
            return new AdminProject(
                project.ProjectId,
                project.Name,
                project.ProjectTypes.ToArray(),
                project.Info,
                project.ProjectStatus,
                project.LandingImageUri,
                project.AccessLevel,
                redmineUri,
                gitlabUri,
                new HashSet<Issue>(project.Issues),
                new HashSet<ProjectMembershipDto>(project.ProjectMemberships.Select(ToProjectMembershipDto)),
                new HashSet<Uri>(project.Screenshots));
        }

        private ProjectMembershipDto ToProjectMembershipDto(ProjectMembership projectMembership)
        {
            var user = _userManager.GetUser(projectMembership.DeveloperId);
            return new ProjectMembershipDto(user.UserId, user.Firstname, user.Lastname, projectMembership.Role);
        }
    }
}