using System.Threading.Tasks;

namespace LodCore.Infrastructure.WebSocketConnection
{
    public interface IWebSocketStreamProvider
    {
        //Task ProcessWebSocketSession(AspNetWebSocketContext context);

        void SendMessage(int userId, string message);
    }
}
