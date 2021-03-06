﻿using LodCoreLibraryOld.Domain.NotificationService;

namespace LodCoreLibraryOld.Infrastructure.DataAccess.Repositories
{
    public interface IEventRepository
    {
        void SaveEvent(Event @event, DistributionPolicy distributionPolicy);

        Event[] GetEventsByUser(int userId, bool notReadOnly);

        Event[] GetSomeEvents(int userId, int projectsToSkip, int takeCount);

        void MarkEventsAsRead(int userId, int[] eventIds);

        int GetCountOfUnreadEvents(int userId);

        bool WasThisEventRead(int eventId, int userId);
    }
}