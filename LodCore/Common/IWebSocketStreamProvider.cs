using System.Threading.Tasks;
using System.Web.WebSockets;

namespace Common
{
    public interface IWebSocketStreamProvider
    {
        Task ProcessWebSocketSession(AspNetWebSocketContext context);

        void SendMessage(int userId, string message);
    }
}
