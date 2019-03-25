using System;
using LodCore.Domain.UserManagement;

namespace LodCore.Infrastructure.DataAccess.Repositories
{
    public class NotificationSettingsRepository : INotificationSettingsRepository
    {
        public NotificationSetting ReadNotificationSettingByCriteria(Func<NotificationSetting, bool> func)
        {
            /*
            var session = _sessionProvider.GetCurrentSession();
            return session.Query<NotificationSetting>().SingleOrDefault(func);*/
            return null;
        }

        public void CreateNotificationSetting(NotificationSetting notificationSetting)
        {
            /*
            Require.NotNull(notificationSetting, nameof(notificationSetting));

            var session = _sessionProvider.GetCurrentSession();
            session.Save(notificationSetting);*/
        }

        public void UpdateNotificationSetting(NotificationSetting notificationSetting)
        {
            /*
            Require.NotNull(notificationSetting, nameof(notificationSetting));

            var session = _sessionProvider.GetCurrentSession();
            session.Update(notificationSetting);*/
        }
    }
}