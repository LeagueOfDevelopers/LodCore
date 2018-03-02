using System;
using System.Collections.Generic;
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
        static WebSocketStreamProvider()
        {
            _wsClients = new Dictionary<int, WebSocket>();
        }

        public async Task ProcessWebSocketSession(AspNetWebSocketContext context)
        {
            await Task.Run(() =>
            {
                var userId = Convert.ToInt32(context.QueryString["id"]);
                var socket = context.WebSocket;
                if (!_wsClients.ContainsKey(userId))
                    _wsClients.Add(userId, socket);
                while (true) ;
            });
        }

        public void SendMessage(int userId, string message)
        {
            if (_wsClients.ContainsKey(userId))
            {
                if (_wsClients[userId].State == WebSocketState.Open)
                {
                    var encoded = Encoding.UTF8.GetBytes(message);
                    var segment = new ArraySegment<Byte>(encoded, 0, encoded.Length);
                    _wsClients[userId].SendAsync(segment, WebSocketMessageType.Text,
                        true, CancellationToken.None);
                }
                else
                {
                    _wsClients.Remove(userId);
                }
            }
                
        }
        
        private static readonly Dictionary<int, WebSocket> _wsClients;
    }
}
