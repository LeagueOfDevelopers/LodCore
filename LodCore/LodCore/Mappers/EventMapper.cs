using LodCoreLibrary.Domain.NotificationService;
using Event = LodCore.Models.Event;

namespace LodCore.Mappers
{
    public class EventMapper
    {
        private readonly INotificationService _notificationService;

        public EventMapper(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public Event ToEventPageEvent(LodCoreLibrary.Domain.NotificationService.Event @event, int userId)
        {
            var wasRead = _notificationService.WasEventRead(@event.Id, userId);
            return new Event(
                @event.Id,
                @event.OccuredOn,
                @event.EventType,
                @event.EventInfo,
                wasRead);
        }
    }
}