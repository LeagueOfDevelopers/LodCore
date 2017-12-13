using System;
using System.Collections.Generic;
using Common;
using Journalist;
using ProjectManagement.Domain;

namespace FrontendServices.Models
{
    public class AdminProject
    {
        public AdminProject(
            int projectId,
            string name,
            ProjectType[] projectType,
            string info,
            ProjectStatus projectStatus,
            Common.Image landingImage,
            AccessLevel accessLevel,
            HashSet<Issue> issues,
            HashSet<ProjectMembership> projectDevelopers,
            HashSet<Common.Image> screenshots)
        {
            Require.Positive(projectId, nameof(projectId));
            Require.NotEmpty(name, nameof(name));
            Require.NotNull(info, nameof(info));

            ProjectId = projectId;
            Name = name;
            ProjectType = projectType ?? new[] {Common.ProjectType.Other};
            AccessLevel = accessLevel;
            Info = info;
            ProjectStatus = projectStatus;
            LandingImage = landingImage;
            Issues = issues ?? new HashSet<Issue>();
            ProjectMemberships = projectDevelopers ?? new HashSet<ProjectMembership>();
            Screenshots = screenshots ?? new HashSet<Common.Image>();
        }

        public int ProjectId { get; private set; }

        public string Name { get; private set; }

        public ProjectType[] ProjectType { get; private set; }

        public AccessLevel AccessLevel { get; private set; }

        public string Info { get; private set; }

        public ProjectStatus ProjectStatus { get; private set; }

        public Common.Image LandingImage { get; private set; }

        public HashSet<Issue> Issues { get; private set; }

        public HashSet<ProjectMembership> ProjectMemberships { get; private set; }

        public HashSet<Common.Image> Screenshots { get; private set; }
    }
}