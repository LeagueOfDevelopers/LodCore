﻿using LodCore.Common;
using LodCore.Domain.ProjectManagment;
using LodCore.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCore.QueryService.Views.ProjectView
{
    public class AllProjectsView
    {
        public AllProjectsView(IEnumerable<ProjectDto> projects)
        {
            Projects = projects.Select(p => new ShortDeveloperInfoView(p));
        }

        public IEnumerable<ShortDeveloperInfoView> Projects { get; private set; }

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
