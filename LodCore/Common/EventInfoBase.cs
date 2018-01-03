namespace Common
{
    public abstract class EventInfoBase : IEventInfo
    {
        public string GetEventType()
        {
            return GetType().Name;
        }
    }
}