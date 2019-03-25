using System.Linq;
using Journalist;
using LodCoreLibraryOld.Domain.UserManagement;
using LodCoreLibraryOld.Infrastructure.DataAccess.Repositories;
using LodCoreLibraryOld.Infrastructure.Mailing;

namespace LodCoreLibraryOld.Domain.NotificationService
{
    public abstract class EventSinkBase<T> : IEventConsumer<T> where T : IEventInfo
    {
        private readonly IUserPresentationProvider _userPresentationProvider;

        public EventSinkBase(IDistributionPolicyFactory distributionPolicyFactory,
            IEventRepository eventRepository,
            IMailer mailer,
            IUserPresentationProvider userPresentationProvider)
        {
            Require.NotNull(distributionPolicyFactory, nameof(distributionPolicyFactory));
            Require.NotNull(eventRepository, nameof(eventRepository));
            Require.NotNull(mailer, nameof(mailer));
            Require.NotNull(userPresentationProvider, nameof(userPresentationProvider));

            DistributionPolicyFactory = distributionPolicyFactory;
            EventRepository = eventRepository;
            Mailer = mailer;
            _userPresentationProvider = userPresentationProvider;
        }

        protected IMailer Mailer { get; }

        protected IDistributionPolicyFactory DistributionPolicyFactory { get; }

        protected IEventRepository EventRepository { get; }

        public abstract void Consume(T @event);

        protected void SendOutEmailsAboutEvent(int[] userIds, IEventInfo eventInfo)
        {
            var ids =
                userIds.Where(
                    id =>
                        _userPresentationProvider.GetUserEventSettings(id, eventInfo.GetEventType()) ==
                        NotificationSettingValue.SendNotificationAndMail).ToArray();

            Mailer.SendNotificationEmail(ids, eventInfo);
        }
    }
}