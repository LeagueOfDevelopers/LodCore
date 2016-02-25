using System;
using Journalist;

namespace ProjectManagement
{
    public class RedmineProjectInfo
    {
        public RedmineProjectInfo(int projectId, Uri projectUrl, string projectIdentifier)
        {
            Require.Positive(projectId, nameof(projectId));
            Require.NotNull(projectUrl, nameof(projectUrl));
            Require.NotEmpty(projectIdentifier, nameof(projectIdentifier));

            ProjectId = projectId;
            ProjectUrl = projectUrl;
            ProjectIdentifier = projectIdentifier;
        }

        public RedmineProjectInfo()
        {
        }

        public int ProjectId { get; protected set; } 

        public Uri ProjectUrl { get; protected set; }

        public string ProjectIdentifier { get; protected set; }
    }
}