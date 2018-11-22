using LodCore.Domain.ProjectManagment;
using LodCore.Infrastructure.DataAccess.Pagination;
using LodCore.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCore.QueryService.Views.ProjectView
{
    public class MinProjectView 
    {
        public MinProjectView(ProjectDto projectDto)
        {
            Id = projectDto.ProjectId;
            Name = projectDto.Name;
            ProjectStatus = projectDto.ProjectStatus;
            LandingImage = new ImageView(projectDto.BigPhotoUri, projectDto.SmallPhotoUri);
        }

        public int Id { get; }
        public string Name { get; }
        public ProjectStatus ProjectStatus { get; }
        public ImageView LandingImage { get; }
    }
}
