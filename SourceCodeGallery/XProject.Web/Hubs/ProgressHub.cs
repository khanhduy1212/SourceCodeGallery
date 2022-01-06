using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace XProject.Web.Hubs
{
    public class ProgressHub : Hub
    {
        public static void SendMessage(string msg)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
            hubContext.Clients.All.sendMessage(msg);
        }
    }
}