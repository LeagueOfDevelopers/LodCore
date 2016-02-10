using System;
using System.Linq;
using Journalist;
using NHibernate.Linq;
using UserPresentaton;

namespace DataAccess.Repositories
{
    public class NotificationSettingsRepository : INotificationSettingsRepository
    {
        private readonly DatabaseSessionProvider _sessionProvider;

        public NotificationSettingsRepository(DatabaseSessionProvider sessionProvider)
        {
            Require.NotNull(sessionProvider, nameof(sessionProvider));

            _sessionProvider = sessionProvider;
        }

        public NotificationSetting ReadNotificationSettingByCriteria(Func<NotificationSetting, bool> func)
        {
            var session = _sessionProvider.GetCurrentSession();
            return session.Query<NotificationSetting>().SingleOrDefault(func);
        }

        public void CreateNotificationSetting(NotificationSetting notificationSetting)
        {
            Require.NotNull(notificationSetting, nameof(notificationSetting));

            var session = _sessionProvider.GetCurrentSession();
            session.Save(notificationSetting);
        }

        public void UpdateNotificationSetting(NotificationSetting notificationSetting)
        {
            Require.NotNull(notificationSetting, nameof(notificationSetting));

            var session = _sessionProvider.GetCurrentSession();
            session.Update(notificationSetting);
        }
    }
}