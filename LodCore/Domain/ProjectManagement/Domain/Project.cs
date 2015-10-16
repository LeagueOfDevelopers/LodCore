﻿using System;
using System.Collections.Generic;
using Journalist;

namespace ProjectManagement.Domain
{
    public class Project
    {
        public Project(
            string name, 
            ProjectType projectType, 
            string info, 
            ProjectStatus projectStatus,
            Uri landingImageUri, 
            AccessLevel accessLevel, 
            Uri versionControlSystemUri, 
            Uri projectManagementSystemUri, 
            List<Issue> issues, 
            List<int> projectUserIds,
            List<Uri> screenshots)
        {
            Require.NotEmpty(name, nameof(name));
            Require.NotNull(info, nameof(info));
            Require.NotNull(versionControlSystemUri, nameof(versionControlSystemUri));
            Require.NotNull(projectManagementSystemUri, nameof(projectManagementSystemUri));

            Name = name;
            ProjectType = projectType;
            AccessLevel = accessLevel;
            Info = info;
            ProjectStatus = projectStatus;
            LandingImageUri = landingImageUri;
            VersionControlSystemUri = versionControlSystemUri;
            ProjectManagementSystemUri = projectManagementSystemUri;
            Issues = issues ?? new List<Issue>();
            ProjectUserIds = projectUserIds ?? new List<int>();
            Screenshots = screenshots ?? new List<Uri>();
        }

        public int ProjectId { get; protected set; }

        public string Name { get; private set; }

        public ProjectType ProjectType { get; private set; }

        public AccessLevel AccessLevel { get; private set; }

        public string Info { get; private set; }

        public ProjectStatus ProjectStatus { get; private set; }

        public Uri LandingImageUri { get; private set; }

        public Uri VersionControlSystemUri { get; private set; }

        public Uri ProjectManagementSystemUri { get; private set; }

        public List<Issue> Issues { get; private set; }

        public List<int> ProjectUserIds { get; private set; } 

        public List<Uri> Screenshots { get; private set; } 
    }
}