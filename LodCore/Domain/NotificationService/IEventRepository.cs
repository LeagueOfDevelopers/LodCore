namespace NotificationService
{
    public interface IEventRepository
    {
        void DistrubuteEvent(Event @event, DistributionPolicy distributionPolicy);

        Event[] GetEventsByUser(int userId);

        void MarkEventsAsRead(int[] eventIds);
    }
}