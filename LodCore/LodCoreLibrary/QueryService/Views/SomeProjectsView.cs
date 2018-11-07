using LodCoreLibrary.Domain.ProjectManagment;
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
        public SomeProjectsView(IEnumerable<ProjectDto> allProjects, int allProjectCount)
        {
            var result = new List<MinProjectView>();
            allProjects.ToList().ForEach(p => result.Add(new MinProjectView(p)));
            Projects = result;

            AllProjectsCount = allProjectCount;
        }

        public SomeProjectsView(IEnumerable<MinProjectView> projects, int allProjectsCount)
        {
            Projects = projects;
            AllProjectsCount = allProjectsCount;
        }

        public IEnumerable<MinProjectView> Projects { get; }
        public int AllProjectsCount { get; }

        public SomeProjectsView FilterResult()
        {
            var resultProjects = Projects.Where(p => p.ProjectStatus == ProjectStatus.Done
                || p.ProjectStatus == ProjectStatus.InProgress);
            return new SomeProjectsView(resultProjects, AllProjectsCount);
        }
    }
}
