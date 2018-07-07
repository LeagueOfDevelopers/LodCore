using System.Linq;
using Journalist;
using LodCoreLibrary.Infrastructure.DataAccess.Repositories;
using LodCoreLibrary.Infrastructure.Mailing;
using LodCoreLibrary.Domain.UserManagement;

namespace LodCoreLibrary.Domain.NotificationService
{
    public abstract class EventSinkBase<T> : IEventConsumer<T> where T : IEventInfo
    {
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

        protected IDistributionPolicyFactory DistributionPolicyFactory { get; private set; }

        protected IEventRepository EventRepository { get; private set; }

        private readonly IUserPresentationProvider _userPresentationProvider;

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