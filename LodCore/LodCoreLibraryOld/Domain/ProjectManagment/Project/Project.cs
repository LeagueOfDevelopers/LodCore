using System;
using System.Collections.Generic;
using Journalist;
using LodCoreLibraryOld.Common;

namespace LodCoreLibraryOld.Domain.ProjectManagment
{
    public class Project
    {
        public Project(
            string name,
            ISet<ProjectType> projectTypes,
            string info,
            ProjectStatus projectStatus,
            Image landingImage,
            ISet<ProjectMembership> projectDevelopers,
            ISet<Image> screenshots,
            ISet<ProjectLink> links,
            ISet<Uri> linksToGithubRepositories)
        {
            Require.NotEmpty(name, nameof(name));
            Require.NotNull(info, nameof(info));
            Require.NotEmpty(projectTypes, nameof(projectTypes));

            Name = name;
            ProjectTypes = projectTypes;
            Info = info;
            ProjectStatus = projectStatus;
            LandingImage = landingImage;
            ProjectMemberships = projectDevelopers ?? new HashSet<ProjectMembership>();
            Screenshots = screenshots ?? new HashSet<Image>();
            Links = links ?? new HashSet<ProjectLink>();
            LinksToGithubRepositories = linksToGithubRepositories ?? new HashSet<Uri>();
        }

        public int ProjectId { get; protected set; }
        public string Name { get; set; }
        public ISet<ProjectType> ProjectTypes { get; set; }
        public string Info { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public Image LandingImage { get; set; }
        public ISet<ProjectMembership> ProjectMemberships { get; protected set; }
        public ISet<Image> Screenshots { get; set; }
        public ISet<ProjectLink> Links { get; set; }
        public ISet<Uri> LinksToGithubRepositories { get; set; }
    }
}