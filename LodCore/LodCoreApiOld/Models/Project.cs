using System;
using System.Collections.Generic;
using Journalist;
using LodCoreLibraryOld.Common;
using LodCoreLibraryOld.Domain.ProjectManagment;

namespace LodCoreApiOld.Models
{
    public class Project
    {
        public Project(
            int projectId,
            string name,
            ProjectType[] projectType,
            string info,
            ProjectStatus projectStatus,
            LodCoreLibraryOld.Common.Image landingImage,
            HashSet<ProjectMembership> projectMemberships,
            HashSet<LodCoreLibraryOld.Common.Image> screenshots,
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

        public LodCoreLibraryOld.Common.Image LandingImage { get; }

        public Uri VersionControlSystemUri { get; private set; }

        public Uri ProjectManagementSystemUri { get; private set; }

        public HashSet<ProjectMembership> ProjectMemberships { get; }

        public HashSet<LodCoreLibraryOld.Common.Image> Screenshots { get; }

        public HashSet<ProjectLink> Links { get; }

        public HashSet<Uri> LinksToGithubRepositories { get; }
    }
}