namespace NotificationService
{
    public interface IEventRepository
    {
        void DistrubuteEvent(Event @event, DistributionPolicy distributionPolicy);

        Event[] GetEventsByUser(int userId, bool notReadOnly);

        void MarkEventsAsRead(int[] eventIds);
    }
}