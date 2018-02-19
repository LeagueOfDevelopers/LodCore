using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Common;

namespace FrontendServices.Controllers
{
    public class WebSocketController : ApiController
    {
        public WebSocketController(IWebSocketStreamProvider webSocketStreamProvider, 
                                   INumberOfNotificationsProvider numberOfNotificationsProvider)
        {
            _webSocketStreamProvider = webSocketStreamProvider;
            _numberOfNotificationsProvider = numberOfNotificationsProvider;
        }
        
        [HttpGet]
        [Route("socket")]
        public async Task<HttpResponseMessage> OpenSocketConnection()
        {
            var currentContext = HttpContext.Current;
            return await Task.Run(() =>
            {
                if (currentContext.IsWebSocketRequest || currentContext.IsWebSocketRequestUpgrading)
                {
                    currentContext.AcceptWebSocketRequest(_webSocketStreamProvider.ProcessWebSocketSession);
                    return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            });
        }

        [HttpGet]
        [Route("socket/message")]
        public void SendFirstMessage()
        {
            _numberOfNotificationsProvider.SendNumberOfNotificationsViaWebSocket();
        }

        private readonly IWebSocketStreamProvider _webSocketStreamProvider;
        private readonly INumberOfNotificationsProvider _numberOfNotificationsProvider;
    }
}
