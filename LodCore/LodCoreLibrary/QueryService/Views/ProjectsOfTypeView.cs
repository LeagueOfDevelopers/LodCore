using LodCoreLibrary.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Views
{
    public class ProjectsOfTypeView
    {
        public ProjectsOfTypeView(List<ProjectDto> projects)
        {
            var result = new List<ShortProjectView>();
            projects.ForEach(p => result.Add(new ShortProjectView(p)));
            Projects = result;
        }

        public IEnumerable<ShortProjectView> Projects { get; }
    }
}
