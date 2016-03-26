using System;
using System.Diagnostics;
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
            _databaseSessionProvider.OpenSession();
            Session.Save(email);
            _databaseSessionProvider.CloseSession();
        }

        public NotificationEmail PullNotificationEmail()
        {
            _databaseSessionProvider.OpenSession();
            var email = Session.Query<NotificationEmail>().FirstOrDefault();
            _databaseSessionProvider.CloseSession();
            return email;
        }

        public void RemoveNotificationEmail(NotificationEmail notificationMail)
        {
            Require.NotNull(notificationMail, nameof(notificationMail));
            _databaseSessionProvider.OpenSession();
            Session.Delete(notificationMail);
            _databaseSessionProvider.CloseSession();
        }

        public void ExecuteInNHibernateSession(Action action)
        {
            _databaseSessionProvider.ProcessInNHibernateSession(action);
        }

        private ISession Session => _databaseSessionProvider.GetCurrentSession();

        private readonly DatabaseSessionProvider _databaseSessionProvider;
    }
}