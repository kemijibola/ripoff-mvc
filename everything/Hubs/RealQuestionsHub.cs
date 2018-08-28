using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using everything.Models;
using Microsoft.AspNet.SignalR.Hubs;

namespace everything.Hubs
{
    [HubName("realQuestionsHub")]
    public class RealQuestionsHub : Hub
    {
        public static void NotifyRecentQuestionToAllClients()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<RealQuestionsHub>();

            context.Clients.All.updatedClients();
        }
    }
}