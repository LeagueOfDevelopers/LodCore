using System;
using System.Linq;
using Journalist;
using NHibernate.Linq;
using LodCoreLibrary.Common;
using LodCoreLibrary.Domain.UserManagement;

namespace LodCoreLibrary.Infrastructure.DataAccess.Repositories
{
    public class NotificationSettingsRepository : INotificationSettingsRepository
    {
        private readonly IDatabaseSessionProvider _sessionProvider;

        public NotificationSettingsRepository(IDatabaseSessionProvider sessionProvider)
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