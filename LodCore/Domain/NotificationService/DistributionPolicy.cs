using System.Linq;
using Journalist;

namespace NotificationService
{
    public class DistributionPolicy
    {
        public DistributionPolicy(int[] receiverIds)
        {
            Require.NotNull(receiverIds, nameof(receiverIds));

            ReceiverIds = receiverIds;
        }

        public int[] ReceiverIds { get; }

        public DistributionPolicy Merge(DistributionPolicy policy)
        {
            var mergingIds = policy.ReceiverIds;
            var newIdsArr = ReceiverIds.Union(mergingIds).ToArray();
            return new DistributionPolicy(newIdsArr);
        }
    }
}