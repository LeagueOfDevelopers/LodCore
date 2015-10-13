namespace NotificationService
{
    public interface INotificationService
    {
        Event[] GetEventsForUser(int userId);

        void MarkEventsAsRead(params int[] eventIds);
    }
}