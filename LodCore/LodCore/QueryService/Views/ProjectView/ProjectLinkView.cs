using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCore.QueryService.Views.ProjectView
{
    public class ProjectLinkView : IEquatable<ProjectLinkView>
    {
        public ProjectLinkView(string name, string uri)
        {
            Name = name;
            Uri = uri;
        }

        public string Name { get; }
        public string Uri { get; }

        public bool Equals(ProjectLinkView other)
        {
            return Uri == Uri;
        }
    }
}
