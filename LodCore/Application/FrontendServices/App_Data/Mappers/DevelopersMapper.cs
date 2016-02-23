using System.Linq;
using FrontendServices.Models;
using Journalist;
using ProjectManagement.Application;
using ProjectManagement.Domain;
using UserManagement.Domain;

namespace FrontendServices.App_Data.Mappers
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
                account.Profile.SmallPhotoUri,
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
                account.Profile.SmallPhotoUri,
                role,
                account.RegistrationTime,
                projectCount,
                account.Profile.VkProfileUri);
        }

        public Developer ToDeveloper(Account account)
        {
            Require.NotNull(account, nameof(account));

            var userProjects =
                _projectProvider.GetProjects(
                    project => project.ProjectMemberships.Any(
                        membership => membership.DeveloperId == account.UserId));

            var projectPreviews = userProjects.Select(
                project => ToDeveloperPageProjectPreview(account.UserId, project));

            return new Developer(
                account.UserId,
                account.Firstname,
                account.Lastname,
                account.Email.Address,
                account.RedmineUserId,
                account.GitlabUserId,
                account.ConfirmationStatus,
                account.Profile.BigPhotoUri,
                account.RegistrationTime,
                account.Profile.VkProfileUri,
                account.Profile.PhoneNumber,
                account.Profile.StudentAccessionYear,
                account.Profile.StudyingDirection,
                account.Profile.InstituteName,
                account.Profile.Specialization,
                _userRoleAnalyzer.GetUserCommonRole(account.UserId),
                projectPreviews.ToArray());
        }

        private DeveloperPageProjectPreview ToDeveloperPageProjectPreview(int userId, Project project)
        {
            var userMembership = project.ProjectMemberships.Single(membership => membership.DeveloperId == userId);
            return new DeveloperPageProjectPreview(
                project.ProjectId,
                project.LandingImageUri,
                project.Name,
                project.ProjectStatus,
                userMembership.Role);
        }
    }
}