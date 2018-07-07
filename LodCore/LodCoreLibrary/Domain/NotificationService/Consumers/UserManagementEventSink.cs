using Journalist;
using LodCoreLibrary.Domain.UserManagement;
using LodCoreLibrary.Infrastructure.DataAccess.Repositories;
using LodCoreLibrary.Infrastructure.Mailing;

namespace LodCoreLibrary.Domain.NotificationService
{
    public class UserManagementEventSink<T> : EventSinkBase<T> where T : IEventInfo
    {
        public UserManagementEventSink(IDistributionPolicyFactory distributionPolicyFactory,
            IEventRepository eventRepository, 
            IMailer mailer, 
            IUserPresentationProvider userPresentationProvider)
            : base(distributionPolicyFactory, 
                  eventRepository, 
                  mailer, 
                  userPresentationProvider)
        {
        }

        public override void Consume(T eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));

            var distributionPolicy = GetDistributionPolicyForEvent((dynamic)eventInfo);

            EventRepository.SaveEvent(new Event(eventInfo), distributionPolicy);

            SendOutEmailsAboutEvent(distributionPolicy.ReceiverIds, eventInfo);
        }

        private DistributionPolicy GetDistributionPolicyForEvent(NewEmailConfirmedDeveloper eventInfo)
        {
            return DistributionPolicyFactory.GetAdminRelatedPolicy();
        }

        private DistributionPolicy GetDistributionPolicyForEvent(NewFullConfirmedDeveloper eventInfo)
        {
            return DistributionPolicyFactory.GetAllPolicy();
        }
    }
}