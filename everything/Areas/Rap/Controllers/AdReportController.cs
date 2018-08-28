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
    public class AdReportController : ApplicationBaseController
    {
        ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        public AdReportController()
        {
        }

        public AdReportController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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
        // GET: Rap/AdReport
        public ActionResult Index(int? page, string search)
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
                    ViewBag.CurrentSearch = search;

                    IQueryable<Report> reports = _applicationDbContext.Reports.Where(s => s.Status == false && s.RejectionStatus == false)
                        .OrderByDescending(d => d.DateCreated);

                    if (!String.IsNullOrEmpty(search))
                        reports =
                            reports.Where(
                                r => r.Title.Contains(search) || r.ReportText.Contains(search));

                    int pageSize = 50;
                    int pageNumber = page ?? 1;

                    return View(reports.ToPagedList(pageNumber, pageSize));
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");
        }


        public async Task<ActionResult> Approve(int? page)
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
                    if (page == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Report report = await _applicationDbContext.Reports.FindAsync(page);
                    if (report == null)
                    {
                        return HttpNotFound();
                    }
                    return View(report);
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Access");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Approve(int ReportId)
        {
            var reportToApprove = _applicationDbContext.Reports.Find(ReportId);
            if (reportToApprove != null)
            {
                reportToApprove.Status = true;
                _applicationDbContext.Entry(reportToApprove).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult RejectReport(int? page)
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
                    ViewBag.ReportId = page;
                    ViewBag.ReportRejection = _applicationDbContext.RejectionReasons.OrderBy(n => n.Reason);
                    return View();
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RejectReport(ReportRejection model)
        {
            model.DateCreated = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _applicationDbContext.ReportRejections.Add(model);
                await _applicationDbContext.SaveChangesAsync();

                var ReportToUpdate = _applicationDbContext.Reports.SingleOrDefault(id => id.ReportId == model.ReportId);
                ReportToUpdate.RejectionStatus = true;

                _applicationDbContext.Entry(ReportToUpdate).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Unable to add request");
            return View();
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