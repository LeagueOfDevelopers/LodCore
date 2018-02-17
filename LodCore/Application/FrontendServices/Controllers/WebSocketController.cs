using System;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;
using NotificationService;
using Common;
using System.Text;
using Serilog;

namespace FrontendServices.Controllers
{
    public class WebSocketController : ApiController
    {
        public WebSocketController(IEventRepository eventRepository, IDatabaseSessionProvider databaseSessionProvider)
        {
            _eventRepository = eventRepository;
            _databaseSessionProvider = databaseSessionProvider;
        }

        [HttpGet]
        [Route("socket")]
        public async Task<HttpResponseMessage> Messages()

        {
            var currentContext = HttpContext.Current;
            return await Task.Run(() =>
            {
                if (currentContext.IsWebSocketRequest || currentContext.IsWebSocketRequestUpgrading)
                {
                    currentContext.AcceptWebSocketRequest(ProcessWebSocketSession);
                    return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            });
        }

        private async Task ProcessWebSocketSession(AspNetWebSocketContext context)
        {
            var currentUserId = Convert.ToInt32(context.QueryString["id"]);
            await Task.Run(() =>
            {
                var ws = context.WebSocket;
                SendMessage(ws, currentUserId);
                while (ws.State == WebSocketState.Open)
                {
                    if (_eventRepository.WasUpdated())
                        SendMessage(ws, currentUserId);
                }
            });
        }

        private void SendMessage(WebSocket webSocket, int userId)
        {
            _databaseSessionProvider.OpenSession();
            var numberOfUnreadEvents = _eventRepository.GetCountOfUnreadEvents(userId);
            _databaseSessionProvider.CloseSession();
            var encoded = Encoding.UTF8.GetBytes(numberOfUnreadEvents.ToString());
            var segment = new ArraySegment<Byte>(encoded, 0, encoded.Length);
            webSocket.SendAsync(segment, WebSocketMessageType.Text,
                true, CancellationToken.None);
        }

        private readonly IDatabaseSessionProvider _databaseSessionProvider;
        private readonly IEventRepository _eventRepository;
    }
}
