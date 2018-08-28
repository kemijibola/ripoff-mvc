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
using System.Web.Helpers;
using everything.Hubs;

namespace everything.Controllers
{

    public class ReportController : ApplicationBaseController
    {
        public string titleC = null;
        public string iD = null;
        public ReportController()
        {
        }
        public ReportController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public int rId = 0;
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();

        [HttpGet]
        public ActionResult GetFileUploadForm(string selection,int page)
        {
            var UserId = User.Identity.GetUserId();
            var loggedUserByUserId = _applicationDbContext.Users.SingleOrDefault(i => i.Id == UserId);
            if (loggedUserByUserId.Career == "General")
            {
                var Id = page;
                if (selection == "Image")
                {
                    ViewBag.page = Id;
                    return PartialView("_Image");
                }
                else
                {
                    ViewBag.page = Id;
                    return PartialView("_Video");
                }
            }
            else
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("FileUpload");
            }
        }

        public ActionResult DisplayReplySection(int page,int randRid)
        {
            var UserId = User.Identity.GetUserId();
            var loggedUserByUserId = _applicationDbContext.Users.SingleOrDefault(i => i.Id == UserId);
            if (loggedUserByUserId.Career == "General")
            {
                CommentReplyViewModel replyModel = new CommentReplyViewModel();
                HtmlToText convert = new HtmlToText();
                var threadById = _applicationDbContext.Threads.Where(m => m.ThreadId == page).SingleOrDefault();


                var ReportByID = _applicationDbContext.Reports.Where(m => m.ReportId == randRid).SingleOrDefault();

                string rawPageTitle = convert.Convert(ReportByID.ReportText).Substring(0, 90);
                int indexP = rawPageTitle.LastIndexOf(' ');
                string titleOutput = rawPageTitle.Substring(0, indexP);

                string PageTitle = ReportByID.CompanyorIndividual + " : " + titleOutput;
                string sm_PageTitle = Regex.Replace(PageTitle, "[^A-Za-z0-9]", "-");

                //string myString = threadById.ThreadText;

                replyModel.ThreadId = threadById.ThreadId;
                replyModel.ThreadText = convert.Convert(threadById.ThreadText);
                replyModel.PageTitle = sm_PageTitle;
                replyModel.ReportId = ReportByID.ReportId;
                replyModel.RandomId = Guid.NewGuid().ToString();


                return PartialView("_ReplyModal", replyModel);
            }
            else
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult FileUpload(int page, string iD, ManageMessageId? message,string titleC)
        {
            var UserId = User.Identity.GetUserId();
            var loggedUserByUserId = _applicationDbContext.Users.SingleOrDefault(i => i.Id == UserId);
            if(loggedUserByUserId.Career == "General")
            {
                ViewBag.StatusMessage =
                message == ManageMessageId.AddReportSuccess ? "Your complaint has been published."
                : message == ManageMessageId.UploadImageSuccess ? "Your image has been uploaded."
                : message == ManageMessageId.UploadVideoSuccess ? "Your video has been uploaded."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";

                int reportId = page;
                ViewBag.page = reportId;
                return View();
            }
            else
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]    
        public async Task<ActionResult> UploadImage(FormCollection fc, IEnumerable<HttpPostedFileBase> files)
        {

                ReportImage reportImage = null;
                HtmlToText convert = new HtmlToText();
                ManageMessageId? message;
                try
                {

                    int reportId = Convert.ToInt32(fc["ReportId"]);
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    var userReport = _applicationDbContext.Reports.Where(m => m.ReportId == reportId).SingleOrDefault();
                    string loggedUserId = user.Id;
                    string userrptId = userReport.UserId;
                    string sm_titleC = userReport.CompanyorIndividual + " : " + convert.Convert(userReport.ReportText).Substring(0, 90);
                    titleC = Regex.Replace(sm_titleC, "[^A-Za-z0-9]", "-");
                    iD = Guid.NewGuid().ToString();
                    var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", ".jpeg", ".JPG",".GIF",".Gif",".gif" };


                    var isReportExist = _applicationDbContext.Reports.Where(m => m.ReportId == reportId).SingleOrDefault();

                    foreach (var file in files)
                    {
                        var ext = Path.GetExtension(file.FileName);
                        if (file != null && file.ContentLength > 0 && isReportExist != null && loggedUserId.Equals(userrptId) && allowedExtensions.Contains(ext))
                        {
                            string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);

                            file.SaveAs(Path.Combine(Server.MapPath("~/ImageUpload"), filename));

                            reportImage = new ReportImage();

                            reportImage.ImageName = filename;
                            reportImage.ReportId = Convert.ToInt32(fc["ReportId"]);
                            reportImage.ImageCaption = fc["imgCaption"].ToString();
                            reportImage.DateCreated = DateTime.UtcNow;

                            _applicationDbContext.ReportImages.Add(reportImage);
                            await _applicationDbContext.SaveChangesAsync();


                        }
                        else
                        {
                            message = ManageMessageId.Error;

                            return RedirectToAction("FileUpload", new { Controller = "Report", action = "FileUpload", title = titleC, page = Convert.ToInt32(fc["ReportId"]), id = iD, Message = message });
                        }
                    }
                    return RedirectToAction("ReportDetails", new { Controller = "Report", action = "ReportDetails", title = titleC, page = Convert.ToInt32(fc["ReportId"]), id = iD });
                }
                catch (Exception ex)
                {

                    message = ManageMessageId.Error;
                    return RedirectToAction("FileUpload", new { Controller = "Report", action = "FileUpload", title = titleC, page = Convert.ToInt32(fc["ReportId"]), id = iD, Message = message });
                }
            
            //return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadVideo(FormCollection fc, IEnumerable<HttpPostedFileBase> files)
        {
                ReportVideo reportVideo = null;
                ManageMessageId? message;
                try
                {
                    HtmlToText convert = new HtmlToText();

                    int reportId = Convert.ToInt32(fc["ReportId"]);
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    var userReport = _applicationDbContext.Reports.Where(m => m.ReportId == reportId).SingleOrDefault();
                    string loggedUserId = user.Id;
                    string userrptId = userReport.UserId;
                    string sm_titleC = userReport.CompanyorIndividual + " : " + convert.Convert(userReport.ReportText).Substring(0, 90);
                    titleC = Regex.Replace(sm_titleC, "[^A-Za-z0-9]", "-");
                    iD = Guid.NewGuid().ToString();
                    var allowedExtensions = new[] { ".mp4", ".MP4" };

                    var isReportExist = _applicationDbContext.Reports.Where(m => m.ReportId == reportId).SingleOrDefault();

                    foreach (var file in files)
                    {
                        var ext = Path.GetExtension(file.FileName);
                        if (file != null && file.ContentLength > 0 && isReportExist != null && loggedUserId.Equals(userrptId) && allowedExtensions.Contains(ext))
                        {
                            // string filename = System.IO.Path.GetFileName(file.FileName);
                            string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);

                            file.SaveAs(Path.Combine(Server.MapPath("~/VideoUpload"), filename));

                            reportVideo = new ReportVideo();

                            reportVideo.VideoName = filename;
                            reportVideo.ReportId = Convert.ToInt32(fc["ReportId"]);
                            reportVideo.VideoCaption = fc["vidCaption"].ToString();
                            reportVideo.DateCreated = DateTime.UtcNow;

                            _applicationDbContext.ReportVideos.Add(reportVideo);
                            await _applicationDbContext.SaveChangesAsync();

                            return RedirectToAction("ReportDetails", new { Controller = "Report", action = "ReportDetails", title = titleC, page = Convert.ToInt32(fc["ReportId"]), id = iD });

                        }
                        else
                        {
                            message = ManageMessageId.Error;
                            return RedirectToAction("FileUpload", new { Controller = "Report", action = "FileUpload", title = titleC, page = Convert.ToInt32(fc["ReportId"]), id = iD, Message = message });
                        }
                    }
                    return RedirectToAction("ReportDetails", new { Controller = "Report", action = "ReportDetails", title = titleC, page = Convert.ToInt32(fc["ReportId"]), id = iD });
                }
                catch (Exception ex)
                {
                    message = ManageMessageId.Error;
                    return RedirectToAction("FileUpload", new { Controller = "Report", action = "FileUpload", title = titleC, page = Convert.ToInt32(fc["ReportId"]), id = iD, Message = message });
                }
          
            //return View();
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]

