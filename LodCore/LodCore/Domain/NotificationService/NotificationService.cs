using Journalist;
using LodCore.Common;
using LodCore.Infrastructure.DataAccess.Repositories;

namespace LodCore.Domain.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly IEventRepository _eventRepository;

        private readonly PaginationSettings _notificationsPaginationSettings;

        public NotificationService(IEventRepository eventRepository,
            PaginationSettings notificationsPaginationSettings)
        {
            _eventRepository = eventRepository;
            _notificationsPaginationSettings = notificationsPaginationSettings;
        }

        public Event[] GetEventsForUser(int userId, int pageNumber = 0)
        {
            var projectsToSkip = pageNumber * _notificationsPaginationSettings.PageSize;

            var takeCount = _notificationsPaginationSettings.PageSize;

            return _eventRepository.GetSomeEvents(userId, projectsToSkip, takeCount);
        }

        public void MarkEventsAsRead(int userId, params int[] eventIds)
        {
            _eventRepository.MarkEventsAsRead(userId, eventIds);
        }

        public int GetNumberOfUnreadEvents(int userId)
        {
            Require.Positive(userId, nameof(userId));

            return _eventRepository.GetCountOfUnreadEvents(userId);
        }

        public bool WasEventRead(int eventId, int userId)
        {
            return _eventRepository.WasThisEventRead(eventId, userId);
        }
    }
}