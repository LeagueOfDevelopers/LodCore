using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.WebSockets;
using Common;

namespace WebSocketConnection
{
    public class WebSocketStreamProvider : IWebSocketStreamProvider
    {
        public async Task ProcessWebSocketSession(AspNetWebSocketContext context)
        {
            await Task.Run(() =>
            {
                _userId = Convert.ToInt32(context.QueryString["id"]);
                _webSocket = context.WebSocket;
                while (true) ;
            });
        }

            public void SendMessage(string message)
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                var encoded = Encoding.UTF8.GetBytes(message);
                var segment = new ArraySegment<Byte>(encoded, 0, encoded.Length);
                _webSocket.SendAsync(segment, WebSocketMessageType.Text,
                    true, CancellationToken.None);
            }
        }

        public int GetCurrentUserId()
        {
            return _userId;
        }

        private WebSocket _webSocket;
        private int _userId;
    }
}
