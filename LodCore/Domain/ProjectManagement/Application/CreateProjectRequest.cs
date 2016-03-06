using System;
using Common;
using Journalist;
using ProjectManagement.Domain;

namespace ProjectManagement.Application
{
    public class CreateProjectRequest
    {
        public CreateProjectRequest(
            string name, 
            ProjectType[] projectTypes, 
            string info, 
            ProjectStatus projectStatus,
            AccessLevel accessLevel, 
            Uri landingImageUri, 
            Uri[] screenshots)
        {
            Require.NotEmpty(name, nameof(name));
            Require.NotEmpty(info, nameof(info));
            Require.NotEmpty(projectTypes, nameof(projectTypes));

            Name = name;
            ProjectTypes = projectTypes;
            Info = info;
            AccessLevel = accessLevel;
            LandingImageUri = landingImageUri;
            ProjectStatus = projectStatus;
            Screenshots = screenshots;
        }

        public string Name { get; private set; }

        public ProjectType[] ProjectTypes { get; private set; }

        public ProjectStatus ProjectStatus { get; private set; }

        public string Info { get; private set; }

        public AccessLevel AccessLevel { get; private set; }

        public Uri LandingImageUri { get; private set; }

        public Uri[] Screenshots { get; private set; }
    }
}