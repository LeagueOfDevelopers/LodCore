namespace NotificationService
{
    public interface IEventRepository
    {
        void DistrubuteEvent(Event @event);

        Event[] GetEventsByUser(int userId);

        void MarkEventsAsRead(int[] eventIds);
    }
}