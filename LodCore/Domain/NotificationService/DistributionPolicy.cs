using System.Linq;
using Journalist;

namespace NotificationService
{
    public class DistributionPolicy
    {
        internal DistributionPolicy(int[] receiverIds)
        {
            Require.NotEmpty(receiverIds, nameof(receiverIds));

            ReceiverIds = receiverIds;
        }

        public DistributionPolicy Merge(DistributionPolicy policy)
        {
            var mergingIds = policy.ReceiverIds;
            var newIdsArr = ReceiverIds.Union(mergingIds).ToArray();
            return new DistributionPolicy(newIdsArr);
        }

        internal int[] ReceiverIds { get; private set; } 
    }
}