﻿using System;
using LodCore.Common;
using LodCore.Domain.NotificationService;
using LodCore.Infrastructure.WebSocketConnection;

namespace LodCore.Infrastructure.DataAccess.Repositories
{
    public class EventRepository : IEventRepository, INumberOfNotificationsProvider
    {
        private readonly IWebSocketStreamProvider _webSocketStreamProvider;

        public EventRepository(IWebSocketStreamProvider webSocketStreamProvider)
        {
            _webSocketStreamProvider = webSocketStreamProvider ??
                                       throw new ArgumentNullException(nameof(webSocketStreamProvider));
        }

        public void SaveEvent(Event @event, DistributionPolicy distributionPolicy)
        {
            /*
            var session = _sessionProvider.GetCurrentSession();

            var eventId = (int) session.Save(@event);

            foreach (var receiverId in distributionPolicy.ReceiverIds)
            {
                var id = session.Save(new Delivery(receiverId, eventId));
                SendNumberOfNotificationsViaWebSocket(receiverId);
            }*/
        }

        public Event[] GetEventsByUser(int userId, bool newOnly)
        {
            if (newOnly)
                return null;
            return null; // notificationHandler.Handle(new AllNotificationForDeveloperQuery(userId)); cast to Event
            /*
            var session = _sessionProvider.GetCurrentSession();
            var userDeliveries = session.Query<Delivery>().Where(delivery => delivery.UserId == userId);
            var eventIds = newOnly
                ? userDeliveries.Where(delivery => !delivery.WasRead).Select(delivery => delivery.EventId)
                : userDeliveries.Select(delivery => delivery.EventId);
            var events = session.Query<Event>().Where(@event => eventIds.Contains(@event.Id)).ToArray();
            return events;*/
            return null;
        }

        public Event[] GetSomeEvents(int userId, int projectsToSkip, int takeCount)
        {
            /*
            var session = _sessionProvider.GetCurrentSession();
            var userDeliveries =
                session.Query<Delivery>()
                    .Where(delivery => delivery.UserId == userId)
                    .OrderBy(delivery => delivery.WasRead)
                    .ThenByDescending(delivery => delivery.EventId)
                    .Skip(projectsToSkip)
                    .Take(takeCount)
                    .ToArray();
            var unreadEventIds = userDeliveries.Where(delivery => !delivery.WasRead).Select(delivery => delivery.EventId);
            var eventIds = userDeliveries.Select(delivery => delivery.EventId);
            var events =
                session.Query<Event>()
                    .Where(@event => eventIds.Contains(@event.Id))
                    .OrderBy(@event => unreadEventIds.Contains(@event.Id) ? 0 : 1)
                    .ThenByDescending(@event => @event.OccuredOn)
                    .ToArray();

            return events;*/
            return null;
        }

        public void MarkEventsAsRead(int userId, int[] eventIds)
        {
            /*
            var session = _sessionProvider.GetCurrentSession();
            var deliveries =
                session.Query<Delivery>().Where(delivery => eventIds.Contains(delivery.EventId) && delivery.UserId == userId).ToArray();
            foreach (var delivery in deliveries)
            {
                delivery.WasRead = true;
                session.Update(delivery);
            }
            SendNumberOfNotificationsViaWebSocket(userId);*/
        }

        public int GetCountOfUnreadEvents(int userId)
        {
            /*
            var session = _sessionProvider.GetCurrentSession();

            return session.Query<Delivery>().Count(delivery => delivery.UserId == userId && !delivery.WasRead);*/
            return 0;
        }

        public bool WasThisEventRead(int eventId, int userId)
        {
            /*
            var session = _sessionProvider.GetCurrentSession();
            var userDelivery =
                session.Query<Delivery>().Single(delivery => delivery.UserId == userId && delivery.EventId == eventId);

            return userDelivery.WasRead;*/
            return false;
        }

        public void SendNumberOfNotificationsViaWebSocket(int userId)
        {
            var countOfUnreadEvents = GetCountOfUnreadEvents(userId).ToString();
            _webSocketStreamProvider.SendMessage(userId, countOfUnreadEvents);
        }
    }
}