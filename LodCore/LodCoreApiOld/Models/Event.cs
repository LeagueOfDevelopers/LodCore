using System;
using Journalist;
using LodCoreLibraryOld.Infrastructure.DataAccess.Pagination;

namespace LodCoreApiOld.Models
{
    public class Event : IPaginable
    {
        public Event(int id, DateTime occuredOn, string eventType, string eventInfo, bool wasRead)
        {
            Require.Positive(id, nameof(id));
            Require.NotNull(occuredOn, nameof(occuredOn));
            Require.NotEmpty(eventType, nameof(eventType));
            Require.NotEmpty(eventInfo, nameof(eventInfo));

            Id = id;
            OccuredOn = occuredOn;
            EventType = eventType;
            EventInfo = eventInfo;
            WasRead = wasRead;
        }

        public virtual int Id { get; private set; }

        public virtual DateTime OccuredOn { get; private set; }

        public virtual string EventType { get; private set; }

        public virtual string EventInfo { get; private set; }

        public bool WasRead { get; private set; }
    }
}