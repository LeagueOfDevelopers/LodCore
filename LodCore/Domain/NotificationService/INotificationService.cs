namespace NotificationService
{
    public interface INotificationService
    {
        Event[] GetEventsForUser(int userId, int pageNumber);

        void MarkEventsAsRead(params int[] eventIds);

        int GetNumberOfUnreadEvents(int userId);
    }
}