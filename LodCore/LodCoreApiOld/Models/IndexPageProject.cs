using System;

namespace LodCoreApiOld.Models
{
    public class IndexPageProject
    {
        public IndexPageProject(int projectId, Uri photoUri, string name)
        {
            ProjectId = projectId;
            PhotoUri = photoUri;
            Name = name;
        }

        public int ProjectId { get; }

        public Uri PhotoUri { get; }

        public string Name { get; }
    }
}