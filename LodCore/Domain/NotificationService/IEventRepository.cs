namespace NotificationService
{
    public interface IEventRepository
    {
        void DistrubuteEvent(Event @event, DistributionPolicy distributionPolicy);

        Event[] GetEventsByUser(int userId, bool notReadOnly);

        Event[] GetSomeEvents(int userId, int projectsToSkip, int takeCount);

        void MarkEventsAsRead(int[] eventIds);

        int GetCountOfUnreadEvents(int userId);
    }
}