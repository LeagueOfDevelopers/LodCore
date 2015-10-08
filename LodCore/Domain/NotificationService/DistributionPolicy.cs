using Journalist;

namespace NotificationService
{
    internal class DistributionPolicy
    {
        internal DistributionPolicy(int[] receiverIds)
        {
            Require.NotEmpty(receiverIds, nameof(receiverIds));

            ReceiverIds = receiverIds;
        }
        internal int[] ReceiverIds { get; private set; } 
    }
}