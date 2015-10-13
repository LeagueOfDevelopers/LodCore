using Journalist;
using NotificationService;

namespace UserManagement.Domain.Events
{
    public class NewFullConfirmedDeveloper : Event
    {
        public NewFullConfirmedDeveloper(int userId, DistributionPolicy distributionPolicy) : base(distributionPolicy)
        {
            Require.Positive(userId, nameof(userId));

            NewDeveloperId = userId;
        }

        public int NewDeveloperId { get; private set; }
    }
}