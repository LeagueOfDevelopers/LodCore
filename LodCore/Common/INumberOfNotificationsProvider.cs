
namespace Common
{
    public interface INumberOfNotificationsProvider
    {
        void SendNumberOfNotificationsViaWebSocket(int userId);
    }
}
