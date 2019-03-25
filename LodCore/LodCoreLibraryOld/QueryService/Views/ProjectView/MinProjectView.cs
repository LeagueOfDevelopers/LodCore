using LodCoreLibrary.QueryService.Views.ProjectView;
using LodCoreLibraryOld.Domain.ProjectManagment;
using LodCoreLibraryOld.QueryService.DTOs;

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