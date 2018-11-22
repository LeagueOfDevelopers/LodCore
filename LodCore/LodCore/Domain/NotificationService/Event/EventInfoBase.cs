namespace LodCore.Domain.NotificationService
{
    public abstract class EventInfoBase : IEventInfo
    {
        public virtual string GetEventType()
        {
            return GetType().Name;
        }
    }
}