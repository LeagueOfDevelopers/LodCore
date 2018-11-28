using System;
using System.Collections.Generic;
using System.Text;

namespace LodCore.QueryService.Views.NotificationView
{
    public class NotificationView
    {
        public int EventId { get; set; }
        public string EventType { get; set; }
        public DateTimeOffset OccuredOn { get; set; }
        public string EventInfo { get; set; }
        public bool WasRead { get; set; }
    }
}
