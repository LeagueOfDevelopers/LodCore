using System;
using Journalist;
using LodCoreLibraryOld.Domain.ProjectManagment;
using LodCoreLibraryOld.Infrastructure.DataAccess.Pagination;

namespace LodCoreApiOld.Models
{
    public class ProjectPreview : IPaginable
    {
        public ProjectPreview(
            int projectId,
            Uri photoUri,
            string name,
            ProjectStatus projectStatus,
            ProjectType[] projectTypes)
        {
            Require.Positive(projectId, nameof(projectId));
            Require.NotEmpty(name, nameof(name));
            Require.NotEmpty(projectTypes, nameof(projectTypes));

            ProjectId = projectId;
            PhotoUri = photoUri;
            Name = name;
            ProjectStatus = projectStatus;
            ProjectTypes = projectTypes;
        }

        public int ProjectId { get; }

        public Uri PhotoUri { get; }

        public string Name { get; }

        public ProjectStatus ProjectStatus { get; }

        public ProjectType[] ProjectTypes { get; }
    }
}