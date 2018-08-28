using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using everything.Models;
using Microsoft.AspNet.SignalR.Hubs;

namespace everything.Hubs
{
    [HubName("recentComplaints")]
    public class RecentComplaintsHub : Hub
    {
        public static void NotifyRecentComplaintsToAllClients()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<RecentComplaintsHub>();

            context.Clients.All.updatedClients();
        }
    }
}