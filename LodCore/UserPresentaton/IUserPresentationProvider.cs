namespace UserPresentaton
{
    public interface IUserPresentationProvider
    {
        NotificationSettings GetUserEventSettings(int userId);
    }
}