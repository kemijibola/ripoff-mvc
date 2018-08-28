using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using everything.Models;
using everything.DataLayer;
using System.Collections.Generic;
using Facebook;
using System.Threading;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Routing;

namespace everything.Helpers
{
public class CustomAuthorizeAttribute : AuthorizeAttribute
{

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            
            // Returns HTTP 401 - see comment in HttpUnauthorizedResult.cs.
            var routeData = HttpContext.Current.Request.RequestContext.RouteData;
            var controller = routeData.GetRequiredString("controller");
            var action = routeData.GetRequiredString("action");
            var area = routeData.DataTokens["area"];
            if(area != null && area.ToString() == "Rap")
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                                {"action","Login" },
                                {"controller","Access" }
                     });
            }
            //if (area != null && area.ToString() == "Admin")
            //{
            //    filterContext.Result = new RedirectToRouteResult(
            //        new RouteValueDictionary
            //        {
            //                    {"area", "Admin" },
            //                    {"action","Login" },
            //                    {"controller","Secure" }
            //         });
            //}
        }

    }


}