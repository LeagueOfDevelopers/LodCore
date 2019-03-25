using System;

namespace LodCore.QueryService.Views.NotificationView
{
    public class NotificationView
    {
        public int EventId { get; set; }
        public string EventType { get; set; }
        public DateTime OccuredOn { get; set; }
        public string EventInfo { get; set; }
        public bool WasRead { get; set; }
    }
}