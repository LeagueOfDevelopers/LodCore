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
        public SomeProjectsView(IEnumerable<ShortDeveloperInfoView> projects, int allProjectsCount)
        {
            Projects = projects;
            AllProjectsCount = allProjectsCount;
        }

        public IEnumerable<ShortDeveloperInfoView> Projects { get; }
        public int AllProjectsCount { get; }
    }
}
