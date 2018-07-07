namespace LodCoreLibrary.Common
{
    public interface INumberOfNotificationsProvider
    {
        void SendNumberOfNotificationsViaWebSocket(int userId);
    }
}
