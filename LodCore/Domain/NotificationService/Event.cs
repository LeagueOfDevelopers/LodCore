namespace NotificationService
{
    public class Event
    {
        public int EventId { get; set; }

        public EventType Type { get; set; }

        public string Content { get; set; }
    }
}