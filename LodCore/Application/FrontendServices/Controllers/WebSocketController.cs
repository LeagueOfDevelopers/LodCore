using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;
using FrontendServices.App_Data;

namespace FrontendServices.Controllers
{
    public class WebSocketController : ApiController
    {
        public WebSocketController(WebSocketProvider webSocketProvider)
        {
            _webSocketProvider = webSocketProvider;
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
                    currentContext.AcceptWebSocketRequest(ProcessWebSocketRequest);
                    return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            });
        }

        private async Task ProcessWebSocketRequest(AspNetWebSocketContext context)
        {
            var sessionCookie = context.Cookies["SessionId"];
            if (sessionCookie != null)
                await _webSocketProvider.ProcessWebSocketRequestAsync(context);
        }

        private readonly WebSocketProvider _webSocketProvider;
    }
}
