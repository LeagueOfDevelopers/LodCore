using System;
using Common;
using Journalist;
using ProjectManagement.Domain;

namespace FrontendServices.Models
{
    public class ProjectPreview
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

        public int ProjectId { get; private set; }

        public Uri PhotoUri { get; private set; }

        public string Name { get; private set; }

        public ProjectStatus ProjectStatus { get; private set; }

        public ProjectType[] ProjectTypes { get; private set; }
    }
}