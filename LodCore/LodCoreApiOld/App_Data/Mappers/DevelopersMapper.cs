using System.Collections.Generic;
using System.Linq;
using Journalist;
using LodCoreApiOld.App_Data.Authorization;
using LodCoreApiOld.Models;
using LodCoreLibraryOld.Domain.ProjectManagment;
using LodCoreLibraryOld.Domain.UserManagement;
using LodCoreLibraryOld.Facades;
using Project = LodCoreLibraryOld.Domain.ProjectManagment.Project;

namespace LodCoreApiOld.App_Data.Mappers
{
    public class DevelopersMapper
    {
        private readonly IProjectProvider _projectProvider;

        private readonly IUserRoleAnalyzer _userRoleAnalyzer;

        public DevelopersMapper(
            IUserRoleAnalyzer userRoleAnalyzer,
            IProjectProvider projectProvider)
        {
            Require.NotNull(userRoleAnalyzer, nameof(userRoleAnalyzer));
            Require.NotNull(projectProvider, nameof(projectProvider));

            _userRoleAnalyzer = userRoleAnalyzer;
            _projectProvider = projectProvider;
        }

        public IndexPageDeveloper ToIndexPageDeveloper(Account account)
        {
            Require.NotNull(account, nameof(account));

            var role = _userRoleAnalyzer.GetUserCommonRole(account.UserId);
            return new IndexPageDeveloper(
                account.UserId,
                account.Firstname,
                account.Lastname,
                account.Profile.Image?.SmallPhotoUri,
                role);
        }

        public DeveloperPageDeveloper ToDeveloperPageDeveloper(Account account)
        {
            Require.NotNull(account, nameof(account));

            var role = _userRoleAnalyzer.GetUserCommonRole(account.UserId);

            var projectCount =
                _projectProvider.GetProjects(
                        project => project.ProjectMemberships.Any(
                            membership => membership.DeveloperId == account.UserId))
                    .Count;

            return new DeveloperPageDeveloper(
                account.UserId,
                account.Firstname,
                account.Lastname,
                account.Profile?.Image?.SmallPhotoUri,
                role,
                account.RegistrationTime,
                projectCount,
                account.Profile?.VkProfileUri,
                account.Role,
                account.ConfirmationStatus,
                account.IsHidden);
        }

        public Developer ToDeveloper(Account account)
        {
            Require.NotNull(account, nameof(account));

            var userProjects = _projectProvider.GetProjects(
                project => project.ProjectMemberships.Any(
                    membership => membership.DeveloperId == account.UserId));
            var projectPreviews = GetDeveloperProjectPreviews(account, userProjects);

            return new Developer(
                account.UserId,
                account.Firstname,
                account.Lastname,
                account.Email.Address,
                account.ConfirmationStatus,
                account.IsOauthRegistered,
                account.Profile?.Image?.BigPhotoUri,
                account.RegistrationTime,
                account.Profile?.LinkToGithubProfile,
                account.Profile?.VkProfileUri,
                account.Profile?.PhoneNumber,
                account.Profile?.StudentAccessionYear,
                (bool) account.Profile?.IsGraduated,
                account.Profile?.StudyingDirection,
                account.Profile?.InstituteName,
                account.Profile?.Specialization,
                _userRoleAnalyzer.GetUserCommonRole(account.UserId),
                projectPreviews.ToArray());
        }

        public GuestDeveloper ToGuestDeveloper(Account account)
        {
            Require.NotNull(account, nameof(account));

            var projectList = _projectProvider.GetProjects(
                    project => project.ProjectMemberships.Any(
                        membership => membership.DeveloperId == account.UserId))
                .Where(ProjectsPolicies.OnlyDoneOrInProgress);
            var projectPreviews = GetDeveloperProjectPreviews(account, projectList.ToList());

            return new GuestDeveloper(
                account.UserId,
                account.Firstname,
                account.Lastname,
                account.Profile?.Image?.BigPhotoUri,
                account.RegistrationTime,
                account.Profile?.VkProfileUri,
                account.Profile?.LinkToGithubProfile,
                account.Profile?.StudentAccessionYear,
                (bool) account.Profile?.IsGraduated,
                account.Profile?.StudyingDirection,
                account.Profile?.InstituteName,
                account.Profile?.Specialization,
                _userRoleAnalyzer.GetUserCommonRole(account.UserId),
                projectPreviews.ToArray());
        }

        private DeveloperPageProjectPreview ToDeveloperPageProjectPreview(int userId, Project project)
        {
            var userMembership = project.ProjectMemberships.Single(membership => membership.DeveloperId == userId);
            return new DeveloperPageProjectPreview(
                project.ProjectId,
                project.LandingImage?.BigPhotoUri,
                project.Name,
                project.ProjectStatus,
                userMembership.Role);
        }

        private IEnumerable<DeveloperPageProjectPreview> GetDeveloperProjectPreviews(
            Account account,
            List<Project> userProjects)
        {
            var projectPreviews = userProjects.Select(
                project => ToDeveloperPageProjectPreview(account.UserId, project));
            return projectPreviews;
        }
    }
}