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
            ISet<Issue> issues, 
            ISet<ProjectMembership> projectDevelopers,
            ISet<Image> screenshots)
        {
            Require.NotEmpty(name, nameof(name));
            Require.NotNull(info, nameof(info));
            Require.NotEmpty(projectTypes, nameof(projectTypes));

            Name = name;
            ProjectTypes = projectTypes;
            AccessLevel = accessLevel;
            Info = info;
            ProjectStatus = projectStatus;
            LandingImage = landingImage;
            Issues = issues ?? new HashSet<Issue>();
            ProjectMemberships = projectDevelopers ?? new HashSet<ProjectMembership>();
            Screenshots = screenshots ?? new HashSet<Image>();
        }

        protected Project()
        {
        }

        public virtual int ProjectId { get; protected set; }

        public virtual string Name { get; set; }

        public virtual ISet<ProjectType> ProjectTypes { get; set; }

        public virtual AccessLevel AccessLevel { get; set; }

        public virtual string Info { get; set; }

        public virtual ProjectStatus ProjectStatus { get; set; }

        public virtual Image LandingImage { get; set; }

        public virtual ISet<Issue> Issues { get; set; } = new HashSet<Issue>();

        public virtual ISet<ProjectMembership> ProjectMemberships { get; protected set; }

        public virtual ISet<Image> Screenshots { get; set; }
    }
}