using System;
using System.Collections.Generic;
using Journalist;
using LodCoreLibraryOld.Domain.ProjectManagment;

namespace LodCoreApiOld.Models
{
    public class AdminProject
    {
        public AdminProject(
            int projectId,
            string name,
            ProjectType[] projectType,
            string info,
            ProjectStatus projectStatus,
            LodCoreLibraryOld.Common.Image landingImage,
            HashSet<ProjectMembership> projectDevelopers,
            HashSet<LodCoreLibraryOld.Common.Image> screenshots,
            HashSet<Uri> linksToGithubRepositories)
        {
            Require.Positive(projectId, nameof(projectId));
            Require.NotEmpty(name, nameof(name));
            Require.NotNull(info, nameof(info));

            ProjectId = projectId;
            Name = name;
            ProjectType = projectType ?? new[] { LodCoreLibraryOld.Domain.ProjectManagment.ProjectType.Other};
            Info = info;
            ProjectStatus = projectStatus;
            LandingImage = landingImage;
            ProjectMemberships = projectDevelopers ?? new HashSet<ProjectMembership>();
            Screenshots = screenshots ?? new HashSet<LodCoreLibraryOld.Common.Image>();
            LinksToGithubRepositories = linksToGithubRepositories ?? new HashSet<Uri>();
        }

        public int ProjectId { get; private set; }

        public string Name { get; private set; }

        public ProjectType[] ProjectType { get; private set; }

        public string Info { get; private set; }

        public ProjectStatus ProjectStatus { get; private set; }

        public LodCoreLibraryOld.Common.Image LandingImage { get; private set; }

        public HashSet<ProjectMembership> ProjectMemberships { get; private set; }

        public HashSet<LodCoreLibraryOld.Common.Image> Screenshots { get; private set; }

        public HashSet<Uri> LinksToGithubRepositories { get; private set; }
    }
}