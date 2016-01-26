namespace UserPresentaton
{
    public interface IUserPresentationProvider
    {
        NotificationSetting GetUserEventSettings(int userId, string eventType);
    }
}