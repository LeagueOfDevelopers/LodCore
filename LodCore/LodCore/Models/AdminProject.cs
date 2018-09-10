using System;
using System.Collections.Generic;
using Journalist;
using LodCoreLibrary.Domain.ProjectManagment;

namespace LodCore.Models
{
    public class AdminProject
    {
        public AdminProject(
            int projectId,
            string name,
            ProjectType[] projectType,
            string info,
            ProjectStatus projectStatus,
            LodCoreLibrary.Common.Image landingImage,
            HashSet<Issue> issues,
            HashSet<ProjectMembership> projectDevelopers,
            HashSet<LodCoreLibrary.Common.Image> screenshots,
            HashSet<Uri> linksToGithubRepositories)
        {
            Require.Positive(projectId, nameof(projectId));
            Require.NotEmpty(name, nameof(name));
            Require.NotNull(info, nameof(info));

            ProjectId = projectId;
            Name = name;
            ProjectType = projectType ?? new[] {LodCoreLibrary.Domain.ProjectManagment.ProjectType.Other};
            Info = info;
            ProjectStatus = projectStatus;
            LandingImage = landingImage;
            Issues = issues ?? new HashSet<Issue>();
            ProjectMemberships = projectDevelopers ?? new HashSet<ProjectMembership>();
            Screenshots = screenshots ?? new HashSet<LodCoreLibrary.Common.Image>();
            LinksToGithubRepositories = linksToGithubRepositories ?? new HashSet<Uri>();
        }

        public int ProjectId { get; private set; }

        public string Name { get; private set; }

        public ProjectType[] ProjectType { get; private set; }

        public string Info { get; private set; }

        public ProjectStatus ProjectStatus { get; private set; }

        public LodCoreLibrary.Common.Image LandingImage { get; private set; }

        public HashSet<Issue> Issues { get; private set; }

        public HashSet<ProjectMembership> ProjectMemberships { get; private set; }

        public HashSet<LodCoreLibrary.Common.Image> Screenshots { get; private set; }

        public HashSet<Uri> LinksToGithubRepositories { get; private set; }
    }
}