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
    public class ReportRejectionController : ApplicationBaseController
    {
        ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        public ReportRejectionController()
        {
        }

        public ReportRejectionController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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

        // GET: Rap/ReportRejection
        public ActionResult Index()
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
                    IQueryable<RejectionReason> reportRejection = _applicationDbContext.RejectionReasons.OrderBy(n => n.Reason);
                    return View(reportRejection);
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Reason")] RejectionReason report)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.RejectionReasons.Add(report);
                await _applicationDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Unable to add request");
            return View();
        }

        public async Task<ActionResult> Edit(int? id)
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
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    RejectionReason reason = await _applicationDbContext.RejectionReasons.FindAsync(id);
                    if (reason == null)
                    {
                        return HttpNotFound();
                    }
                    return View(reason);
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RejectionReasonId,Reason")] RejectionReason report)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Entry(report).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(report);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(RejectionReason id)
        {
            RejectionReason reason = await _applicationDbContext.RejectionReasons.FindAsync(id.RejectionReasonId);
            _applicationDbContext.RejectionReasons.Remove(reason);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
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