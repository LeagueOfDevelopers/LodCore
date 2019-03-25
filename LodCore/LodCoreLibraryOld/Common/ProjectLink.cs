using System;

namespace LodCoreLibraryOld.Common
{
    public class ProjectLink
    {
        public ProjectLink()
        {
        }

        public ProjectLink(string name, Uri uri)
        {
            Name = name;
            Uri = uri;
        }

        public virtual string Name { get; set; }
        public virtual Uri Uri { get; set; }
    }
}