namespace NotificationService
{
    public interface IMailer
    {
        void ConfigureMailByEvent(Event @event);
    }
}