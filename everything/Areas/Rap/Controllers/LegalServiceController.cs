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
    public class LegalServiceController : ApplicationBaseController
    {
        ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        public LegalServiceController()
        {
        }

        public LegalServiceController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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
        
        public ActionResult Index(int? page)
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
                    LawyerRequestWithReportViewModel mainModel = new LawyerRequestWithReportViewModel();

                    var requests = _applicationDbContext.LawyerRequests.Where(s => s.AssignedToFirm == false)
                        .OrderByDescending(d => d.RequestDate).ToList();

                    var reqList = new List<LawyerRequestViewModel>();
                    foreach (var request in requests)
                    {
                        LawyerRequestViewModel reqModel = new LawyerRequestViewModel();
                        reqModel.Id = request.LawyerRequestId;
                        reqModel.RequestOwnerName = request.FullName;

                        var mainReport = _applicationDbContext.Reports.SingleOrDefault(id => id.ReportId == request.ReportId);
                        mainModel.ReportTitle = mainReport.Title;

                        reqList.Add(reqModel);
                    }

                    int pageSize = 50;
                    int pageNumber = page ?? 1;
                    mainModel.LawyerRequestViewModels = reqList.ToPagedList<LawyerRequestViewModel>(pageNumber, pageSize);

                    return View(mainModel);
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Access");
        }

        public ActionResult Assign(int page)
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
                    ViewBag.Lawfirms = _applicationDbContext.Lawfirms.ToList();
                    ViewBag.LawyerRequestId = page;
                    return View();
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Access");


        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Assign([Bind(Include = "ApprovedLawyerRequestId,LawyerRequestId,LawFirmId,DateApproved")]ApprovedLawyerRequest model)
        {
            ViewBag.Lawfirms = _applicationDbContext.Lawfirms.ToList();

            model.DateApproved = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                _applicationDbContext.ApprovedLawyerRequests.Add(model);
                await _applicationDbContext.SaveChangesAsync();


                var RequestToUpdate = _applicationDbContext.LawyerRequests
                        .Where(i => i.LawyerRequestId == model.LawyerRequestId)
                        .Single();
                if (TryUpdateModel(RequestToUpdate, "",
                     new string[] { "AssignedToFirm" }))
                {
                    try
                    {
                        RequestToUpdate.AssignedToFirm = true;
                        _applicationDbContext.Entry(RequestToUpdate).State = EntityState.Modified;
                        await _applicationDbContext.SaveChangesAsync();

                        return RedirectToAction("Index");
                    }
                    catch (Exception /* dex */)
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again.");
                    }
                }

            }
            ModelState.AddModelError("", "Unable to save changes. Try again.");
            return View(new { page = model.LawyerRequestId });
        }

        public ActionResult RejectRequest(int page)
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
                    ViewBag.RejectionReason = _applicationDbContext.LawyerRejectionReason.ToList();
                    ViewBag.LawyerRequestId = page;
                    return View();
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");


        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> RejectRequest([Bind(Include = "RejectedLawyerRequestId,LawyerRequestId,LawyerRejectionReasonId,DateRejected")]RejectedLawyerRequest model)
        {
            ViewBag.RejectionReason = _applicationDbContext.LawyerRejectionReason.ToList();

            model.DateRejected = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                _applicationDbContext.RejectedLawyerRequests.Add(model);
                await _applicationDbContext.SaveChangesAsync();

                var RequestToUpdate = _applicationDbContext.LawyerRequests
                .Where(i => i.LawyerRequestId == model.LawyerRequestId)
                .Single();
                if (TryUpdateModel(RequestToUpdate, "",
                     new string[] { "AssignedToFirm" }))
                {
                    try
                    {
                        RequestToUpdate.AssignedToFirm = true;
                        _applicationDbContext.Entry(RequestToUpdate).State = EntityState.Modified;
                        await _applicationDbContext.SaveChangesAsync();

                        return RedirectToAction("Index");
                    }
                    catch (Exception /* dex */)
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again.");
                    }
                }
            }
            ModelState.AddModelError("", "Unable to save changes. Try again.");
            return View(new { page = model.LawyerRequestId });
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