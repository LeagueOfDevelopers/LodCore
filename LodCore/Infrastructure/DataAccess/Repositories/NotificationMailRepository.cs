using System.Linq;
using Journalist;
using Mailing;
using Mailing.AsyncMailing;
using NHibernate;
using NHibernate.Linq;

namespace DataAccess.Repositories
{
    public class NotificationMailRepository : INotificationMailRepository
    {
        public NotificationMailRepository(DatabaseSessionProvider databaseSessionProvider)
        {
            Require.NotNull(databaseSessionProvider, nameof(databaseSessionProvider));

            _databaseSessionProvider = databaseSessionProvider;
        }

        public void SaveNotificationEmail(NotificationEmail email)
        {
            Require.NotNull(email, nameof(email));
            Session.Save(email);
        }

        public NotificationEmail PullNotificationEmail()
        {
            return Session.Query<NotificationEmail>().FirstOrDefault();
        }

        public void RemoveNotificationEmail(NotificationEmail notificationMail)
        {
            Require.NotNull(notificationMail, nameof(notificationMail));
            Session.Delete(notificationMail);
        }

        private ISession Session => _databaseSessionProvider.GetCurrentSession();

        private readonly DatabaseSessionProvider _databaseSessionProvider;
    }
}