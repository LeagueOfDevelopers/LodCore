using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FrontendServices.App_Data
{
    public class WebSocketConfig
    {
        public WebSocketConfig(WebSocketProvider webSocketProvider)
        {
            _webSocketProvider = webSocketProvider;
        }

        public void SetEventsToSendMessages()
        {

        }

        private readonly WebSocketProvider _webSocketProvider;
    }
}
