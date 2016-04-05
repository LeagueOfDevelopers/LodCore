namespace NotificationService
{
    public class NotificationService : INotificationService
    {
        public Event[] GetEventsForUser(int userId, int pageNumber = 0)
        {
            var projectsToSkip = pageNumber * _paginationSettings.PageSize;

            var takeCount = _paginationSettings.PageSize;

            return _eventRepository.GetSomeEvents(userId, projectsToSkip, takeCount);
        }

        public void MarkEventsAsRead(params int[] eventIds)
        {
            _eventRepository.MarkEventsAsRead(eventIds);
        }

        public int GetNumberOfUnreadEvents(int userId)
        {
            return _eventRepository.GetCountOfUnreadEvents(userId);
        }

        private readonly IEventRepository _eventRepository;

        private readonly PaginationSettings _paginationSettings;

        public NotificationService(IEventRepository eventRepository, PaginationSettings paginationSettings)
        {
            _eventRepository = eventRepository;
            _paginationSettings = paginationSettings;
        }
    }
}