﻿using System;
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
            Image landingImage, 
            Image[] screenshots,
            Uri[] linksToGithubRepositories)
        {
            Require.NotEmpty(name, nameof(name));
            Require.NotEmpty(info, nameof(info));
            Require.NotEmpty(projectTypes, nameof(projectTypes));

            Name = name;
            ProjectTypes = projectTypes;
            Info = info;
            AccessLevel = accessLevel;
            LandingImage = landingImage;
            ProjectStatus = projectStatus;
            Screenshots = screenshots;
            LinksToGithubRepositories = linksToGithubRepositories;
        }

        public string Name { get; private set; }

        public ProjectType[] ProjectTypes { get; private set; }

        public ProjectStatus ProjectStatus { get; private set; }

        public string Info { get; private set; }

        public AccessLevel AccessLevel { get; private set; }

        public Image LandingImage { get; private set; }

        public Image[] Screenshots { get; private set; }

        public Uri[] LinksToGithubRepositories { get; private set; }
    }
}