using System;
using Journalist;
using LodCore.Common;

namespace LodCore.Domain.ProjectManagment
{
    public class CreateProjectRequest
    {
        public CreateProjectRequest(
            string name,
            ProjectType[] projectTypes,
            string info,
            ProjectStatus projectStatus,
            Image landingImage,
            Image[] screenshots,
            ProjectLink[] links,
            Uri[] linksToGithubRepositories)
        {
            Require.NotEmpty(name, nameof(name));
            Require.NotEmpty(info, nameof(info));
            Require.NotEmpty(projectTypes, nameof(projectTypes));

            Name = name;
            ProjectTypes = projectTypes;
            Info = info;
            LandingImage = landingImage;
            ProjectStatus = projectStatus;
            Screenshots = screenshots;
            Links = links;
            LinksToGithubRepositories = linksToGithubRepositories;
        }

        public string Name { get; }

        public ProjectType[] ProjectTypes { get; }

        public ProjectStatus ProjectStatus { get; }

        public string Info { get; }

        public Image LandingImage { get; }

        public Image[] Screenshots { get; }

        public ProjectLink[] Links { get; }

        public Uri[] LinksToGithubRepositories { get; }
    }
}