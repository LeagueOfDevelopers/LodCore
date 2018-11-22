namespace LodCore.Common
{
    public interface INumberOfNotificationsProvider
    {
        void SendNumberOfNotificationsViaWebSocket(int userId);
    }
}
