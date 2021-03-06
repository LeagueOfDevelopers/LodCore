﻿using System.Collections.Generic;
using System.Linq;
using LodCoreLibraryOld.Domain.ProjectManagment;
using LodCoreLibraryOld.QueryService.DTOs;

namespace LodCoreLibraryOld.QueryService.Views.ProjectView
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
        public int AllProjectsCount { get; }

        public void FilterResult()
        {
            Projects = Projects.Where(p => p.ProjectStatus == ProjectStatus.Done
                                           || p.ProjectStatus == ProjectStatus.InProgress);
        }
    }
}