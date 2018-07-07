using System;
using Journalist;

namespace LodCoreLibrary.Domain.ProjectManagment
{
    public class VersionControlSystemInfo
    {
        public VersionControlSystemInfo(int projectId, Uri projectUrl)
        {
            Require.Positive(projectId, nameof(projectId));
            Require.NotNull(projectUrl, nameof(projectUrl));
            ProjectId = projectId;
            ProjectUrl = projectUrl;
        }

        protected VersionControlSystemInfo()
        {
        }

        public int ProjectId { get; protected set; } 

        public Uri ProjectUrl { get; protected set; }
    }
}