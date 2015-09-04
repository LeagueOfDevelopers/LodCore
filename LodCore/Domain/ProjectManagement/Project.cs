using System;
using System.Collections.Generic;
using Journalist;
using UserManagement;

namespace ProjectManagement
{
    public class Project
    {
        public Project(
            Guid projectId, 
            string name, 
            ProjectType projectType, 
            string info, 
            Uri versionControlSystemUri, 
            Uri projectManagementSystemUri, 
            List<Issue> issues, 
            List<UserSummary> projectUsers)
        {
            Require.NotEmpty(projectId, nameof(projectId));
            Require.NotEmpty(name, nameof(name));
            Require.NotNull(info, nameof(info));

            ProjectId = projectId;
            Name = name;
            ProjectType = projectType;
            Info = info;
            VersionControlSystemUri = versionControlSystemUri;
            ProjectManagementSystemUri = projectManagementSystemUri;
            Issues = issues ?? new List<Issue>();
            ProjectUsers = projectUsers ?? new List<UserSummary>();
        }

        public Guid ProjectId { get; private set; }

        public string Name { get; private set; }

        public ProjectType ProjectType { get; private set; }

        public string Info { get; private set; }

        public Uri VersionControlSystemUri { get; private set; }

        public Uri ProjectManagementSystemUri { get; private set; }

        public List<Issue> Issues { get; private set; }

        public List<UserSummary> ProjectUsers { get; private set; } 
    }
}