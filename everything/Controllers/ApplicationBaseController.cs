using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using everything.Models;
using everything.DataLayer;

namespace everything.Controllers
{
    public class ApplicationBaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)

        {
            
            if (User != null)
            {
                var context = new ApplicationDbContext();
                var username = User.Identity.Name;
                
                string displayImage = null;
                
                if (!string.IsNullOrEmpty(username))
                {
                    var user = context.Users.SingleOrDefault(u => u.UserName == username);
                    var userImage = context.UserProfilePhotos.SingleOrDefault(u => u.UserId == user.Id);
                    if(userImage == null)
                    {
                        displayImage = "person.gif";
                    }
                    else
                    {
                        displayImage = userImage.ImageName;
                    }
                    string displayName = user.NameExtension;
                    string FullName = user.FullName;
                    string career = user.Career;

                    //string displayName = string.Concat(new string[] { user.FirstName, " ", user.LastName });
                    ViewData.Add("Career", career);
                    ViewData.Add("DisplayName", displayName);
                    ViewData.Add("DisplayImage", displayImage);
                }
            }

            base.OnActionExecuted(filterContext);
        }
        public ApplicationBaseController()
        { }
    }
}