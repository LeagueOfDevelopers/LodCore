namespace NotificationService
{
    public interface IEventRepository
    {
        void SaveEvent(Event @event, DistributionPolicy distributionPolicy);

        Event[] GetEventsByUser(int userId, bool notReadOnly);

        Event[] GetSomeEvents(int userId, int projectsToSkip, int takeCount);

        void MarkEventsAsRead(int[] eventIds);

        int GetCountOfUnreadEvents(int userId);

        bool WasThisEventRead(int eventId, int userId);

        bool WasUpdated();
    }
}