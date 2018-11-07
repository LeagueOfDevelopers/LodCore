using LodCoreLibrary.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Views
{
    public class SomeProjectsView
    {
        public SomeProjectsView(IEnumerable<ProjectDto> allProjects)
        {
            var result = new List<MinProjectView>();
            allProjects.ToList().ForEach(p => result.Add(new MinProjectView(p)));
            Projects = result;
        }

        public IEnumerable<MinProjectView> Projects { get; }
    }
}
