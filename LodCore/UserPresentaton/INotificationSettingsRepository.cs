using System;

namespace UserPresentaton
{
    public interface INotificationSettingsRepository
    {
        NotificationSetting ReadNotificationSettingByCriteria(Func<NotificationSetting, bool> func);

        void CreateNotificationSetting(NotificationSetting notificationSetting);

        void UpdateNotificationSetting(NotificationSetting notificationSetting);
    }
}