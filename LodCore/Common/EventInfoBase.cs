namespace Common
{
    public abstract class EventInfoBase : IEventInfo
    {
        public virtual string GetEventType()
        {
            return GetType().Name;
        }
    }
}