using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using everything.DataLayer;
using everything.Models;
using everything.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using everything;
using everything.Areas.Rap.ViewModels;
using everything.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using PagedList;

namespace everything.Areas.Rap.Controllers
{
    [Authorize]
    public class AdMaintenanceController : ApplicationBaseController
    {
        ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        public AdMaintenanceController()
        {
        }

        public AdMaintenanceController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;

        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public List<string> canLoggedInUserView()
        {
            string currentUser = User.Identity.GetUserId();
            var Roles = UserManager.GetRolesAsync(currentUser);
            var roleLists = Roles.Result.ToList();
            if (roleLists.Count >= 1)
            {
                return roleLists.ToList();
            }
            else
                return null;

        }
        // GET: Rap/AdMaintenance
        public ActionResult Bug(int? page)
        {
            var rolesAssigneed = canLoggedInUserView();
            string roleCanView = "Customer Interface";

            if (rolesAssigneed != null)
            {
                var element = rolesAssigneed.Where(x => x.StartsWith(roleCanView)).FirstOrDefault();
                if (element != roleCanView)
                {
                    return RedirectToAction("Unauthorized", "Access");
                }
                else
                {
                    IQueryable<ReportBug> reportBugs = _applicationDbContext.ReportBugs
                        .OrderByDescending(d => d.DateCreated);

                    int pageSize = 50;
                    int pageNumber = page ?? 1;

                    return View(reportBugs.ToPagedList(pageNumber, pageSize));
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");


        }

        public ActionResult Feedback(int? page)
        {
            var rolesAssigneed = canLoggedInUserView();
            string roleCanView = "Customer Interface";

            if (rolesAssigneed != null)
            {
                var element = rolesAssigneed.Where(x => x.StartsWith(roleCanView)).FirstOrDefault();
                if (element != roleCanView)
                {
                    return RedirectToAction("Unauthorized", "Access");
                }
                else
                {
                    IQueryable<Feedback> feedbacks = _applicationDbContext.Feedbacks
                        .OrderByDescending(d => d.DateCreated);

                    int pageSize = 50;
                    int pageNumber = page ?? 1;

                    return View(feedbacks.ToPagedList(pageNumber, pageSize));
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");


        }

        #region Helpers

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _applicationDbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}