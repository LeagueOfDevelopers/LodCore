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
            AccessLevel accessLevel, 
            Uri versionControlSystemUri, 
            Uri projectManagementSystemUri, 
            List<Issue> issues, 
            List<int> projectUserIds)
        {
            Require.NotEmpty(name, nameof(name));
            Require.NotNull(info, nameof(info));
            Require.NotNull(versionControlSystemUri, nameof(versionControlSystemUri));
            Require.NotNull(projectManagementSystemUri, nameof(projectManagementSystemUri));
            Require.NotNull(issues, nameof(issues));
            Require.NotNull(projectUserIds, nameof(projectUserIds));

            Name = name;
            ProjectType = projectType;
            Info = info;
            ProjectStatus = projectStatus;
            AccessLevel = accessLevel;
            VersionControlSystemUri = versionControlSystemUri;
            ProjectManagementSystemUri = projectManagementSystemUri;
            Issues = issues;
            ProjectUserIds = projectUserIds;
        }

        public int ProjectId { get; protected set; }

        public string Name { get; private set; }

        public ProjectType ProjectType { get; private set; }

        public AccessLevel AccessLevel { get; private set; }

        public string Info { get; private set; }

        public ProjectStatus ProjectStatus { get; private set; }

        public Uri VersionControlSystemUri { get; private set; }

        public Uri ProjectManagementSystemUri { get; private set; }

        public List<Issue> Issues { get; private set; }

        public List<int> ProjectUserIds { get; private set; } 
    }
}