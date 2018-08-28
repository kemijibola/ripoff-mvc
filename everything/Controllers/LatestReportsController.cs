using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using everything.DataLayer;
using everything.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.IO;
using everything.ViewModels;
using PagedList;
using System.Collections;
using System.Dynamic;
using everything.Helpers;
using System.Text.RegularExpressions;

namespace everything.Controllers
{
    public class LatestReportsController : ApplicationBaseController
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public string userId = "";

        public LatestReportsController()
        {
        }
        public LatestReportsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

 
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


        // GET: LatestReports
        [AllowAnonymous]
        public ActionResult Index(int? page)
        {

            UserReportViewModel viewModel = new UserReportViewModel();

            IQueryable<Report> reports = _applicationDbContext.Reports
                .Include(t => t.Topic).Include(c => c.Category).Include(r => r.ReportImages)
                .Include(reb => reb.Rebuttals).Include(th => th.Threads).Include(rcu => rcu.RipoffCaseUpdates)
                .Include(upd => upd.ReportUpdates).Include(v => v.ReportVideos)
                .OrderByDescending(d => d.DateCreated);

            var reportList = new List<ReportsWithOwner>();
            var questList = new List<TopTenQuestionForLatestReportViewModel>();
            foreach (var m in reports)
            {
                var models = new ReportsWithOwner();
                HtmlToText convert = new HtmlToText();
                

                models.RandomId = Guid.NewGuid().ToString();

                string PageTitle = m.CompanyorIndividual +" : " + convert.Convert(m.ReportText).Substring(0, 50);
                string sm_PageTitle = Regex.Replace(PageTitle, "[^A-Za-z0-9]", "-");

                models.PageTitle =sm_PageTitle;
                models.ReportId = m.ReportId;
                models.CompanyorIndividual = m.CompanyorIndividual;
                //string reportText = convert.Convert(m.ReportText);
                models.ReportText = convert.Convert(m.ReportText);
                models.DateCreated = m.DateCreated;
                models.CategoryName = m.Category.Name;
                models.TopicName = m.Topic.Name;

                var User = UserManager.FindByIdAsync(m.UserId);
                var ReportOwner = User.Result.NameExtension;
                

                //var CatName = _applicationDbContext.Categories

                models.DisplayName = ReportOwner;

                if(m.ReportImages.Count > 0 )
                {
                    models.ReportImagesExist = true;
                    models.ReportImagesCount = m.ReportImages.Count;

                }
                if (m.Rebuttals.Count > 0)
                {
                    models.RebuttalsExist = true;
                    models.RebuttalsCount = m.Rebuttals.Count;
                }
                if (m.Threads.Count > 0)
                {
                    models.ThreadsExist = true;
                    models.ThreadsCount = m.Threads.Count;
                }
                //if ()
                //{
                 //    models.CaseUpdatesExist = true;

                //}

                if (m.ReportUpdates.Count > 0)
                {
                    models.ReportUpdateExist = true;
                    models.ReportUpdateCount = m.ReportUpdates.Count;
                }
                if(m.ReportVideos.Count > 0)
                {
                    models.ReportVideosExist = true;
                    models.ReportVideoCount = m.ReportVideos.Count;
                }
                else { }

                reportList.Add(models);      

            }

            //var topTenQuestion = _applicationDbContext.Questions
            //            .OrderByDescending(dt => dt.DateAsked)
            //            .Take(10);
            //foreach(var top in topTenQuestion)
            //{
            //    TopTenQuestionForLatestReportViewModel questModel = new TopTenQuestionForLatestReportViewModel();
            //    questModel.QuestionId = top.QuestionId;
            //    questModel.QuestionTitle = top.Title;
            //    questModel.QuestionText = top.QuestionText;

            //    var QuestOwner = UserManager.FindByIdAsync(top.UserId);
            //    string QuestionOwner = QuestOwner.Result.NameExtension;
            //    questModel.QuestionOwner = QuestionOwner;
            //    questModel.DateAsked = top.DateAsked.ToString("d,MMMM yy");
            //    questModel.TimeAsked = top.DateAsked.ToString("H:mm tt");

            //    questList.Add(questModel);
            //}

            int pageSize = 25;
            int pageNumber = page ?? 1;
            //viewModel.TopTenQuestionForLatestReportViewModels = questList;
            viewModel.ReportsWithOwners = reportList.ToPagedList<ReportsWithOwner>(pageNumber, pageSize);

            return View(viewModel);

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetTopRecentQuestions()
        {
            HtmlToText convert = new HtmlToText();
            UserReportViewModel mainModel = new UserReportViewModel();
            var questList = new List<TopTenQuestionForLatestReportViewModel>();

            var topTenQuestion = _applicationDbContext.Questions
                        .OrderByDescending(dt => dt.DateAsked)
                        .Take(10);
            foreach (var top in topTenQuestion)
            {
                TopTenQuestionForLatestReportViewModel questModel = new TopTenQuestionForLatestReportViewModel();
                questModel.QuestionId = top.QuestionId;
                questModel.QuestionTitle = top.Title;
                questModel.QuestionText = top.QuestionText;

                var QuestOwner = UserManager.FindByIdAsync(top.UserId);
                string QuestionOwner = QuestOwner.Result.NameExtension;
                questModel.QuestionOwner = QuestionOwner;
                questModel.DateAsked = top.DateAsked.ToString("d,MMMM yy");
                questModel.TimeAsked = top.DateAsked.ToString("H:mm tt");

                questList.Add(questModel);
            }
            mainModel.TopTenQuestionForLatestReportViewModels = questList;
            return PartialView("_RecentQuestions", mainModel);
        }
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