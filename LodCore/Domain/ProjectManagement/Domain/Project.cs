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
            Image landingImage, 
            AccessLevel accessLevel,
            VersionControlSystemInfo versionControlSystemInfo,
            RedmineProjectInfo redmineProjectInfo, 
            ISet<Issue> issues, 
            ISet<ProjectMembership> projectDevelopers,
            ISet<Image> screenshots)
        {
            Require.NotEmpty(name, nameof(name));
            Require.NotNull(info, nameof(info));
            Require.NotNull(versionControlSystemInfo, nameof(versionControlSystemInfo));
            Require.NotNull(redmineProjectInfo, nameof(redmineProjectInfo));
            Require.NotEmpty(projectTypes, nameof(projectTypes));

            Name = name;
            ProjectTypes = projectTypes;
            AccessLevel = accessLevel;
            Info = info;
            ProjectStatus = projectStatus;
            LandingImage = landingImage;
            VersionControlSystemInfo = versionControlSystemInfo;
            RedmineProjectInfo = redmineProjectInfo;
            Issues = issues ?? new HashSet<Issue>();
            ProjectMemberships = projectDevelopers ?? new HashSet<ProjectMembership>();
            Screenshots = screenshots ?? new HashSet<Image>();
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

        public virtual Image LandingImage { get; protected set; }

        public virtual VersionControlSystemInfo VersionControlSystemInfo { get; protected set; }

        public virtual RedmineProjectInfo RedmineProjectInfo { get; protected set; }

        public virtual ISet<Issue> Issues { get; set; } = new HashSet<Issue>();

        public virtual ISet<ProjectMembership> ProjectMemberships { get; protected set; }

        public virtual ISet<Image> Screenshots { get; set; }
    }
}