using System.Linq;
using DataAccess.Entities;
using Journalist;
using NHibernate.Linq;
using NotificationService;

namespace DataAccess.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly DatabaseSessionProvider _sessionProvider;

        public EventRepository(DatabaseSessionProvider sessionProvider)
        {
            Require.NotNull(sessionProvider, nameof(sessionProvider));

            _sessionProvider = sessionProvider;
        }

        public void DistrubuteEvent(Event @event, DistributionPolicy distributionPolicy)
        {
            var session = _sessionProvider.GetCurrentSession();

            var eventId = (int) session.Save(@event);
            foreach (var receiverId in distributionPolicy.ReceiverIds)
            {
                var id = session.Save(new Delivery(receiverId, eventId));
            }
        }

        public Event[] GetEventsByUser(int userId, bool newOnly)
        {
            var session = _sessionProvider.GetCurrentSession();
            var userDeliveries = session.Query<Delivery>().Where(delivery => delivery.UserId == userId);
            var eventIds = newOnly
                ? userDeliveries.Where(delivery => !delivery.WasRead).Select(delivery => delivery.EventId)
                : userDeliveries.Select(delivery => delivery.EventId);
            var events = session.Query<Event>().Where(@event => eventIds.Contains(@event.Id)).ToArray();
            return events;
        }

        public void MarkEventsAsRead(int[] eventIds)
        {
            var session = _sessionProvider.GetCurrentSession();
            var deliveries =
                session.Query<Delivery>().Where(delivery => eventIds.Contains(delivery.EventId)).ToArray();
            foreach (var delivery in deliveries)
            {
                delivery.WasRead = true;
                session.Update(delivery);
            }
        }
    }
}