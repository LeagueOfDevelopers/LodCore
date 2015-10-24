using System;
using Journalist;
using Newtonsoft.Json;

namespace NotificationService
{
    public sealed class Event
    {
        public Event(IEventInfo eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));

            OccuredOn = DateTime.UtcNow;
            EventType = eventInfo.GetEventType();
            EventInfo = SerializeEventInfo(eventInfo);
        }

        public DateTime OccuredOn { get; private set; }

        public string EventType { get; private set; }

        public string EventInfo { get; private set; }

        private static string SerializeEventInfo(IEventInfo eventInfo)
        {
            return JsonConvert.SerializeObject((dynamic) eventInfo);
        }
    }
}