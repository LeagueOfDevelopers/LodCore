using System.Threading.Tasks;
using System.Web.WebSockets;

namespace Common
{
    public interface IWebSocketStreamProvider
    {
        Task ProcessWebSocketSession(AspNetWebSocketContext context);

        void SendMessage(string message);

        int GetCurrentUserId();
    }
}
