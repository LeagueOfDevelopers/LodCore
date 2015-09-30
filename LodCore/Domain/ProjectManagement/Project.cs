﻿using System;
using System.Collections.Generic;
using Journalist;
using UserManagement.Domain;

namespace ProjectManagement
{
    public class Project
    {
        public Project(
            int projectId, 
            string name, 
            ProjectType projectType, 
            string info, 
            Uri versionControlSystemUri, 
            Uri projectManagementSystemUri, 
            IReadOnlyList<Issue> issues, 
            List<Account> projectUsers)
        {
            Require.NotEmpty(name, nameof(name));
            Require.NotNull(info, nameof(info));

            ProjectId = projectId;
            Name = name;
            ProjectType = projectType;
            Info = info;
            VersionControlSystemUri = versionControlSystemUri;
            ProjectManagementSystemUri = projectManagementSystemUri;
            Issues = issues;
            ProjectUsers = projectUsers ?? new List<Account>();
        }

        public int ProjectId { get; private set; }

        public string Name { get; private set; }

        public ProjectType ProjectType { get; private set; }

        public string Info { get; private set; }

        public Uri VersionControlSystemUri { get; private set; }

        public Uri ProjectManagementSystemUri { get; private set; }

        public IReadOnlyList<Issue> Issues { get; private set; }

        public IReadOnlyList<Account> ProjectUsers { get; private set; } 
    }
}