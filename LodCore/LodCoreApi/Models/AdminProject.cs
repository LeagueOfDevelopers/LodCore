using System;
using System.Collections.Generic;
using Journalist;
using LodCore.Domain.ProjectManagment;

namespace LodCoreApi.Models
{
    public class AdminProject
    {
        public AdminProject(
            int projectId,
            string name,
            ProjectType[] projectType,
            string info,
            ProjectStatus projectStatus,
            LodCore.Common.Image landingImage,
            HashSet<ProjectMembership> projectDevelopers,
            HashSet<LodCore.Common.Image> screenshots,
            HashSet<Uri> linksToGithubRepositories)
        {
            Require.Positive(projectId, nameof(projectId));
            Require.NotEmpty(name, nameof(name));
            Require.NotNull(info, nameof(info));

            ProjectId = projectId;
            Name = name;
            ProjectType = projectType ?? new[] {LodCore.Domain.ProjectManagment.ProjectType.Other};
            Info = info;
            ProjectStatus = projectStatus;
            LandingImage = landingImage;
            ProjectMemberships = projectDevelopers ?? new HashSet<ProjectMembership>();
            Screenshots = screenshots ?? new HashSet<LodCore.Common.Image>();
            LinksToGithubRepositories = linksToGithubRepositories ?? new HashSet<Uri>();
        }

        public int ProjectId { get; }

        public string Name { get; }

        public ProjectType[] ProjectType { get; }

        public string Info { get; }

        public ProjectStatus ProjectStatus { get; }

        public LodCore.Common.Image LandingImage { get; }

        public HashSet<ProjectMembership> ProjectMemberships { get; }

        public HashSet<LodCore.Common.Image> Screenshots { get; }

        public HashSet<Uri> LinksToGithubRepositories { get; }
    }
}