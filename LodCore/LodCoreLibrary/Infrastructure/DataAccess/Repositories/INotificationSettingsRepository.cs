using LodCoreLibrary.Domain.UserManagement;
using System;

namespace LodCoreLibrary.Infrastructure.DataAccess.Repositories
{
    public interface INotificationSettingsRepository
    {
        NotificationSetting ReadNotificationSettingByCriteria(Func<NotificationSetting, bool> func);

        void CreateNotificationSetting(NotificationSetting notificationSetting);

        void UpdateNotificationSetting(NotificationSetting notificationSetting);
    }
}