        public async Task<ActionResult> Create([Bind(Include = "ReportId,UserId,CompanyorIndividual,Website,TopicId,CategoryId,ReportText,OnlineTransaction,CreditCard,Status,DateCreated,Address,CityId,StateId,ContactNumber,Email,Title,AdviceStatus")]Report report)
        {
            HtmlToText convert = new HtmlToText();
            ManageMessageId? message;
            int countryId = 160;
            ViewBag.State = _applicationDbContext.States.Where(c => c.CountryId == countryId).ToList();
            ViewBag.City = _applicationDbContext.Cities.ToList();
            ViewBag.Topic = _applicationDbContext.Topics.ToList();
            ViewBag.Category = _applicationDbContext.Categories.ToList();

            report.UserId = User.Identity.GetUserId();
            report.DateCreated = DateTime.UtcNow;
            report.Status = false;
            report.AdviceStatus = false;
            if (ModelState.IsValid)
            {
                _applicationDbContext.Reports.Add(report);
                await _applicationDbContext.SaveChangesAsync();

                int reportiD = 0;
                reportiD = report.ReportId;

                rId = reportiD;
                string sm_titleC = report.CompanyorIndividual + " : " + convert.Convert(report.ReportText).Substring(0, 90);
                titleC = Regex.Replace(sm_titleC, "[^A-Za-z0-9]", "-");

                string iD = Guid.NewGuid().ToString();

                //page is used instead of ReportId
                //Once the record is inserted , then notify all the subscribers (Clients)
                RecentComplaintsHub.NotifyRecentComplaintsToAllClients();

                var AdminUsers = _applicationDbContext.Users.Where(userType => userType.Career == "Admin").ToList();
                foreach(var user in AdminUsers)
                {
                    var getReportOwnerDisplayName = UserManager.FindByIdAsync(report.UserId);
                    if (UserManager.EmailService != null)
                    {
                        string id = Guid.NewGuid().ToString();


                        var callbackUrl = Url.Action(
                            "ReportDetails",
                            "Report",
                            new { title = titleC, page = report.ReportId, iD = id }, protocol: Request.Url.Scheme);

                        string Body =
                                "<p><h3>Rip-Off NG</h3></p>" +
                                 "<p>Hi " + user.NameExtension + ",</p>" +
                                "<p class=\"lead\">Someone posted a complaint on https://www.ripoff.com.ng " +
                                "<p>----------------------------------------------------------------------------------------------------------------------------------------</p>";

                        await UserManager.SendEmailAsync(user.Id, "Rip-Off Ng | There's a complaint about: " + report.Title, Body + " <a href=\"" + callbackUrl + "\">Click here to view</a>" + "<p></p><p>Do not reply to this email.</p><p>Regards,</p><p>Rip-Off NG Admin</p><p>Psst! Remember - this is not a marketing email.Since you have a Rip-Off NG Account,we want to keep you informed about operational updates or changes to our websites.</p>");
                    }
                }
                return RedirectToAction("FileUpload" , new { Controller = "Report", action = "FileUpload", title = titleC, page = reportiD, id = iD});

            }
            
            message = ManageMessageId.Error;
            return View(message);
        }

