using LodCoreLibrary.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Views
{
    public class AllProjectsView
    {
        public AllProjectsView(IEnumerable<ProjectDto> allProjects)
        {
            var result = new List<ShortProjectView>();
            allProjects.ToList().ForEach(p => result.Add(new ShortProjectView(p)));
            AllProjects = result;
        }

        public IEnumerable<ShortProjectView> AllProjects { get; }
    }
}
