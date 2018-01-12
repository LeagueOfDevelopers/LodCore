namespace Common
{
    public interface IEventPublisher
    {
        void PublishEvent<T>(T @event) where T : EventInfoBase;
    }
}
