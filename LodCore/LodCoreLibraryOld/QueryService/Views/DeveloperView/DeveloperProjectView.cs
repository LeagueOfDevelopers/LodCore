using LodCoreLibrary.QueryService.Views.ProjectView;
using LodCoreLibraryOld.Domain.ProjectManagment;
using LodCoreLibraryOld.QueryService.DTOs;

namespace LodCoreLibraryOld.QueryService.Views.DeveloperView
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