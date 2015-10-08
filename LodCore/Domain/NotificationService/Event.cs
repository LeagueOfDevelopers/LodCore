namespace NotificationService
{
    public abstract class Event
    {
        public virtual string EventType => GetType().ToString();
    }
}