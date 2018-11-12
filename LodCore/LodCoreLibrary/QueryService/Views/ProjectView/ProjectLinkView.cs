using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Views.ProjectView
{
    public class ProjectLinkView
    {
        public ProjectLinkView(string name, string uri)
        {
            Name = name;
            Uri = uri;
        }

        public string Name { get; }
        public string Uri { get; }
    }
}
