using System;
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
            int versionControlSystemId,
            int projectManagementSystemId,
            List<Issue> issues,
            List<int> projectUserIds,
            List<Uri> screenshots)
        {
            Require.NotEmpty(name, nameof(name));
            Require.NotNull(info, nameof(info));
            Require.NotNull(versionControlSystemId, nameof(versionControlSystemId));
            Require.NotNull(projectManagementSystemId, nameof(projectManagementSystemId));

            Name = name;
            ProjectType = projectType;
            AccessLevel = accessLevel;
            Info = info;
            ProjectStatus = projectStatus;
            LandingImageUri = landingImageUri;
            VersionControlSystemId = versionControlSystemId;
            ProjectManagementSystemId = projectManagementSystemId;
            Issues = issues ?? new List<Issue>();
            ProjectUserIds = projectUserIds ?? new List<int>();
            Screenshots = screenshots ?? new List<Uri>();
        }

        protected Project()
        {
        }

        public virtual int ProjectId { get; protected set; }

        public virtual string Name { get; protected set; }

        public virtual ProjectType ProjectType { get; protected set; }

        public virtual AccessLevel AccessLevel { get; protected set; }

        public virtual string Info { get; protected set; }

        public virtual ProjectStatus ProjectStatus { get; protected set; }

        public virtual Uri LandingImageUri { get; protected set; }

        public virtual int VersionControlSystemId { get; protected set; }

        public virtual int ProjectManagementSystemId { get; protected set; }

        public virtual List<Issue> Issues { get; protected set; }

        public virtual List<int> ProjectUserIds { get; protected set; }

        public virtual List<Uri> Screenshots { get; protected set; }
    }
}