using System;
using Journalist;
using LodCore.Infrastructure.DataAccess.Pagination;

namespace LodCoreApi.Models
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

        public virtual int Id { get; }

        public virtual DateTime OccuredOn { get; }

        public virtual string EventType { get; }

        public virtual string EventInfo { get; }

        public bool WasRead { get; }
    }
}