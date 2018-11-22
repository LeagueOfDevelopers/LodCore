using System.Collections.Generic;
using System.Linq;
using LodCoreApiOld.Models;
using Journalist;
using Project = LodCoreLibraryOld.Domain.ProjectManagment.Project;
using ProjectMembership = LodCoreLibraryOld.Domain.ProjectManagment.ProjectMembership;
using ProjectMembershipDto = LodCoreApiOld.Models.ProjectMembership;
using LodCoreLibraryOld.Facades;
using LodCoreLibraryOld.Domain.ProjectManagment;

namespace LodCoreApiOld.App_Data.Mappers
{
    public class ProjectsMapper
    {
        private readonly IUserManager _userManager;

        public ProjectsMapper(IUserManager userManager)
        {
            Require.NotNull(userManager, nameof(userManager));

            _userManager = userManager;
        }

        public IndexPageProject ToIndexPageProject(Project project)
        {
            Require.NotNull(project, nameof(project));

            return new IndexPageProject(project.ProjectId, project.LandingImage?.SmallPhotoUri, project.Name);
        }

        public ProjectPreview ToProjectPreview(Project project)
        {
            Require.NotNull(project, nameof(project));

            return new ProjectPreview(
                project.ProjectId,
                project.LandingImage?.SmallPhotoUri,
                project.Name,
                project.ProjectStatus,
                project.ProjectTypes.ToArray());
        }

        public AdminProject ToAdminProject(Project project)
        {
            return new AdminProject(
                project.ProjectId,
                project.Name,
                project.ProjectTypes.ToArray(),
                project.Info,
                project.ProjectStatus,
                project.LandingImage,
                new HashSet<ProjectMembershipDto>(project.ProjectMemberships.Select(ToProjectMembershipDto)),
                new HashSet<LodCoreLibraryOld.Common.Image>(project.Screenshots),
                new HashSet<System.Uri>(project.LinksToGithubRepositories));
        }

        public Models.Project ToProject(Project project)
        {
            return new Models.Project(
                project.ProjectId,
                project.Name,
                project.ProjectTypes.ToArray(),
                project.Info,
                project.ProjectStatus,
                project.LandingImage,
                new HashSet<ProjectMembershipDto>(project.ProjectMemberships.Select(ToProjectMembershipDto)),
                new HashSet<LodCoreLibraryOld.Common.Image>(project.Screenshots),
                new HashSet<LodCoreLibraryOld.Common.ProjectLink>(project.Links),
                new HashSet<System.Uri>(project.LinksToGithubRepositories));
        }

        private ProjectMembershipDto ToProjectMembershipDto(ProjectMembership projectMembership)
        {
            var user = _userManager.GetUser(projectMembership.DeveloperId);
            return new ProjectMembershipDto(user.UserId, user.Firstname, user.Lastname, projectMembership.Role);
        }
    }
}