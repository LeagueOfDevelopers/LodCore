using LodCoreLibrary.Common;
using LodCoreLibrary.Domain.ProjectManagment;
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
        public AllProjectsView(IEnumerable<ProjectDto> projects)
        {
            Projects = projects.Select(p => new MinProjectView(p));
        }

        public IEnumerable<MinProjectView> Projects { get; private set; }

        public void FilterResult()
        {
            Projects = Projects.Where(p => p.ProjectStatus == ProjectStatus.Done
                || p.ProjectStatus == ProjectStatus.InProgress);
        }

        public void SelectRandomProjects(int count)
        {
            Projects = Projects.GetRandom(count);
        }
    }
}
