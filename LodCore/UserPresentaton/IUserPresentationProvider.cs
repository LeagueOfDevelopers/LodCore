namespace UserPresentaton
{
    public interface IUserPresentationProvider
    {
        NotificationSettingValue GetUserEventSettings(int userId, string eventType);

        void UpdateNotificationSetting(NotificationSetting notificationSetting);
    }
}