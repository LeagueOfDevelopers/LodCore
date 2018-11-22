using System.Linq;
using Journalist;
using LodCoreLibraryOld.Infrastructure.DataAccess.Repositories;
using LodCoreLibraryOld.Common;

namespace LodCoreLibraryOld.Domain.ProjectManagment
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
            Require.Positive(userId, nameof(userId));
            var allRoles = _repository.GetUserRoles(userId);
            if (!allRoles.Any())
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