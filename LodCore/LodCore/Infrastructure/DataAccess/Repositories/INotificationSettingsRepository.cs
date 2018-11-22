using LodCore.Domain.UserManagement;
using System;

namespace LodCore.Infrastructure.DataAccess.Repositories
{
    public interface INotificationSettingsRepository
    {
        NotificationSetting ReadNotificationSettingByCriteria(Func<NotificationSetting, bool> func);

        void CreateNotificationSetting(NotificationSetting notificationSetting);

        void UpdateNotificationSetting(NotificationSetting notificationSetting);
    }
}