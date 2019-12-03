using Microsoft.AspNetCore.SignalR;
using SmartNamePlate.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNamePlate.Web.Hubs
{
    public class SmartNamePlateHub : Hub
    {
        public async Task SendMessage(string client, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", client, message);
        }

        public override async Task OnConnectedAsync()
        {
            string cacheKey = string.Format("connection-{0}", Context.ConnectionId);
            MemCache.Instance.AddOrReplace("cacheKey", DateTime.Now.ToString());
            await Clients.All.SendAsync("ConnectionMessage", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string cacheKey = string.Format("connection-{0}", Context.ConnectionId);
            MemCache.Instance.Remove(cacheKey);
            await Clients.All.SendAsync("DisconnectionMessage", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
