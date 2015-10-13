using Journalist;
using NotificationService;

namespace UserManagement.Domain.Events
{
    public class NewEmailConfirmedDeveloper : Event
    {
        public NewEmailConfirmedDeveloper(int userId, DistributionPolicy distributionPolicy) : base(distributionPolicy)
        {
            Require.Positive(userId, nameof(userId));

            UserId = userId;
        }

        public int UserId { get; private set; }
    }
}