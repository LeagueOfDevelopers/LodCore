using System;
using Journalist;
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
            using ( var session = _sessionProvider.OpenSession())
            {
                return session.Query<NotificationSetting>().Single(func);
            }
        }

        public void CreateNotificationSetting(NotificationSetting notificationSetting)
        {
            Require.NotNull(notificationSetting, nameof(notificationSetting));

            using (var session = _sessionProvider.OpenSession())
            {
                var save = (int) session.Save(notificationSetting);
            }
        }

        public void UpdateNotificationSetting(NotificationSetting notificationSettingToUpdate,
            NotificationSetting newNotificationSetting)
        {
            throw new NotImplementedException();
        }
    }
}