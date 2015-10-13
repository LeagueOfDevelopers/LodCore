using Journalist;
using NotificationService;

namespace UserManagement.Domain.Events
{
    public class UserManagementEventSink : EventSink
    {
        public UserManagementEventSink(IEventRepository repository, IDistributionPolicyFactory distributionPolicyFactory)
            : base(repository, distributionPolicyFactory)
        {
        }

        public void SendNewFullConfirmedDeveloperEvent(int userId)
        {
            Require.Positive(userId, nameof(userId));

            ConsumeEvent(new NewFullConfirmedDeveloper(userId, DistributionPolicyFactory.GetAllPolicy()));
        }

        public void SendNewEmailConfirmedDeveloperEvent(int userId)
        {
            Require.Positive(userId, nameof(userId));

            ConsumeEvent(new NewEmailConfirmedDeveloper(userId, DistributionPolicyFactory.GetAdminRelatedPolicy()));
        }
    }
}