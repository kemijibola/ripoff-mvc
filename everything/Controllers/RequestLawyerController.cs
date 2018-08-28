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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.IO;
using everything.ViewModels;
using everything.Helpers;
using System.Text.RegularExpressions;

namespace everything.Controllers
{
    public class RequestLawyerController : ApplicationBaseController
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        // GET: RequestLawyer
        public ActionResult Create(int page)
        {
            var UserId = User.Identity.GetUserId();
            var loggedUserByUserId = _applicationDbContext.Users.SingleOrDefault(i => i.Id == UserId);
            if (loggedUserByUserId.Career == "General")
            {

                ViewBag.ReportId = page;
                ViewBag.FirmRegion = _applicationDbContext.FirmRegions.ToList();
                return View();
            }
            else
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Success(int page)
        {
            var UserId = User.Identity.GetUserId();
            var loggedUserByUserId = _applicationDbContext.Users.SingleOrDefault(i => i.Id == UserId);
            if (loggedUserByUserId.Career == "General")
            {
                
                var getReportByReportId = _applicationDbContext.Reports.SingleOrDefault(rId => rId.ReportId == page);
                if(getReportByReportId != null)
                {
                    string sm_PageTitle = Regex.Replace(getReportByReportId.Title, "[^A-Za-z0-9]", "-");
                    ViewBag.suc = getReportByReportId.Title;
                    return View();
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "LawyerRequestId,UserId,FullName,FirmRegionId,ReportId,Email,PhoneNumber,AdditionalNote,AssignedToFirm,RequestDate")]LawyerRequest model)
        {
            ViewBag.FirmRegion = _applicationDbContext.FirmRegions.ToList();

            model.UserId = User.Identity.GetUserId();
            model.AssignedToFirm = false;
            model.RequestDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                _applicationDbContext.LawyerRequests.Add(model);
                await _applicationDbContext.SaveChangesAsync();

                var iD = Guid.NewGuid().ToString();
                var getReportByReportId = _applicationDbContext.Reports.SingleOrDefault(rId => rId.ReportId == model.ReportId);
                string sm_PageTitle = Regex.Replace(getReportByReportId.Title, "[^A-Za-z0-9]", "-");
                return RedirectToAction("Success", new { Controller = "RequestLawyer", action = "Success", title = sm_PageTitle, page = model.ReportId, id = iD });
            }

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