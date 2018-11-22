namespace LodCoreLibraryOld.Domain.UserManagement
{
    public interface IUserPresentationProvider
    {
        NotificationSettingValue GetUserEventSettings(int userId, string eventType);

        void UpdateNotificationSetting(NotificationSetting notificationSetting);
    }
}