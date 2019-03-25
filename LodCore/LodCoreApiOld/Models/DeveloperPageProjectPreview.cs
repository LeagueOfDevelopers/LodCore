using System;
using Journalist;
using LodCoreLibraryOld.Domain.ProjectManagment;

namespace LodCoreApiOld.Models
{
    public class DeveloperPageProjectPreview
    {
        public DeveloperPageProjectPreview(
            int projectId,
            Uri photoUri,
            string name,
            ProjectStatus projectStatus,
            string developerRole)
        {
            Require.Positive(projectId, nameof(projectId));
            Require.NotEmpty(name, nameof(name));
            Require.NotNull(developerRole, nameof(developerRole));

            ProjectId = projectId;
            PhotoUri = photoUri;
            Name = name;
            ProjectStatus = projectStatus;
            DeveloperRole = developerRole;
        }

        public int ProjectId { get; }

        public Uri PhotoUri { get; }

        public string Name { get; }

        public ProjectStatus ProjectStatus { get; }

        public string DeveloperRole { get; }
    }
}