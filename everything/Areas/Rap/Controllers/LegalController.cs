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
using NReco.VideoConverter;
using PagedList;
using everything.Controllers;
using everything.Areas.Rap.ViewModels;

namespace everything.Areas.Rap.Controllers
{
    [Authorize]
    public class LegalController : ApplicationBaseController
    {
        ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        public LegalController()
        {
        }

        public LegalController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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

        public ActionResult Create(int page)
        {
            var rolesAssigneed = canLoggedInUserView();
            string roleCanView = "Legal Team";

            if (rolesAssigneed != null)
            {
                var element = rolesAssigneed.Where(x => x.StartsWith(roleCanView)).FirstOrDefault();
                if (element != roleCanView)
                {
                    return RedirectToAction("Unauthorized", "Access");
                }
                else
                {
                    HtmlToText convert = new HtmlToText();
                    var report = _applicationDbContext.Reports.SingleOrDefault(i => i.ReportId == page);
                    ViewBag.ReportText = convert.Convert(report.ReportText);
                    ViewBag.ReportId = page;
                    return View();
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");

        }

        public ActionResult CreateUpdate(int page,int rid)
        {
            var rolesAssigneed = canLoggedInUserView();
            string roleCanView = "Legal Team";

            if (rolesAssigneed != null)
            {
                var element = rolesAssigneed.Where(x => x.StartsWith(roleCanView)).FirstOrDefault();
                if (element != roleCanView)
                {
                    return RedirectToAction("Unauthorized", "Access");
                }
                else
                {
                    HtmlToText convert = new HtmlToText();
                    var report = _applicationDbContext.Reports.SingleOrDefault(i => i.ReportId == page);
                    var advice = _applicationDbContext.RipOffLegalTeamAdvices.SingleOrDefault(i => i.ReportId == page);
                    var rptUpdate = _applicationDbContext.ReportUpdates.SingleOrDefault(i => i.ReportId == page && i.ReportUpdateId == rid);
                    ViewBag.ReportText = convert.Convert(report.ReportText);
                    ViewBag.LegalAdvice = advice.LegalAdvice;
                    ViewBag.ReportId = page;
                    ViewBag.ReportUpdateId = rid;
                    ViewBag.Update = rptUpdate.Update;
                    return View();
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ReportId,UserId,LegalAdvice,DateCreated")] RipOffLegalTeamAdvice advice)
        {
            advice.UserId = User.Identity.GetUserId();
            advice.DateCreated = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                _applicationDbContext.RipOffLegalTeamAdvices.Add(advice);
                await _applicationDbContext.SaveChangesAsync();

                HtmlToText convert = new HtmlToText();
                var getReportOwnerByReportId = _applicationDbContext.Reports.SingleOrDefault(i => i.ReportId == advice.ReportId);
                string UserId = getReportOwnerByReportId.UserId;
                string id = Guid.NewGuid().ToString();

                string PageTitle = getReportOwnerByReportId.CompanyorIndividual + " : " + convert.Convert(getReportOwnerByReportId.ReportText).Substring(0, 50);
                string sm_PageTitle = Regex.Replace(PageTitle, "[^A-Za-z0-9]", "-");
                var getReportOwnerDisplayName = UserManager.FindByIdAsync(UserId);
                if (UserManager.EmailService != null)
                {

                    var callbackUrl = Url.Action(
                        "ReportDetails",
                        "Report",
                        new { title = sm_PageTitle, page = getReportOwnerByReportId.ReportId, iD = id, area = "" }, protocol: Request.Url.Scheme);

                    string Body =
                            "<p><h3>Rip-Off NG</h3></p>" +
                             "<p>Hi " + getReportOwnerDisplayName.Result.Email + ",</p>" +
                            "<p class=\"lead\">Our legal team has posted a legal advice to your complaint on https://www.ripoff.com.ng " +
                            "<p>----------------------------------------------------------------------------------------------------------------------------------------</p>";

                    await UserManager.SendEmailAsync(UserId, "Rip-Off Ng | Legal Team advice to your complaint about: " + getReportOwnerByReportId.Title, Body + " <a href=\"" + callbackUrl + "\">Click here to view</a>" + "<p></p><p>Do not reply to this email.</p><p>Regards,</p><p>Rip-Off NG Legal Team</p><p>Psst! Remember - this is not a marketing email.Since you have a Rip-Off NG Account,we want to keep you informed about operational updates or changes to our websites.</p>");
                }


                var ReportToUpdate = _applicationDbContext.Reports.SingleOrDefault(i => i.ReportId == advice.ReportId);

                if (TryUpdateModel(ReportToUpdate, "",new string[] { "AdviceStatus" }))
                {
                    try
                    {
                        ReportToUpdate.AdviceStatus = true;
                        _applicationDbContext.Entry(ReportToUpdate).State = EntityState.Modified;
                        await _applicationDbContext.SaveChangesAsync();

                        return RedirectToAction("Index");
                    }
                    catch (Exception /* dex */)
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again.");
                    }
                }

            }
            ModelState.AddModelError("", "Unable to add request");
            return View();
        }

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

                    IQueryable<Report> reports = _applicationDbContext.Reports.Where(a => a.AdviceStatus == false && a.Status == true)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateNews([Bind(Include = "BlogId,Title,Post,UserId,DateCreated")] Blog blog, HttpPostedFileBase[] images, HttpPostedFileBase[] videos)
        {
            try
            {
                blog.UserId = User.Identity.GetUserId();
                blog.DateCreated = DateTime.Now;
                blog.IsVisible = true;
                
                if (ModelState.IsValid)
                {
                    _applicationDbContext.Blogs.Add(blog);
                    await _applicationDbContext.SaveChangesAsync();

                    int newsId = 0;
                    newsId = blog.BlogId;
                    
                    
                   foreach (var img in images)
                   {
                        if (img != null)
                        {
                            string filename = DateTime.UtcNow.ToFileTimeUtc() + Path.GetExtension(img.FileName);

                            img.SaveAs(Path.Combine(Server.MapPath("~/ImageUpload"), filename));

                            var blogModel = new BlogImage();

                            blogModel.ImageName = filename;
                            blogModel.BlogId = newsId;
                            blogModel.ImageCaption = "Article image";
                            blogModel.DateCreated = DateTime.UtcNow;

                            _applicationDbContext.BlogImages.Add(blogModel);
                            await _applicationDbContext.SaveChangesAsync();
                        }
                        else { }

                    }
                    

                     foreach (var vid in videos)
                     {
                        if(vid != null)
                        {
                            string filename = DateTime.UtcNow.ToFileTimeUtc() + Path.GetExtension(vid.FileName);

                            vid.SaveAs(Path.Combine(Server.MapPath("~/VideoUpload"), filename));

                            var blogModel = new BlogVideo();

                            blogModel.VideoName = filename;
                            blogModel.BlogId = newsId;
                            blogModel.VideoCaption = "Article video";
                            blogModel.DateCreated = DateTime.UtcNow;

                            _applicationDbContext.BlogVideos.Add(blogModel);
                            await _applicationDbContext.SaveChangesAsync();
                        }
                        else { }

                    }

                    //foreach (var doc in documents)
                    //{
                    //    if (doc != null)
                    //    {
                    //        string filename = DateTime.UtcNow.ToFileTimeUtc() + Path.GetExtension(doc.FileName);

                    //        doc.SaveAs(Path.Combine(Server.MapPath("~/Documents"), filename));

                    //        var blogModel = new BlogImage();

                    //        blogModel.ImageName = filename;
                    //        blogModel.BlogId = newsId;
                    //        blogModel.ImageCaption = "Article video";
                    //        blogModel.DateCreated = DateTime.UtcNow;

                    //        _applicationDbContext.BlogImages.Add(blogModel);
                    //        await _applicationDbContext.SaveChangesAsync();
                    //    }
                    //    else { }

                    //}

                }

            }
            catch(Exception ex)
            {
                ViewBag.Message = "Could not publish article.Please try again later";
                return View(blog);
            }
            ViewBag.Message = "You have successfully published article";
            return View();
        }
        public ActionResult CreateNews()
        {
            var rolesAssigneed = canLoggedInUserView();
            string roleCanView = "Legal Team";

            if (rolesAssigneed != null)
            {
                var element = rolesAssigneed.Where(x => x.StartsWith(roleCanView)).FirstOrDefault();
                if (element != roleCanView)
                {
                    return RedirectToAction("Unauthorized", "Access");
                }
                else
                {

                    return View();

                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");
        }

        public ActionResult Update(int? page)
        {
            var rolesAssigneed = canLoggedInUserView();
            string roleCanView = "Legal Team";

            if (rolesAssigneed != null)
            {
                var element = rolesAssigneed.Where(x => x.StartsWith(roleCanView)).FirstOrDefault();
                if (element != roleCanView)
                {
                    return RedirectToAction("Unauthorized", "Access");
                }
                else
                {

                    IQueryable<ReportUpdate> updates = _applicationDbContext.ReportUpdates.Where(a => a.AdviseStatus == false)
                        .OrderByDescending(d => d.DateCreated);

                    int pageSize = 50;
                    int pageNumber = page ?? 1;

                    return View(updates.ToPagedList(pageNumber, pageSize));

                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUpdate([Bind(Include = "ReportUpdateId,UserId,AdviseText,DateCreated")] UpdateAdvice advice,int ReportId)
        {
            advice.UserId = User.Identity.GetUserId();
            advice.DateCreated = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                _applicationDbContext.UpdateAdvices.Add(advice);
                await _applicationDbContext.SaveChangesAsync();

                HtmlToText convert = new HtmlToText();
                var getReportOwnerByReportId = _applicationDbContext.Reports.SingleOrDefault(i => i.ReportId == ReportId);
                string UserId = getReportOwnerByReportId.UserId;
                string id = Guid.NewGuid().ToString();

                string PageTitle = getReportOwnerByReportId.CompanyorIndividual + " : " + convert.Convert(getReportOwnerByReportId.ReportText).Substring(0, 50);
                string sm_PageTitle = Regex.Replace(PageTitle, "[^A-Za-z0-9]", "-");
                var getReportOwnerDisplayName = UserManager.FindByIdAsync(UserId);
                if (UserManager.EmailService != null)
                {

                    var callbackUrl = Url.Action(
                        "ReportDetails",
                        "Report",
                        new { title = sm_PageTitle, page = getReportOwnerByReportId.ReportId, iD = id, area = "" }, protocol: Request.Url.Scheme);

                    string Body =
                            "<p><h3>Rip-Off NG</h3></p>" +
                             "<p>Hi " + getReportOwnerDisplayName.Result.Email + ",</p>" +
                            "<p class=\"lead\">Our legal team has posted a legal advice to your complaint update on https://www.ripoff.com.ng " +
                            "<p>----------------------------------------------------------------------------------------------------------------------------------------</p>";

                    await UserManager.SendEmailAsync(UserId, "Rip-Off Ng | Legal Team advice to your update about: " + getReportOwnerByReportId.Title, Body + " <a href=\"" + callbackUrl + "\">Click here to view</a>" + "<p></p><p>Do not reply to this email.</p><p>Regards,</p><p>Rip-Off NG Legal Team</p><p>Psst! Remember - this is not a marketing email.Since you have a Rip-Off NG Account,we want to keep you informed about operational updates or changes to our websites.</p>");
                }


                var ReportToUpdate = _applicationDbContext.ReportUpdates.SingleOrDefault(i => i.ReportUpdateId == advice.ReportUpdateId);

                if (TryUpdateModel(ReportToUpdate, "", new string[] { "AdviseStatus" }))
                {
                    try
                    {
                        ReportToUpdate.AdviseStatus = true;
                        _applicationDbContext.Entry(ReportToUpdate).State = EntityState.Modified;
                        await _applicationDbContext.SaveChangesAsync();

                        return RedirectToAction("Update");
                    }
                    catch (Exception /* dex */)
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again.");
                    }
                }

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