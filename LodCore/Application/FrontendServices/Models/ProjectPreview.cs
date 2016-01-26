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
            ProjectType projectType)
        {
            Require.Positive(projectId, nameof(projectId));
            Require.NotEmpty(name, nameof(name));

            ProjectId = projectId;
            PhotoUri = photoUri;
            Name = name;
            ProjectStatus = projectStatus;
            ProjectType = projectType;
        }

        public int ProjectId { get; private set; }

        public Uri PhotoUri { get; private set; }

        public string Name { get; private set; }

        public ProjectStatus ProjectStatus { get; private set; }

        public ProjectType ProjectType { get; private set; }
    }
}