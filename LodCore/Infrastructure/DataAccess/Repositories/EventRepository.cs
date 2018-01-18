using System.Linq;
using Journalist;
using NHibernate.Criterion;
using NHibernate.Linq;
using NotificationService;
using Common;

namespace DataAccess.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly IDatabaseSessionProvider _sessionProvider;

        public EventRepository(IDatabaseSessionProvider sessionProvider)
        {
            Require.NotNull(sessionProvider, nameof(sessionProvider));

            _sessionProvider = sessionProvider;
        }

        public void SaveEvent(Event @event, DistributionPolicy distributionPolicy)
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

        public Event[] GetSomeEvents(int userId, int projectsToSkip, int takeCount)
        {
            var session = _sessionProvider.GetCurrentSession();
            var userDeliveries =
                session.Query<Delivery>()
                    .Where(delivery => delivery.UserId == userId)
                    .OrderBy(delivery => delivery.WasRead)
                    .Skip(projectsToSkip)
                    .Take(takeCount)
                    .ToArray();
            var unreadedEventIds = userDeliveries.Where(delivery => !delivery.WasRead).Select(delivery => delivery.EventId);
            var eventIds = userDeliveries.Select(delivery => delivery.EventId);
            var events =
                session.Query<Event>()
                    .Where(@event => eventIds.Contains(@event.Id))
                    .OrderBy(@event => unreadedEventIds.Contains(@event.Id) ? 0 : 1)
                    .ToArray();

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

        public int GetCountOfUnreadEvents(int userId)
        {
            var session = _sessionProvider.GetCurrentSession();

            return session.Query<Delivery>().Count(delivery => delivery.UserId == userId && !delivery.WasRead);
        }

        public bool WasThisEventRead(int eventId, int userId)
        {
            var session = _sessionProvider.GetCurrentSession();
            var userDelivery =
                session.Query<Delivery>().Single(delivery => delivery.UserId == userId && delivery.EventId == eventId);

            return userDelivery.WasRead;
        }
    }
}