        [AllowAnonymous]
        public ActionResult ReportDetails(int page,string iD)
        {
            FullReportViewModel mainModel = new FullReportViewModel();
            HtmlToText convert = new HtmlToText();
            string LoggedInUser = User.Identity.GetUserId();
             

            var SingleReport = _applicationDbContext.Reports
                .Where(i => i.ReportId == page)
                .Include(t => t.Topic).Include(c => c.Category).Include(r => r.ReportImages)
                .Include(reb => reb.Rebuttals).Include(th => th.Threads)
                .Include(upd => upd.ReportUpdates).Include(st => st.State).Include(ci => ci.City)
                .Include(vi => vi.ReportVideos)/*.Include(l =>l.RipOffLegalTeams)*/
                .SingleOrDefault();

            var rebuttalList = new List<RebuttalViewModel>();
            var threadList = new List<ThreadViewModel>();
            var rptUpdateList = new List<ReportUpdateViewModel>();
            var caseUpdateList = new List<RipOffCaseUpdateViewModel>();
            var rptImageList = new List<ReportImageViewModel>();
            var rptVideoList = new List<ReportVideoViewModel>();
            var commentList = new List<CommentViewModel>();
            var legalList = new List<RipOffLegalTeamViewModel>();
            var updateLegalList = new List<LegalAdviceViewModel>();
            var relatedCompanyReport = new List<RelatedReportedCompanyViewModel>();


            if (SingleReport != null)
            {

                var User = UserManager.FindByIdAsync(SingleReport.UserId);
                var ReportOwner = User.Result.NameExtension;

                mainModel.ReportId = SingleReport.ReportId;
                mainModel.DisplayName = ReportOwner;
                mainModel.CompanyorIndividual = SingleReport.CompanyorIndividual;
                mainModel.Website = SingleReport.Website;
                mainModel.Email = SingleReport.Email;
                mainModel.CategoryName = SingleReport.Category.Name;
                mainModel.TopicName = SingleReport.Topic.Name;
                mainModel.ContactNumber = SingleReport.ContactNumber;
                mainModel.Address = SingleReport.Address;
                mainModel.Title = SingleReport.Title;
                if(SingleReport.StateId != null || SingleReport.CityId != null)
                {
                    mainModel.CityName = SingleReport.City.Name;
                    mainModel.StateName = SingleReport.State.Name;
                }
                else
                {
                    //Do nothing
                }
                //HtmlToText convert = new HtmlToText();

                mainModel.ReportText = SingleReport.ReportText;
                mainModel.OnlineTransaction = SingleReport.OnlineTransaction;
                mainModel.CreditCard = SingleReport.CreditCard;
                mainModel.ReportDateCreated = SingleReport.DateCreated.ToString("d,MMMM yy");
                mainModel.ReportTimeCreated = SingleReport.DateCreated.ToString("H:mm tt");


                var rebuttals = _applicationDbContext.Rebuttals.Where(r => r.ReportId == SingleReport.ReportId)
                    .Include(c => c.City).Include(s => s.State)
                    .OrderByDescending(d => d.DateCreated);


                foreach (var reb in rebuttals)
                {
                    var rebModel = new RebuttalViewModel();
                    var RebUser = UserManager.FindByIdAsync(reb.UserId);
                    var RebOwner = RebUser.Result.NameExtension;

                    rebModel.RebuttalDisplayName = RebOwner;
                    rebModel.RebuttalTitle = reb.Title;
                    rebModel.RebuttalAddress = reb.Address;
                    rebModel.RebuttalText = reb.RebuttalText;

                    if (reb.StateId != null || reb.CityId != null)
                    {
                        rebModel.RebuttalCity = reb.City.Name;
                        rebModel.RebuttalState = reb.State.Name;
                    }

                    rebModel.RebuttalDateCreated = reb.DateCreated.ToString("d,MMMM yy");
                    rebModel.RebuttalTimeCreated = reb.DateCreated.ToString("H:mm tt");

                    rebuttalList.Add(rebModel);
                }
                if (LoggedInUser == null)
                {
                    LoggedInUser = "Null";
                }
                else
                {
                    if (LoggedInUser.StartsWith(SingleReport.UserId))
                    {
                        mainModel.RebuttalAccess = true;
                    }
                }

                var thread = _applicationDbContext.Threads.Where(r => r.ReportId == SingleReport.ReportId)
                    .Include(com => com.Comments)
                    .OrderBy(d => d.DateCreated).ToList();
                
                foreach (var th in thread)
                {
                    var CommenterProfile = _applicationDbContext.UserProfilePhotos.SingleOrDefault(id => id.UserId == th.UserId);
                    var threadModel = new ThreadViewModel();
                    var threadUser = UserManager.FindByIdAsync(th.UserId);
                    var threadOwner = threadUser.Result.NameExtension;
                    
                    if (CommenterProfile == null)
                    {
                        threadModel.ThreadImageName = "person.gif";
                    }
                    else
                    {
                        threadModel.ThreadImageName = CommenterProfile.ImageName;
                    }

                    threadModel.ThreadDisplayName = threadOwner;
                    threadModel.ThreadId = th.ThreadId;
                    threadModel.ThreadText = th.ThreadText;
                    threadModel.ThreadDateCreated = th.DateCreated.ToString("d,MMMM yy");
                    threadModel.ThreadTimeCreated = th.DateCreated.ToString("H:mm tt");

             

                    foreach (Comment com in th.Comments.OrderBy(d => d.DateCreated))
                    {
                        var commentModel = new CommentViewModel();
                        var ReplyDisplayImage = _applicationDbContext.UserProfilePhotos.SingleOrDefault(id => id.UserId == com.UserId);
                        if (ReplyDisplayImage == null)
                        {
                            commentModel.CommentImageName = "person.gif";
                        }
                        else
                        {
                            commentModel.CommentImageName = ReplyDisplayImage.ImageName;
                        }

                        commentModel.ThreadId = com.ThreadId;
                        commentModel.CommentText = com.CommentText;
                        commentModel.CommentDateCreated = com.DateCreated.ToString("d,MMMM yy");
                        commentModel.CommentTimeCreated = com.DateCreated.ToString("H:mm tt");

                        var commentUser = UserManager.FindByIdAsync(com.UserId);
                        var commentOwner = commentUser.Result.NameExtension;
                        commentModel.CommentDisplayName = commentOwner;

                        commentList.Add(commentModel);

                    }

                    threadList.Add(threadModel);
                }

                IQueryable<ReportUpdate> update = _applicationDbContext.ReportUpdates.Where(r => r.ReportId == SingleReport.ReportId)
                    .Include(upd => upd.UpdateAdvices)
                    .OrderByDescending(d => d.DateCreated);
                foreach (var r in update)
                {
                    var rptUpdateModel = new ReportUpdateViewModel();

                    rptUpdateModel.ReportUpdateId = r.ReportUpdateId;
                    rptUpdateModel.ReportUpdateDisplayName = ReportOwner;
                    rptUpdateModel.ReportUpdateText = r.Update;
                    rptUpdateModel.ReportUpdateDateCreated = r.DateCreated.ToString("d,MMMM yy");
                    rptUpdateModel.ReportUpdateTimeCreated = r.DateCreated.ToString("H:mm tt");

                    foreach (UpdateAdvice upd in r.UpdateAdvices.OrderBy(d => d.DateCreated))
                    {
                        var updateLegalModel = new LegalAdviceViewModel();

                        updateLegalModel.UpdateAdvice = upd.AdviseText;
                        updateLegalModel.ReportUpdateId = upd.ReportUpdateId;

                        updateLegalList.Add(updateLegalModel);

                    }

                    rptUpdateList.Add(rptUpdateModel);
                }

                IQueryable<ReportImage> image = _applicationDbContext.ReportImages.Where(r => r.ReportId == SingleReport.ReportId)
                    .OrderByDescending(d => d.DateCreated); 
                foreach (var i in image)
                {

                    var rptImageModel = new ReportImageViewModel();

                    rptImageModel.ImageName = i.ImageName;
                    rptImageModel.ImageCaption = i.ImageCaption;
                    rptImageModel.ImageDateCreated = i.DateCreated;

                    rptImageList.Add(rptImageModel);
                }

                IQueryable<ReportVideo> video = _applicationDbContext.ReportVideos.Where(r => r.ReportId == SingleReport.ReportId)
                    .OrderByDescending(d => d.DateCreated); 
                foreach (var v in video)
                {
                    var rptVideoModel = new ReportVideoViewModel();

                    rptVideoModel.VideoName = v.VideoName;
                    rptVideoModel.VideoCaption = v.VideoCaption;
                    rptVideoModel.VideoDateCreated = v.DateCreated;

                    rptVideoList.Add(rptVideoModel);
                }

                IQueryable<RipoffCaseUpdate> caseU = _applicationDbContext.RipoffCaseUpdates.Where(r => r.ApprovedLawyerRequest.LawyerRequest.ReportId == SingleReport.ReportId);
                if(caseU != null)
                {
                    foreach (var c in caseU)
                    {
                        mainModel.CaseUpdateExist = true;
                        var caseModel = new RipOffCaseUpdateViewModel();

                        caseModel.CaseUpdateText = c.UpdateText;
                        

                        caseUpdateList.Add(caseModel);
                    }
                }
                else
                {
                    Exception ex;
                }

                IQueryable<RipOffLegalTeamAdvice> legalAdive = _applicationDbContext.RipOffLegalTeamAdvices.Where(m => m.ReportId == SingleReport.ReportId);
                if (legalAdive != null)
                {
                    foreach (var l in legalAdive)
                    {
                        var legalModel = new RipOffLegalTeamViewModel();
                        mainModel.LegalAdviceExist = true;

                        legalModel.LegalAdvice = convert.Convert(l.LegalAdvice);
                        legalModel.DateCreated = l.DateCreated;

                        legalList.Add(legalModel);
                    }
                }
                else { }

                string search = SingleReport.CompanyorIndividual;
                IQueryable<Report> reports = _applicationDbContext.Reports.Where(r => r.ReportText.ToLower().Contains(search) || r.Title.ToLower().Contains(search) || r.CompanyorIndividual.ToLower().Contains(search) || r.Website.ToLower().Contains(SingleReport.CompanyorIndividual) || r.Email.ToLower().Contains(SingleReport.CompanyorIndividual) || r.Address.ToLower().Contains(SingleReport.CompanyorIndividual))
                .OrderByDescending(d => d.DateCreated);
               
                foreach (var q in reports)
                {
                    RelatedReportedCompanyViewModel relModel = new RelatedReportedCompanyViewModel();

                    relModel.ReportText = convert.Convert(q.ReportText);
                    relModel.ReportId = q.ReportId;

                    relatedCompanyReport.Add(relModel);

                }

            }
           

            string PageTitle = SingleReport.CompanyorIndividual + " : " + convert.Convert(SingleReport.ReportText).Substring(0, 50);
            string sm_PageTitle = Regex.Replace(PageTitle, "[^A-Za-z0-9]", "-");

            mainModel.RebuttalViewModels = rebuttalList;
            mainModel.ThreadViewModels = threadList;
            mainModel.ReportUpdateViewModels = rptUpdateList;
            mainModel.ReportImageViewModels = rptImageList;
            mainModel.ReportVideoViewModels = rptVideoList;
            mainModel.CommentViewModels = commentList;
            mainModel.RipOffLegalTeamViewModels = legalList;
            mainModel.LegalAdviceViewModels = updateLegalList;
            mainModel.RelatedReportedCompanyViewModels = relatedCompanyReport;

            ViewBag.PageTitle = sm_PageTitle;
            ViewBag.RandomId = Guid.NewGuid().ToString();

            string rawUrl = this.Request.RawUrl.ToString();

            ViewBag.absoluteUrl = "https://www.ripoff.com.ng/" + rawUrl;

            return View(mainModel);
        }
        public ActionResult MyPage(int? rpage,int? qpage)
        {
            var loggedUser = User.Identity.GetUserId();
            var user = _applicationDbContext.Users.SingleOrDefault(u => u.Id == loggedUser);
            if (user.Career == "General")
            {
                UserReportsandQuestionsWithOwner uReportModel = new UserReportsandQuestionsWithOwner();

                var reports = _applicationDbContext.Reports.Where(u => u.UserId == loggedUser)
                    .Include(lr => lr.LawyerRequests)
                    .OrderByDescending(d => d.DateCreated).ToList();
                var uRList = new List<UserReport>();

                foreach (var rpt in reports)
                {
                    UserReport userModel = new UserReport();
                    userModel.ReportId = rpt.ReportId;
                    userModel.Title = rpt.Title;
                    userModel.Status = rpt.Status;
                    userModel.DateCreated = rpt.DateCreated;

                    foreach (LawyerRequest lReq in rpt.LawyerRequests.OrderByDescending(rId => rId.ReportId))
                    {
                        var legalServiceRequest = _applicationDbContext.LawyerRequests.SingleOrDefault(id => id.ReportId == rpt.ReportId);

                        if(legalServiceRequest != null)
                        {
                            userModel.hasRequestedLegalService = true;
                        }

                    }


                    uRList.Add(userModel);
                }

                IQueryable<Question> questions = _applicationDbContext.Questions.Where(u => u.UserId == loggedUser)
                .OrderByDescending(d => d.DateAsked);

                var uQlist = new List<UserQuestion>();
                foreach (var uQ in questions)
                {
                    UserQuestion uQModel = new UserQuestion();
                    uQModel.QuestionId = uQ.QuestionId;
                    uQModel.Title = uQ.Title;
                    uQModel.isAnswered = uQ.isAnswered;
                    uQModel.DateAsked = uQ.DateAsked;
                    uQlist.Add(uQModel);
                }

                
                int rpageSize = 25;
                int rpageNumber = rpage ?? 1;

                int qpageSize = 25;
                int qpageNumber = qpage ?? 1;

                uReportModel.UserReports = uRList.ToPagedList<UserReport>(rpageNumber, rpageSize);
                uReportModel.UserQuestions = uQlist.ToPagedList<UserQuestion>(qpageNumber, qpageSize);

                return View(uReportModel);
            }
            else
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
                //return RedirectToAction("Login", "Account");
            }

        }
        public ActionResult Thread(int page, ManageMessageId? message)
        {
            var UserId = User.Identity.GetUserId();
            var loggedUserByUserId = _applicationDbContext.Users.SingleOrDefault(i => i.Id == UserId);
            if (loggedUserByUserId.Career == "General")
            {
                ViewBag.StatusMessage =
                message == ManageMessageId.Error ? "You must enter a comment."
                : "";
                CommentForReportViewModel commentModel = new CommentForReportViewModel();
                HtmlToText convert = new HtmlToText();
                var reportById = _applicationDbContext.Reports.Where(m => m.ReportId == page).SingleOrDefault();

                commentModel.ReportId = reportById.ReportId;
                commentModel.ReportText = convert.Convert(reportById.ReportText);

                return View(commentModel);
            }
            else
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult Update(int page, ManageMessageId? message)
        {
            var UserId = User.Identity.GetUserId();
            var loggedUserByUserId = _applicationDbContext.Users.SingleOrDefault(i => i.Id == UserId);
            if (loggedUserByUserId.Career == "General")
            {
                ViewBag.StatusMessage =
                message == ManageMessageId.Error ? "You must enter an update."
                : "";
                ViewBag.ReportId = page;
                return View();
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
        public async Task<ActionResult> Update([Bind(Include = "ReportId,Update")] int ReportId,string Update)
        {
            ReportUpdate rptUpdate = new ReportUpdate();
            rptUpdate.ReportId = ReportId;
            rptUpdate.Update = Update;
            rptUpdate.AdviseStatus = false;
            rptUpdate.UserId = User.Identity.GetUserId();
            rptUpdate.DateCreated = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _applicationDbContext.ReportUpdates.Add(rptUpdate);
                await _applicationDbContext.SaveChangesAsync();

                var iD = Guid.NewGuid().ToString();
                var getReportByReportId = _applicationDbContext.Reports.SingleOrDefault(rId => rId.ReportId == rptUpdate.ReportId);
                return RedirectToAction("ReportDetails", new { Controller = "Report", action = "ReportDetails", title = getReportByReportId.Title, page = rptUpdate.ReportId, id = iD });
            }

            return View();
        }


        public ActionResult Rebuttal(int page, ManageMessageId? message)
        {
            var UserId = User.Identity.GetUserId();
            var loggedInUserById = _applicationDbContext.Users.SingleOrDefault(i => i.Id == UserId);
            if(loggedInUserById.Career == "General")
            {
                ViewBag.StatusMessage =
                 message == ManageMessageId.Error ? "You must enter a rebuttal."
                : "";
                RebuttalForReportViewModel rebuttalModel = new RebuttalForReportViewModel();
                HtmlToText convert = new HtmlToText();
                var reportById = _applicationDbContext.Reports.Where(m => m.ReportId == page).SingleOrDefault();

                rebuttalModel.ReportId = reportById.ReportId;
                rebuttalModel.ReportText = convert.Convert(reportById.ReportText);

                ViewBag.State = _applicationDbContext.States.ToList();
                ViewBag.City = _applicationDbContext.Cities.ToList();

                return View(rebuttalModel);
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
        public async Task<ActionResult> CreateRebuttal([Bind(Include = "RebuttalId,ReportId,UserId,Title,RebuttalText,Address,CityId,StateId,DateCreated")]RebuttalForReportViewModel rebuttal)
        {
            HtmlToText convert = new HtmlToText();
            ManageMessageId? message;
            var reportById = _applicationDbContext.Reports.Where(m => m.ReportId == rebuttal.ReportId).SingleOrDefault();

            string RandomId = Guid.NewGuid().ToString();

            string PageTitle = reportById.CompanyorIndividual + " : " + convert.Convert(reportById.ReportText).Substring(0, 50);
            string sm_PageTitle = Regex.Replace(PageTitle, "[^A-Za-z0-9]", "-");

            Rebuttal reb = new Rebuttal();
            if (rebuttal.RebuttalText != null && rebuttal.Title != null)
            {
                reb.ReportId = rebuttal.ReportId;
                reb.RebuttalText = rebuttal.RebuttalText;
                reb.UserId = User.Identity.GetUserId();
                reb.Title = rebuttal.Title;
                reb.RebuttalText = rebuttal.RebuttalText;
                reb.Address = rebuttal.Address;
                reb.CityId = rebuttal.CityId;
                reb.StateId = rebuttal.StateId;
                reb.Status = false;
                reb.DateCreated = DateTime.UtcNow;

                if (ModelState.IsValid)
                {
                    _applicationDbContext.Rebuttals.Add(reb);
                    await _applicationDbContext.SaveChangesAsync();

                    var getReportOwnerDisplayName = UserManager.FindByIdAsync(reportById.UserId);
                    var getRebuttalDisplayName = UserManager.FindByIdAsync(reb.UserId);
                    string id = Guid.NewGuid().ToString();
                    if (UserManager.EmailService != null)
                    {

                        var callbackUrl = Url.Action(
                            "ReportDetails",
                            "Report",
                            new { title = sm_PageTitle, page = reportById.ReportId, iD = id}, protocol: Request.Url.Scheme);

                        string Body =
                                "<p><h3>Rip-Off NG</h3></p>" +
                                 "<p>Hi " + getReportOwnerDisplayName.Result.Email + ",</p>" +
                                "<p class=\"lead\">A response has been posted to your complaint by " + getRebuttalDisplayName.Result.NameExtension +
                                "<p>----------------------------------------------------------------------------------------------------------------------------------------</p>";

                        await UserManager.SendEmailAsync(reportById.UserId, "Rip-Off Ng | Response to your complaint about: " + reportById.Title, Body + " <a href=\"" + callbackUrl + "\">Click here to view</a>" + "<p></p><p>Do not reply to this email.</p><p>Regards,</p><p>Rip-Off NG Team</p><p>Psst! Remember - this is not a marketing email.Since you have a Rip-Off NG Account,we want to keep you informed about operational updates or changes to our websites.</p>");
                    }

                }

            }
            else
            {
                message = ManageMessageId.Error;
                
                return RedirectToAction("Rebuttal", new { Controller = "Report", action = "Rebuttal", title = sm_PageTitle, page = rebuttal.ReportId, id = RandomId,message});
            }


            return RedirectToAction("ReportDetails", new { Controller = "Report", action = "ReportDetails", title = sm_PageTitle, page = rebuttal.ReportId, id = RandomId });
        }
        public ActionResult Create(ManageMessageId? message)
        {
            var UserId = User.Identity.GetUserId();
            var LoggedInUserById = _applicationDbContext.Users.SingleOrDefault(id => id.Id == UserId);
            if(LoggedInUserById.Career == "General")
            {
                ViewBag.StatusMessage =
                 message == ManageMessageId.Error ? "You must enter a report."
                : "";

                int countryId = 160;
                ViewBag.State = _applicationDbContext.States.Where(c => c.CountryId == countryId).ToList();
                ViewBag.City = _applicationDbContext.Cities.ToList();
                ViewBag.Topic = _applicationDbContext.Topics.ToList();
                ViewBag.Category = _applicationDbContext.Categories.ToList();

                return View();
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
        public async Task<ActionResult> CreateThread([Bind(Include = "ThreadId,ReportId,UserId,ThreadText,DateCreated")]CommentForReportViewModel comment)
        {
            System.Threading.Thread.Sleep(1000);
            HtmlToText convert = new HtmlToText();
            ManageMessageId? message;

            Thread thread = new Thread();
            var reportById = _applicationDbContext.Reports.Where(m => m.ReportId == comment.ReportId).SingleOrDefault();

            string RandomId = Guid.NewGuid().ToString();

            string PageTitle = reportById.CompanyorIndividual + " : " + convert.Convert(reportById.ReportText).Substring(0, 50);
            string sm_PageTitle = Regex.Replace(PageTitle, "[^A-Za-z0-9]", "-");


            thread.ReportId = comment.ReportId;
            thread.ThreadText = comment.ThreadText;
            thread.UserId = User.Identity.GetUserId();
            thread.DateCreated = DateTime.UtcNow;
            if(comment.ThreadText != null)
            {
                if (ModelState.IsValid)
                {
                    _applicationDbContext.Threads.Add(thread);
                
                    await _applicationDbContext.SaveChangesAsync();

                }
                
            }
            else
            {
                message = ManageMessageId.Error;
                return RedirectToAction("Thread", new { Controller = "Report", action = "Thread", title = sm_PageTitle, page = thread.ReportId, id = RandomId,message });
            }

            return RedirectToAction("ReportDetails", new { Controller = "Report", action = "ReportDetails", title = sm_PageTitle, page = thread.ReportId, id = RandomId });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> CreateComment([Bind(Include = "CommentId,ThreadId,UserId,CommentText,DateCreated")]CommentReplyViewModel comment)
        {
            HtmlToText convert = new HtmlToText();
            Comment com = new Comment();

            com.ThreadId = comment.ThreadId;
            com.CommentText = comment.CommentText;
            com.UserId = User.Identity.GetUserId();
            com.DateCreated = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                _applicationDbContext.Comments.Add(com);

                await _applicationDbContext.SaveChangesAsync();

                return Json(new { success = true });
            }
            else
            {
                
                //return RedirectToAction("DisplayReplySection", new { Controller = "Report", action = "DisplayReplySection", title = comment.PageTitle, page = comment.ReportId});

            }

            return View();
        }
        private IList<City> GetCity(int stateId)
        {
            return _applicationDbContext.Cities.Where(m => m.StateId == stateId).ToList();
        }

        private IList<Category> GetCategory(int topicId)
        {
            return _applicationDbContext.Categories.Where(m => m.TopicId == topicId).ToList();
        }

        [AllowAnonymous]
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetCityByStateId(string stateid)
        {
            try
            {
                var cityList = this.GetCity(Convert.ToInt32(stateid));
                var cityData = cityList.Select(m => new SelectListItem()
                {
                    Text = m.Name,
                    Value = m.CityId.ToString(),

                });
                return Json(cityData, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex) { }

            return Json("Not Found", JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetCategoryByTopicId(string topicid)
        {
            try
            {
                var catList = this.GetCategory(Convert.ToInt32(topicid));
                var catData = catList.Select(m => new SelectListItem()
                {
                    Text = m.Name,
                    Value = m.CategoryId.ToString(),

                });
                return Json(catData, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex) { }

            return Json("Not Found", JsonRequestBehavior.AllowGet);
        }

        #region Helpers

        public enum ManageMessageId
        {
            AddReportSuccess,
            UploadImageSuccess,
            UploadVideoSuccess,
            Error,
            RebuttalError
        }

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