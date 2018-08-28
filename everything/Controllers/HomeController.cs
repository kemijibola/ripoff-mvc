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
    public class HomeController : ApplicationBaseController
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public HomeController()
        {
        }
        public HomeController(ApplicationUserManager userManager)
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

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var mainModel = new FeedbackWithBlog();
            var listFeedback = new List<FeedbackViewModel>();
            var listBlog = new List<BlogViewModel>();
            var homeModel = new HomeViewModel();
            var convert = new HtmlToText();

            var blogs = _applicationDbContext.Blogs.OrderByDescending(d => d.DateCreated).Take(10).ToList();
            foreach(var i in blogs)
            {
                var blogModel = new BlogViewModel();
                blogModel.BlogId = i.BlogId;
                blogModel.Title = i.Title;
                blogModel.DateCreated = i.DateCreated;
                listBlog.Add(blogModel);
            }

            var feedbacks = _applicationDbContext.Feedbacks.OrderByDescending(d => d.DateCreated).Take(10).ToList();
            foreach(var i in feedbacks)
            {
                
                var User = UserManager.FindByIdAsync(i.UserId);
                var userName = User.Result.NameExtension;
               
                 var model = new FeedbackViewModel()
                {
                     DateCreated = i.DateCreated,
                     Message = i.Message,
                     UserName = userName
                };

                listFeedback.Add(model);
            }

            IQueryable<Report> reports = _applicationDbContext.Reports
            .OrderByDescending(d => d.DateCreated)
            .Take(10);
            var reportList = new List<IndexReportViewModel>();
            foreach (var report in reports)
            {
                var reportModel = new IndexReportViewModel();
                reportModel.ReportId = report.ReportId;
                var User = UserManager.FindByIdAsync(report.UserId);
                var ReportOwner = User.Result.NameExtension;
                reportModel.DisplayName = ReportOwner;

                string myString = convert.Convert(report.ReportText).Substring(0, 90);
                int index = myString.LastIndexOf(' ');
                string outputString = myString.Substring(0, index);

                reportModel.ReportText = outputString;
                reportModel.DateCreated = report.DateCreated;

                reportList.Add(reportModel);
            }

            IQueryable<Question> questions = _applicationDbContext.Questions
                           .OrderByDescending(d => d.DateAsked)
                          .Take(10);

            var questionList = new List<IndexQuestionViewModel>();
            foreach (var question in questions)
            {
                var questionModel = new IndexQuestionViewModel();
                questionModel.QuestionId = question.QuestionId;
                var User = UserManager.FindByIdAsync(question.UserId);
                var ReportOwner = User.Result.NameExtension;
                questionModel.DisplayName = ReportOwner;

                questionModel.QuestionText = question.QuestionText;
                questionModel.DateAsked = question.DateAsked;
                questionModel.QuestionTitle = question.Title;

                questionList.Add(questionModel);
            }
            
            ViewBag.Questions = questionList;

            ViewBag.Complaints = reportList;

            mainModel.Posts = listBlog;
            mainModel.Testimonials = listFeedback;
            return View(mainModel);
        }

        [AllowAnonymous]
        public ActionResult Consumer()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Help()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> News(int? page)
        {
            var model = new NewsCreatedByViewModel();
            var newsList = new List<AllNewsViewModel>();

            var blogs = _applicationDbContext.Blogs
                .OrderByDescending(d => d.DateCreated);

            foreach (var item in blogs)
            {
                var newsModel = new AllNewsViewModel();
                newsModel.BlogId = item.BlogId;
                newsModel.Created = item.DateCreated;
                newsModel.Post = item.Post;
                newsModel.Title = item.Title;

                var User = await UserManager.FindByIdAsync(item.UserId);
                var postedBy = User.NameExtension;
                newsModel.PostedBy = postedBy;

                newsList.Add(newsModel);
            }

            int pageSize = 25;
            int pageNumber = page ?? 1;
            model.NewsWithOwner = newsList.ToPagedList<AllNewsViewModel>(pageNumber, pageSize);

            return View(model);
        }


        [AllowAnonymous]
        public  async Task<ActionResult> NewsUpdate(int post)
        {
            ViewBag.TopTen = _applicationDbContext.Blogs.OrderByDescending(d => d.DateCreated).Take(10).ToList();

            var article = await _applicationDbContext.Blogs.Where(i => i.BlogId == post).Select(x => new FullNewsViewModel
            {
                   BlogId = x.BlogId,
                  PublishedBy = x.User.NameExtension,
                  BlogComment = x.BlogComments,
                  CommentBy = x.BlogComments.Select(d => d.User.NameExtension).FirstOrDefault(),
                  Body = x.Post,
                  BlogImages = x.BlogImages,
                  BlogVideos = x.BlogVideos,
                  DateCreated = x.DateCreated,
                  Title = x.Title
            }).FirstOrDefaultAsync();

            var rawUrl = this.Request.RawUrl.ToString();

            ViewBag.absoluteUrl = "https://www.ripoff.com.ng/" + rawUrl;
            ViewBag.MetaDesc = article.Title;
            return View(article);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetAllRecentComplaints()
        {
            HtmlToText convert = new HtmlToText();
            HomeViewModel homeModel = new HomeViewModel();

            IQueryable<Report> reports = _applicationDbContext.Reports
                .OrderByDescending(d => d.DateCreated)
                .Take(10);
            var reportList = new List<IndexReportViewModel>();
            foreach (var report in reports)
            {
                var reportModel = new IndexReportViewModel();
                reportModel.ReportId = report.ReportId;
                var User = UserManager.FindByIdAsync(report.UserId);
                var ReportOwner = User.Result.NameExtension;
                reportModel.DisplayName = ReportOwner;

                string myString = convert.Convert(report.ReportText).Substring(0, 90);
                int index = myString.LastIndexOf(' ');
                string outputString = myString.Substring(0, index);

                reportModel.ReportText = outputString;
                reportModel.DateCreated = report.DateCreated;

                reportList.Add(reportModel);
            }
            homeModel.IndexReportViewModels = reportList;
            //return View("Index", homeModel);
            return PartialView("_RecentComplaints", homeModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetAllRecentQuestions()
        {
            HtmlToText convert = new HtmlToText();
            HomeViewModel homeModel = new HomeViewModel();

             IQueryable<Question> questions = _applicationDbContext.Questions
                .OrderByDescending(d => d.DateAsked)
               .Take(10);

                var questionList = new List<IndexQuestionViewModel>();
                foreach (var question in questions)
                {
                    var questionModel = new IndexQuestionViewModel();
                    questionModel.QuestionId = question.QuestionId;
                    var User = UserManager.FindByIdAsync(question.UserId);
                    var ReportOwner = User.Result.NameExtension;
                    questionModel.DisplayName = ReportOwner;

                    questionModel.QuestionText = question.QuestionText;
                    questionModel.DateAsked = question.DateAsked;
                    questionModel.QuestionTitle = question.Title;

                    questionList.Add(questionModel);
                }
                homeModel.IndexQuestionViewModels = questionList;
                //return View("Index", homeModel);
                return PartialView("_RecentQuestions", homeModel);
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            //ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
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