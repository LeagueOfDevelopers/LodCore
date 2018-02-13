using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Web.WebSockets;
using Newtonsoft.Json;

namespace FrontendServices.App_Data
{
    public class WebSocketProvider : WebSocketHandler
    {
        static WebSocketProvider()
        {
            _loggedUsers = new WebSocketCollection();
        }

        public override void OnOpen()
        {
            this.id = Convert.ToInt32(this.WebSocketContext.QueryString["id"]);
            _loggedUsers.Add(this);
            this.Send(JsonConvert.SerializeObject(10));
            base.OnOpen();
        }

        public override void OnClose()
        {
            _loggedUsers.Remove(this);
            base.OnClose();
        }

        public void SendMessage(Object message, IEnumerable<int> receiverIds)
        {
            var receivers = _loggedUsers.Where(user => receiverIds.Contains(((WebSocketProvider)user).id));
            foreach (var receiver in receivers)
                receiver.Send(JsonConvert.SerializeObject(message));
        }

        private static readonly WebSocketCollection _loggedUsers;
        private int id;
    }
}
