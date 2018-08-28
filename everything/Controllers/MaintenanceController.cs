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

namespace everything.Controllers
{
    public class MaintenanceController : ApplicationBaseController
    {
        ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        // GET: Maintainance
        public ActionResult ReportBug()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReportBug([Bind(Include = "ReportBugId,Error,DateCreated,UserId")] ReportBug bug)
        {
            bug.UserId = User.Identity.GetUserId();
            bug.DateCreated = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _applicationDbContext.ReportBugs.Add(bug);
                await _applicationDbContext.SaveChangesAsync();

                return RedirectToAction("SuccessBug");
            }
            ModelState.AddModelError("", "Unable to add request");
            return View();
        }
        public ActionResult FeedBack()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FeedBack([Bind(Include = "FeedbackId,UserId,Message,DateCreated")] Feedback feedback)
        {
            feedback.UserId = User.Identity.GetUserId();
            feedback.DateCreated = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _applicationDbContext.Feedbacks.Add(feedback);
                await _applicationDbContext.SaveChangesAsync();

                return RedirectToAction("SuccessFeeback");
            }
            ModelState.AddModelError("", "Unable to add request");
            return View();
        }

        public ActionResult SuccessBug()
        {
            return View();
        }
        public ActionResult SuccessFeeback()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Privacy()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult OurTerms()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult LegalDisclaimer()
        {
            return View();
        }
    }
}