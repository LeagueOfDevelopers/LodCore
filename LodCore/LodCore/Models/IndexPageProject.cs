using System;

namespace LodCore.Models
{
    public class IndexPageProject
    {
        public IndexPageProject(int projectId, Uri photoUri, string name)
        {
            ProjectId = projectId;
            PhotoUri = photoUri;
            Name = name;
        }

        public int ProjectId { get; private set; }

        public Uri PhotoUri { get; private set; }

        public string Name { get; private set; }
    }
}