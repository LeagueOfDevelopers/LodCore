using System;
using System.Collections.Generic;
using Journalist;
using LodCore.Common;
using LodCore.Domain.ProjectManagment;

namespace LodCoreApi.Models
{
    public class Project
    {
        public Project(
            int projectId,
            string name,
            ProjectType[] projectType,
            string info,
            ProjectStatus projectStatus,
            LodCore.Common.Image landingImage,
            HashSet<ProjectMembership> projectMemberships,
            HashSet<LodCore.Common.Image> screenshots,
            HashSet<ProjectLink> links,
            HashSet<Uri> linksToGithubRepositories)
        {
            Require.Positive(projectId, nameof(projectId));
            Require.NotEmpty(name, nameof(name));
            Require.NotNull(info, nameof(info));

            ProjectId = projectId;
            Name = name;
            ProjectType = projectType;
            Info = info;
            ProjectStatus = projectStatus;
            LandingImage = landingImage;
            ProjectMemberships = projectMemberships;
            Screenshots = screenshots;
            Links = links;
            LinksToGithubRepositories = linksToGithubRepositories;
        }

        public int ProjectId { get; }

        public string Name { get; }

        public ProjectType[] ProjectType { get; }

        public string Info { get; }

        public ProjectStatus ProjectStatus { get; }

        public LodCore.Common.Image LandingImage { get; }

        public Uri VersionControlSystemUri { get; private set; }

        public Uri ProjectManagementSystemUri { get; private set; }

        public HashSet<ProjectMembership> ProjectMemberships { get; }

        public HashSet<LodCore.Common.Image> Screenshots { get; }

        public HashSet<ProjectLink> Links { get; }

        public HashSet<Uri> LinksToGithubRepositories { get; }
    }
}