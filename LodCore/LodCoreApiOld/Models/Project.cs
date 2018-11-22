using System;
using System.Collections.Generic;
using Journalist;
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
            HashSet<LodCoreLibraryOld.Common.ProjectLink> links,
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

        public int ProjectId { get; private set; }

        public string Name { get; private set; }

        public ProjectType[] ProjectType { get; private set; }

        public string Info { get; private set; }

        public ProjectStatus ProjectStatus { get; private set; }

        public LodCoreLibraryOld.Common.Image LandingImage { get; private set; }

        public Uri VersionControlSystemUri { get; private set; }

        public Uri ProjectManagementSystemUri { get; private set; }

        public HashSet<ProjectMembership> ProjectMemberships { get; private set; }

        public HashSet<LodCoreLibraryOld.Common.Image> Screenshots { get; private set; }

        public HashSet<LodCoreLibraryOld.Common.ProjectLink> Links { get; private set; }

        public HashSet<Uri> LinksToGithubRepositories { get; private set; }
    }
}