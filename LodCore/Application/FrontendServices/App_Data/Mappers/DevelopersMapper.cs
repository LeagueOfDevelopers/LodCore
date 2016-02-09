using System.Linq;
using FrontendServices.Models;
using Journalist;
using ProjectManagement.Application;
using UserManagement.Domain;

namespace FrontendServices.App_Data.Mappers
{
    public class DevelopersMapper
    {
        public DevelopersMapper(IUserRoleAnalyzer userRoleAnalyzer, IProjectProvider projectProvider)
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
                account.Profile.SmallPictureUri,
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
                account.Profile.SmallPictureUri,
                role,
                account.Profile.RegistrationTime,
                projectCount,
                account.Profile.VkProfileUri);
        }

        public Developer ToDeveloper(Account account)
        {
            Require.NotNull(account, nameof(account));

            return new Developer(
                account.UserId,
                account.Firstname,
                account.Lastname,
                account.Email.Address,
                account.RedmineUserId,
                account.GitlabUserId,
                account.ConfirmationStatus,
                account.Profile.BigPhotoUri,
                account.Profile.RegistrationTime,
                account.Profile.VkProfileUri,
                account.Profile.PhoneNumber,
                account.Profile.StudentAccessionYear,
                account.Profile.StudyingDirection,
                account.Profile.InstituteName,
                account.Profile.Specialization);
        }

        private readonly IUserRoleAnalyzer _userRoleAnalyzer;
        private readonly IProjectProvider _projectProvider;
    }
}