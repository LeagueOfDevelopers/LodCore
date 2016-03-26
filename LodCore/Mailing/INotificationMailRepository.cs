using System;
using Mailing.AsyncMailing;

namespace Mailing
{
    public interface INotificationMailRepository
    {
        void SaveNotificationEmail(NotificationEmail email);

        NotificationEmail PullNotificationEmail();

        void RemoveNotificationEmail(NotificationEmail notificationMail);

        void ExecuteInNHibernateSession(Action action);
    }
}