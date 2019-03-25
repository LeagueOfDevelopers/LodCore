using Journalist;
using LodCore.Domain.UserManagement;
using LodCore.Infrastructure.DataAccess.Repositories;
using LodCore.Infrastructure.Mailing;

namespace LodCore.Domain.NotificationService
{
    public class ContactsEventSink<T> : EventSinkBase<T> where T : IEventInfo
    {
        public ContactsEventSink(
            IDistributionPolicyFactory distributionPolicyFactory,
            IEventRepository eventRepository,
            IMailer mailer, IUserPresentationProvider userPresentationProvider) :
            base(distributionPolicyFactory, eventRepository, mailer, userPresentationProvider)
        {
        }

        public override void Consume(T eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));

            var distributionPolicy = DistributionPolicyFactory.GetAdminRelatedPolicy();

            EventRepository.SaveEvent(new Event(eventInfo), distributionPolicy);

            Mailer.SendNotificationEmail(distributionPolicy.ReceiverIds, eventInfo);
        }
    }
}