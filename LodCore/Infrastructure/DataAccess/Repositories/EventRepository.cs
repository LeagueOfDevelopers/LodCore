using NotificationService;

namespace DataAccess.Repositories
{
    public class EventRepository : IEventRepository
    {
        public void DistrubuteEvent(Event @event)
        {
            throw new System.NotImplementedException();
        }

        public Event[] GetEventsByUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public void MarkEventsAsRead(int[] eventIds)
        {
            throw new System.NotImplementedException();
        }
    }
}