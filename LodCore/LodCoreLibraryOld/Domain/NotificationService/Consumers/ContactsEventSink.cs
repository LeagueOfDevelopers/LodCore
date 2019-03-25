using Journalist;
using LodCoreLibraryOld.Domain.UserManagement;
using LodCoreLibraryOld.Infrastructure.DataAccess.Repositories;
using LodCoreLibraryOld.Infrastructure.Mailing;

namespace LodCoreLibraryOld.Domain.NotificationService
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