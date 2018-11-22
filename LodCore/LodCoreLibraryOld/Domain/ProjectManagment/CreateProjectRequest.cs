using System;
using Journalist;
using LodCoreLibraryOld.Common;

namespace LodCoreLibraryOld.Domain.ProjectManagment
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

        public string Name { get; private set; }

        public ProjectType[] ProjectTypes { get; private set; }

        public ProjectStatus ProjectStatus { get; private set; }

        public string Info { get; private set; }

        public Image LandingImage { get; private set; }

        public Image[] Screenshots { get; private set; }

        public ProjectLink[] Links { get; private set; }

        public Uri[] LinksToGithubRepositories { get; private set; }
    }
}