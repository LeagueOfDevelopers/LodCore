using System;
using Journalist;
using Newtonsoft.Json;

namespace LodCoreLibraryOld.Domain.NotificationService
{
    public class Event
    {
        public Event(IEventInfo eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));

            OccuredOn = DateTime.UtcNow;
            EventType = eventInfo.GetEventType();
            EventInfo = SerializeEventInfo(eventInfo);
        }

        protected Event()
        {
        }

        public virtual int Id { get; protected set; }

        public virtual DateTime OccuredOn { get; protected set; }

        public virtual string EventType { get; protected set; }

        public virtual string EventInfo { get; protected set; }

        private static string SerializeEventInfo(IEventInfo eventInfo)
        {
            return JsonConvert.SerializeObject((dynamic) eventInfo);
        }
    }
}