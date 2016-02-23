using System;
using System.Collections.Generic;
using Common;
using Journalist;

namespace ProjectManagement.Domain
{
    public class Project
    {
        public Project(
            string name,
            ISet<ProjectType> projectTypes,
            string info,
            ProjectStatus projectStatus,
            Uri landingImageUri,
            AccessLevel accessLevel,
            int versionControlSystemId,
            int projectManagementSystemId,
            ISet<Issue> issues,
            ISet<ProjectMembership> projectDevelopers,
            ISet<Uri> screenshots)
        {
            Require.NotEmpty(name, nameof(name));
            Require.NotNull(info, nameof(info));
            Require.NotNull(versionControlSystemId, nameof(versionControlSystemId));
            Require.NotNull(projectManagementSystemId, nameof(projectManagementSystemId));
            Require.NotEmpty(projectTypes, nameof(projectTypes));

            Name = name;
            ProjectTypes = projectTypes;
            AccessLevel = accessLevel;
            Info = info;
            ProjectStatus = projectStatus;
            LandingImageUri = landingImageUri;
            VersionControlSystemId = versionControlSystemId;
            ProjectManagementSystemId = projectManagementSystemId;
            Issues = issues ?? new HashSet<Issue>();
            ProjectMemberships = projectDevelopers ?? new HashSet<ProjectMembership>();
            Screenshots = screenshots ?? new HashSet<Uri>();
        }

        protected Project()
        {
        }

        public virtual int ProjectId { get; protected set; }

        public virtual string Name { get; protected set; }

        public virtual ISet<ProjectType> ProjectTypes { get; protected set; }

        public virtual AccessLevel AccessLevel { get; protected set; }

        public virtual string Info { get; protected set; }

        public virtual ProjectStatus ProjectStatus { get; protected set; }

        public virtual Uri LandingImageUri { get; protected set; }

        public virtual int VersionControlSystemId { get; protected set; }

        public virtual int ProjectManagementSystemId { get; protected set; }

        public virtual ISet<Issue> Issues { get; protected set; } = new HashSet<Issue>();

        public virtual ISet<ProjectMembership> ProjectMemberships { get; protected set; }

        public virtual ISet<Uri> Screenshots { get; protected set; }
    }
}