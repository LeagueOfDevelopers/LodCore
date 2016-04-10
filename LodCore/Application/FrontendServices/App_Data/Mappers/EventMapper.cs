using NotificationService;
using Event = FrontendServices.Models.Event;

namespace FrontendServices.App_Data.Mappers
{
    public class EventMapper
    {
        private readonly INotificationService _notificationService;

        public EventMapper(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public Event ToEventPageEvent(NotificationService.Event @event, int userId)
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