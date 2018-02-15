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
            var ws = context.WebSocket;
            var currentUserId = Convert.ToInt32(context.QueryString["id"]);
            new Task(() =>
            {
                SendMessage(ws, currentUserId);
                while (true)
                {
                    if (ws.State == WebSocketState.Open && _eventRepository.WasUpdated())
                        SendMessage(ws, currentUserId);
                }
            }).Start();
        }

        private async void SendMessage(WebSocket webSocket, int userId)
        {
            _databaseSessionProvider.OpenSession();
            var numberOfUnreadEvents = _eventRepository.GetCountOfUnreadEvents(userId);
            _databaseSessionProvider.CloseSession();
            byte[] dataToSend = { (byte)numberOfUnreadEvents };
            var segment = new ArraySegment<byte>(dataToSend);
            await webSocket.SendAsync(segment, WebSocketMessageType.Binary,
                true, CancellationToken.None);
        }

        private readonly IDatabaseSessionProvider _databaseSessionProvider;
        private readonly IEventRepository _eventRepository;
    }
}
