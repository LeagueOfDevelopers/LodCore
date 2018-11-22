using LodCoreLibrary.QueryService.Views.ProjectView;
using LodCoreLibraryOld.Domain.ProjectManagment;
using LodCoreLibraryOld.Infrastructure.DataAccess.Pagination;
using LodCoreLibraryOld.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibraryOld.QueryService.Views.ProjectView
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
