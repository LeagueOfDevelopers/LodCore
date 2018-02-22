﻿using System;
using System.Collections.Generic;
using Common;
using Journalist;
using ProjectManagement.Domain;

namespace FrontendServices.Models
{
    public class Project
    {
        public Project(
            int projectId,
            string name,
            ProjectType[] projectType,
            string info,
            ProjectStatus projectStatus,
            Common.Image landingImage,
            HashSet<Issue> issues,
            HashSet<ProjectMembership> projectMemberships,
            HashSet<Common.Image> screenshots,
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
            Issues = issues;
            ProjectMemberships = projectMemberships;
            Screenshots = screenshots;
            LinksToGithubRepositories = linksToGithubRepositories;
        }

        public int ProjectId { get; private set; }

        public string Name { get; private set; }

        public ProjectType[] ProjectType { get; private set; }

        public string Info { get; private set; }

        public ProjectStatus ProjectStatus { get; private set; }

        public Common.Image LandingImage { get; private set; }

        public Uri VersionControlSystemUri { get; private set; }

        public Uri ProjectManagementSystemUri { get; private set; }

        public HashSet<Issue> Issues { get; private set; }

        public HashSet<ProjectMembership> ProjectMemberships { get; private set; }

        public HashSet<Common.Image> Screenshots { get; private set; }

        public HashSet<Uri> LinksToGithubRepositories { get; private set; }
    }
}