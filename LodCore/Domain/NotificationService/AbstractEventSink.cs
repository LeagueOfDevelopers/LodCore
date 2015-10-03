using System;
using Journalist;

namespace NotificationService
{
    public abstract class AbstractEventSink
    {
        protected AbstractEventSink(IEventRepository eventRepository)
        {
            Require.NotNull(eventRepository, nameof(eventRepository));

            _eventRepository = eventRepository;
        }

        protected void PutEvent(Event @event)
        {
            throw new NotImplementedException();
        }

        private readonly IEventRepository _eventRepository;
    }
}