using LodCoreLibraryOld.Domain.NotificationService;
using Event = LodCoreApiOld.Models.Event;

namespace LodCoreApiOld.App_Data.Mappers
{
    public class EventMapper
    {
        private readonly INotificationService _notificationService;

        public EventMapper(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public Event ToEventPageEvent(LodCoreLibraryOld.Domain.NotificationService.Event @event, int userId)
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