using System.Collections.Generic;
using System.Linq;
using LodCoreApi.Models;
using Journalist;
using Project = LodCoreLibrary.Domain.ProjectManagment.Project;
using ProjectMembership = LodCoreLibrary.Domain.ProjectManagment.ProjectMembership;
using ProjectMembershipDto = LodCoreApi.Models.ProjectMembership;
using LodCoreLibrary.Facades;
using LodCoreLibrary.Domain.ProjectManagment;

namespace LodCoreApi.App_Data.Mappers
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
                new HashSet<Issue>(project.Issues),
                new HashSet<ProjectMembershipDto>(project.ProjectMemberships.Select(ToProjectMembershipDto)),
                new HashSet<LodCoreLibrary.Common.Image>(project.Screenshots),
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
                new HashSet<Issue>(project.Issues),
                new HashSet<ProjectMembershipDto>(project.ProjectMemberships.Select(ToProjectMembershipDto)),
                new HashSet<LodCoreLibrary.Common.Image>(project.Screenshots),
                new HashSet<LodCoreLibrary.Common.ProjectLink>(project.Links),
                new HashSet<System.Uri>(project.LinksToGithubRepositories));
        }

        private ProjectMembershipDto ToProjectMembershipDto(ProjectMembership projectMembership)
        {
            var user = _userManager.GetUser(projectMembership.DeveloperId);
            return new ProjectMembershipDto(user.UserId, user.Firstname, user.Lastname, projectMembership.Role);
        }
    }
}