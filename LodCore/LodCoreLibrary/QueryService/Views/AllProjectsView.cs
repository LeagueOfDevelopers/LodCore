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
            var result = new List<ProjectView>();
            allProjects.ToList().ForEach(p => result.Add(new ProjectView(p)));
            AllProjects = result;
        }

        public IEnumerable<ProjectView> AllProjects { get; }
    }
}
