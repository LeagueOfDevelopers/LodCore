using System.Collections.Generic;
using System.Linq;
using FrontendServices.App_Data.Authorization;
using FrontendServices.Models;
using Journalist;
using ProjectManagement.Application;
using ProjectManagement.Domain;
using UserManagement.Domain;
using Project = ProjectManagement.Domain.Project;

namespace FrontendServices.App_Data.Mappers
{
    public class DevelopersMapper
    {
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
                account.Profile.Image,
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
                account.Profile?.Image,
                role,
                account.RegistrationTime,
                projectCount,
                account.Profile?.VkProfileUri);
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
                account.RedmineUserId,
                account.GitlabUserId,
                account.ConfirmationStatus,
                account.Profile.Image,
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
        
        public GuestDeveloper ToGuestDeveloper(Account account)
        {
            Require.NotNull(account, nameof(account));

            var projectList = _projectProvider.GetProjects(
                project => project.ProjectMemberships.Any(
                    membership => membership.DeveloperId == account.UserId))
                    .Where(ProjectsPolicies.OnlyDoneOrInProgress)
                    .Where(ProjectsPolicies.OnlyPublic);
            var projectPreviews = GetDeveloperProjectPreviews(account, projectList.ToList());

            return new GuestDeveloper(
                account.UserId,
                account.Firstname,
                account.Lastname,
                account.Profile?.Image,
                account.RegistrationTime,
                account.Profile?.VkProfileUri,
                account.Profile?.StudentAccessionYear,
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
                project.LandingImage,
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

        private readonly IUserRoleAnalyzer _userRoleAnalyzer;
        private readonly IProjectProvider _projectProvider;
    }
}