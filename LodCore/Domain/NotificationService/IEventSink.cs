namespace NotificationService
{
    public interface IEventSink
    {
        void ConsumeEvent(IEventInfo eventInfo);
    }
}