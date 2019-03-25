namespace LodCoreLibraryOld.Common
{
    public interface INumberOfNotificationsProvider
    {
        void SendNumberOfNotificationsViaWebSocket(int userId);
    }
}