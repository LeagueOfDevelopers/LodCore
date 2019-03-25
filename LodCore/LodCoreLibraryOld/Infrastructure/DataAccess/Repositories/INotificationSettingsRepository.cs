using System;
using LodCoreLibraryOld.Domain.UserManagement;

namespace LodCoreLibraryOld.Infrastructure.DataAccess.Repositories
{
    public interface INotificationSettingsRepository
    {
        NotificationSetting ReadNotificationSettingByCriteria(Func<NotificationSetting, bool> func);

        void CreateNotificationSetting(NotificationSetting notificationSetting);

        void UpdateNotificationSetting(NotificationSetting notificationSetting);
    }
}