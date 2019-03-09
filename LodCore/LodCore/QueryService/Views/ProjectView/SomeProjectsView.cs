using LodCore.Domain.ProjectManagment;
using LodCore.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCore.QueryService.Views.ProjectView
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

        public IEnumerable<MinProjectView> Projects { get; private set; }
        public int AllProjectsCount { get; private set; }

        public void FilterResult()
        {
            Projects = Projects.Where(p => p.ProjectStatus == ProjectStatus.Done
                || p.ProjectStatus == ProjectStatus.InProgress);
            AllProjectsCount = Projects.Count();
        }

        public SomeProjectsView Take(int offset, int count)
        {
            return new SomeProjectsView(
                Projects
                    .Skip(offset)
                    .Take(count),
                AllProjectsCount);
        }
    }
}
