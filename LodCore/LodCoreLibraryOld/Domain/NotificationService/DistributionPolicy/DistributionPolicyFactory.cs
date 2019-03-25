using Journalist;
using Journalist.Collections;
using LodCoreLibraryOld.Infrastructure.DataAccess.Repositories;

namespace LodCoreLibraryOld.Domain.NotificationService
{
    public class DistributionPolicyFactory : IDistributionPolicyFactory
    {
        private readonly IProjectRelativesRepository _projectRelativesRepository;

        private readonly IUsersRepository _usersRepository;

        public DistributionPolicyFactory(
            IUsersRepository usersRepository,
            IProjectRelativesRepository projectRelativesRepository)
        {
            Require.NotNull(usersRepository, nameof(usersRepository));
            Require.NotNull(projectRelativesRepository, nameof(projectRelativesRepository));

            _usersRepository = usersRepository;
            _projectRelativesRepository = projectRelativesRepository;
        }

        public DistributionPolicy GetAllPolicy()
        {
            var receivers = _usersRepository.GetAllUsersIds();
            return new DistributionPolicy(receivers);
        }

        public DistributionPolicy GetProjectRelatedPolicy(int projectId)
        {
            var receivers = _projectRelativesRepository.GetAllProjectRelativeIds(projectId);
            return new DistributionPolicy(receivers);
        }

        public DistributionPolicy GetAdminRelatedPolicy()
        {
            var receivers = _usersRepository.GetAllAdminIds();
            return new DistributionPolicy(receivers);
        }

        public DistributionPolicy GetUserSpecifiedPolicy(params int[] userIds)
        {
            return new DistributionPolicy(userIds ?? EmptyArray.Get<int>());
        }
    }
}