namespace LodCore.Domain.NotificationService
{
    public interface INotificationService
    {
        Event[] GetEventsForUser(int userId, int pageNumber);

        void MarkEventsAsRead(int userId, params int[] eventIds);

        int GetNumberOfUnreadEvents(int userId);

        bool WasEventRead(int eventId, int userId);
    }
}