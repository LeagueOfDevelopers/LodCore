using LodCore.Domain.ProjectManagment;
using LodCore.QueryService.DTOs;
using LodCore.QueryService.Views.ProjectView;

namespace LodCore.QueryService.Views.DeveloperView
{
    public class DeveloperProjectView
    {
        public DeveloperProjectView(ProjectDto projectDto, ProjectMembershipDto projMembershipDto)
        {
            ProjectName = projectDto.Name;
            ProjectStatus = projectDto.ProjectStatus;
            LandingImage = new ImageView(projectDto.BigPhotoUri, projectDto.SmallPhotoUri);
            DeveloperRole = projMembershipDto.Role;
        }

        public string ProjectName { get; }
        public ProjectStatus ProjectStatus { get; }
        public ImageView LandingImage { get; }
        public string DeveloperRole { get; }
    }
}