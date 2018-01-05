namespace Common
{
    public interface IEventConsumer<in T>
    {
        void Consume(T @event);
    }
}
