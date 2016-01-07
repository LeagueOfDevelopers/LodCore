using System;
using System.Linq;
using Common;
using Journalist;
using Journalist.Extensions;
using ProjectManagement.Application;
using ProjectManagement.Infrastructure;

namespace ProjectManagement.Domain
{
    public class UserRoleAnalyzer : IUserRoleAnalyzer
    {
        public UserRoleAnalyzer(IProjectRepository repository, UserRoleAnalyzerSettings settings)
        {
            Require.NotNull(repository, nameof(repository));
            Require.NotNull(settings, nameof(settings));

            _repository = repository;
            _settings = settings;
        }

        public string GetUserCommonRole(int userId)
        {
            var allRoles = _repository
                .GetAllProjects()
                .SelectMany(project => project.ProjectMemberships)
                .Where(membership => membership.DeveloperId == userId)
                .Select(membership => membership.Role);
            if (allRoles.IsEmpty())
            {
                return _settings.DefaultRole;
            }

            var groupings = allRoles.GroupBy(
                role => role,
                new RelativeEqualityComparer(_settings.AppropriateEditDistance));
            var maxGrouping = groupings.OrderByDescending(grouping => grouping.Count()).First();
            if (maxGrouping.Count() > 1)
            {
                return maxGrouping.Key;
            }

            return allRoles.First();
        }
        
        private readonly IProjectRepository _repository;
        private readonly UserRoleAnalyzerSettings _settings;
    }
}