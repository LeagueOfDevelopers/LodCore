using Common;
using Journalist;
using NotificationService;

namespace Mailing.AsyncMailing
{
    public class MailerAsyncProxy : IMailer
    {
        public MailerAsyncProxy(
            INotificationMailRepository notificationMailRepository,
            INotificationEmailDescriber notificationEmailDescriber)
        {
            Require.NotNull(notificationMailRepository, nameof(notificationMailRepository));
            Require.NotNull(notificationEmailDescriber, nameof(notificationEmailDescriber));

            _notificationMailRepository = notificationMailRepository;
            _notificationEmailDescriber = notificationEmailDescriber;
        }

        public void SendNotificationEmail(int[] userIds, IEventInfo eventInfo)
        {
            Require.NotEmpty(userIds, nameof(userIds));
            Require.NotNull(eventInfo, nameof(eventInfo));

            var description = _notificationEmailDescriber.Describe(eventInfo);
            var notificationMail = new NotificationEmail(userIds, description);
            _notificationMailRepository.SaveNotificationEmail(notificationMail);
        }

        private readonly INotificationMailRepository _notificationMailRepository;
        private readonly INotificationEmailDescriber _notificationEmailDescriber;
    }
}