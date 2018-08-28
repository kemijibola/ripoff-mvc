using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using everything.Models;
using Microsoft.AspNet.SignalR.Hubs;

namespace everything.Hubs
{
    [HubName("recentQuestions")]
    public class RecentQuestionsHub : Hub
    {
        public static void NotifyRecentComplaintsToAllClients()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<RecentQuestionsHub>();

            context.Clients.All.updatedClients();
        }
    }
}