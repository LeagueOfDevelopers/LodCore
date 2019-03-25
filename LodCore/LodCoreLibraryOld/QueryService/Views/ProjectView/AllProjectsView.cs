using System.Collections.Generic;
using System.Linq;
using LodCoreLibraryOld.Common;
using LodCoreLibraryOld.Domain.ProjectManagment;
using LodCoreLibraryOld.QueryService.DTOs;

namespace LodCoreLibraryOld.QueryService.Views.ProjectView
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