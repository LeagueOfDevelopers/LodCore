﻿using LodCoreLibrary.Domain.ProjectManagment;
using LodCoreLibrary.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Views
{
    public class ShortProjectView
    {
        public ShortProjectView(ProjectDto projectDto)
        {
            ProjectId = projectDto.ProjectId;
            Name = projectDto.Name;
            ProjectStatus = projectDto.ProjectStatus;
            LandingImage = new ImageView(projectDto.BigPhotoUri, projectDto.SmallPhotoUri);
        }

        public int ProjectId { get; }
        public string Name { get; }
        public ProjectStatus ProjectStatus { get; }
        public ImageView LandingImage { get; }
    }
}
