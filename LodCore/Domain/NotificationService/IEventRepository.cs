using System;
using System.Collections.Generic;

namespace NotificationService
{
    public interface IEventRepository
    {
        void PutEvent(Event @event);

        List<Event> GetEvents(Func<Event, bool> predicate);
    }
}