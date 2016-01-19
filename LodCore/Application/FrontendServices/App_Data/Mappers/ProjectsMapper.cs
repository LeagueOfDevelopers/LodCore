using System;
using System.Collections.Generic;
using System.IO;
using FrontendServices.Models;
using Gateways.Gitlab;
using Gateways.Redmine;
using Journalist;
using Journalist.Collections;
using ProjectManagement.Domain;

namespace FrontendServices.App_Data.Mappers
{
    public class ProjectsMapper
    {
        public ProjectsMapper(RedmineSettings redmineSettings, GitlabSettings gitlabSettings)
        {
            Require.NotNull(redmineSettings, nameof(redmineSettings));
            Require.NotNull(gitlabSettings, nameof(gitlabSettings));

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
                project.ProjectType);
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
                project.ProjectType.YieldArray(),
                project.Info,
                project.ProjectStatus,
                project.LandingImageUri,
                project.AccessLevel,
                redmineUri, 
                gitlabUri,
                new HashSet<Issue>(project.Issues), 
                new HashSet<ProjectMembership>(project.ProjectMemberships),
                new HashSet<Uri>(project.Screenshots));
        }

        private readonly RedmineSettings _redmineSettings;
        private readonly GitlabSettings _gitlabSettings;
    }
}