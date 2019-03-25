using LodCore.Domain.ProjectManagment;
using LodCore.QueryService.DTOs;

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