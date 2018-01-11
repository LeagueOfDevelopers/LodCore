namespace Common
{
    public interface IEventConsumer<T>
        where T : IEventInfo
    {
        void Consume(T @event);
    }
}
