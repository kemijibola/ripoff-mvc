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
using everything.Hubs;

namespace everything.Controllers
{
    public class QuestionController : ApplicationBaseController
    {
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        
        public DateTime lastAnswerToDate;
        public DateTime lastAnswerComment;

        public QuestionController()
        {
        }
        public QuestionController(ApplicationUserManager userManager)
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
        public ActionResult AllQuestion(string sort, string search, int? page)
        {
            int DiscussionCategoryId = 0;
            string s = sort;
            int result;

            if (int.TryParse(s, out result))
            {
                DiscussionCategoryId = Convert.ToInt32(sort);
            }
            else { }

            ViewBag.NewestSort = sort == "newestsearch" ? "newest_desc" : "newest_search";
            ViewBag.OldestSort = sort == "oldestsearch" ? "oldest_asec" : "oldest_search";
            ViewBag.DiscussionCategory = sort == "discussioncategory" ? "discuss_cat" : "discussion_category";

            ViewBag.CurrentSort = sort;
            ViewBag.CurrentSearch = search;

            AllQuestionListViewModel mainModel = new AllQuestionListViewModel();
            var listModel = new List<QuestionListWithOwnerAndLastParticipantViewModel>();
            var reportList = new List<TopTenReport>();
            var topQList = new List<TopSevenQuestions>();
            var likeList = new List<MostLikeQuestion>();



            if (!String.IsNullOrEmpty(search))
            {
                var questions = _applicationDbContext.Questions.Include(dc => dc.DiscussionCategory).Where(d => d.Title.ToLower().Contains(search) ||
                    d.QuestionText.ToLower().Contains(search) || d.Tag.ToLower().Contains(search)).ToList();


                foreach (var q in questions)
                {
                    var answers = _applicationDbContext.AnswerToQuestions.Where(id => id.QuestionId == q.QuestionId).ToList();
                    QuestionListWithOwnerAndLastParticipantViewModel model = new QuestionListWithOwnerAndLastParticipantViewModel();
                    model.QuestionId = q.QuestionId;
                    model.Title = q.Title;
                    model.DiscussionCategory = q.DiscussionCategory.Name;
                    model.DateAsked = q.DateAsked.ToString("d,MMMM yy");

                    DateTime commentDate = DateTime.UtcNow.Date;
                    DateTime answerDate = DateTime.UtcNow.Date;
                    var userName = _applicationDbContext.Users.Find(q.UserId);
                    var getQuestionComment = _applicationDbContext.QuestionComments.Where(id => id.QuestionId == q.QuestionId).OrderByDescending(p => p.QuestionCommentId).FirstOrDefault();
                    var getAnswerComment = _applicationDbContext.AnswerToQuestions.Where(id => id.QuestionId == q.QuestionId).OrderByDescending(p => p.AnswerToQuestionId).FirstOrDefault();
                    model.QuestionOwner = userName.NameExtension;
                    if (answers != null)
                    {
                        model.AnswerCount = answers.Count;
                    }
                    else
                    {
                        model.AnswerCount = 0;
                    }

                    if (getQuestionComment == null)
                    {
                        model.LastParticipantOwner = userName.NameExtension;
                        model.LastParticipantDate = q.DateAsked.ToString("d,MMMM yy");
                        commentDate = q.DateAsked;
                    }
                    else
                    {
                        commentDate = getQuestionComment.DateCommented;
                    }
                    if (getAnswerComment == null)
                    {
                        model.LastParticipantOwner = userName.NameExtension;
                        model.LastParticipantDate = q.DateAsked.ToString("d,MMM yy");
                        answerDate = q.DateAsked;
                    }
                    else
                    {
                        answerDate = getAnswerComment.DateAnswered;
                    }
                    if (getQuestionComment != null)
                    {
                        if (commentDate >= answerDate)
                        {
                            var commentOwner = _applicationDbContext.Users.Find(getQuestionComment.UserId);
                            model.LastParticipantOwner = commentOwner.NameExtension;
                            model.LastParticipantDate = getQuestionComment.DateCommented.ToString("d,MMMM yy");
                        }
                    }
                    if (getAnswerComment != null)
                    {
                        if (commentDate <= answerDate)
                        {
                            var questionOwner = _applicationDbContext.Users.Find(getAnswerComment.UserId);
                            model.LastParticipantOwner = questionOwner.NameExtension;
                            model.LastParticipantDate = getAnswerComment.DateAnswered.ToString("d,MMMM yy");
                        }
                    }

                    listModel.Add(model);
                }


            }
            else
            {
                var questions = _applicationDbContext.Questions.Include(dc => dc.DiscussionCategory).OrderByDescending(d => d.DateAsked).ToList();

                foreach (var q in questions)
                {
                    var answers = _applicationDbContext.AnswerToQuestions.Where(id => id.QuestionId == q.QuestionId).ToList();
                    QuestionListWithOwnerAndLastParticipantViewModel model = new QuestionListWithOwnerAndLastParticipantViewModel();
                    model.QuestionId = q.QuestionId;
                    model.Title = q.Title;
                    model.DiscussionCategory = q.DiscussionCategory.Name;
                    model.DateAsked = q.DateAsked.ToString("d,MMMM yy");

                    DateTime commentDate = DateTime.UtcNow.Date;
                    DateTime answerDate = DateTime.UtcNow.Date;
                    var userName = _applicationDbContext.Users.Find(q.UserId);
                    var getQuestionComment = _applicationDbContext.QuestionComments.Where(id => id.QuestionId == q.QuestionId).OrderByDescending(p => p.QuestionCommentId).FirstOrDefault();
                    var getAnswerComment = _applicationDbContext.AnswerToQuestions.Where(id => id.QuestionId == q.QuestionId).OrderByDescending(p => p.AnswerToQuestionId).FirstOrDefault();
                    model.QuestionOwner = userName.NameExtension;
                    if(answers != null)
                    {
                        model.AnswerCount = answers.Count;
                    }
                    else
                    {
                        model.AnswerCount = 0;
                    }

                    if(getQuestionComment == null)
                    {
                        model.LastParticipantOwner = userName.NameExtension;
                        model.LastParticipantDate = q.DateAsked.ToString("d,MMMM yy");
                        commentDate = q.DateAsked;
                    }
                    else
                    {
                        commentDate = getQuestionComment.DateCommented;
                    }
                    if(getAnswerComment == null)
                    {
                        model.LastParticipantOwner = userName.NameExtension;
                        model.LastParticipantDate = q.DateAsked.ToString("d,MMM yy");
                        answerDate = q.DateAsked;
                    }
                    else
                    {
                        answerDate = getAnswerComment.DateAnswered;
                    }
                    if(getQuestionComment != null)
                    {
                        if (commentDate >= answerDate)
                        {
                            var commentOwner = _applicationDbContext.Users.Find(getQuestionComment.UserId);
                            model.LastParticipantOwner = commentOwner.NameExtension;
                            model.LastParticipantDate = getQuestionComment.DateCommented.ToString("d,MMMM yy");
                        }
                    }
                    if(getAnswerComment != null)
                    {
                        if (commentDate <= answerDate)
                        {
                            var questionOwner = _applicationDbContext.Users.Find(getAnswerComment.UserId);
                            model.LastParticipantOwner = questionOwner.NameExtension;
                            model.LastParticipantDate = getAnswerComment.DateAnswered.ToString("d,MMMM yy");
                        }
                    }

                    listModel.Add(model);
                }

            }

            HtmlToText convert = new HtmlToText();

            var topReport = _applicationDbContext.Reports
                    .OrderByDescending(dt => dt.DateCreated)
                    .Take(7);
            foreach (var top in topReport)
            {
                
                TopTenReport rmodel = new TopTenReport();
                rmodel.ReportText = convert.Convert(top.ReportText);
                rmodel.ReportId = top.ReportId;
                rmodel.Title = top.Title;
                rmodel.ReportDate = top.DateCreated;
                reportList.Add(rmodel);

            }

            var mostLikedQuestion = _applicationDbContext.QuestionLikes.ToList();
            var itemsWithTotals = mostLikedQuestion
                                .GroupBy(item => item.QuestionId)

                                    .Select(groupByQId => new
                                    {
                                        QuestionId = groupByQId.Key,
                                        Total = groupByQId.Count()
                                    })
                                    .OrderByDescending(t => t.Total)
                                    .Take(7);

                foreach (var item in itemsWithTotals)
                {
                var getQuestion = this._applicationDbContext.Questions.SingleOrDefault(id => id.QuestionId == item.QuestionId);
                    MostLikeQuestion mModel = new MostLikeQuestion();
                    mModel.QuestionId = item.QuestionId;
                    mModel.QuestionLikeCount = item.Total;
                    mModel.QuestionTitle = getQuestion.Title;
                    mModel.QuestionText = getQuestion.QuestionText;
                    mModel.DateAsked = getQuestion.DateAsked;
                    likeList.Add(mModel);
                }

             var topSevenQuestion = _applicationDbContext.Questions
                                    .OrderByDescending(dt => dt.DateAsked)
                                    .Take(7);
            foreach (var qs in topSevenQuestion)
            {
                TopSevenQuestions qmodel = new TopSevenQuestions();
                qmodel.QuestionId = qs.QuestionId;
                qmodel.QuestionTitle = qs.Title;
                qmodel.QuestionText = convert.Convert(qs.QuestionText);
                qmodel.DateAsked = qs.DateAsked;
                topQList.Add(qmodel);
            }

            int pageSize = 50;
            int pageNumber = page ?? 1;
            ViewBag.DiscussionCategory = new SelectList(_applicationDbContext.DiscussionCategories, "DiscussionCategoryId", "Name");
            mainModel.TopTenReports = reportList;
            mainModel.TopSevenQuestions = topQList;
            mainModel.MostLikeQuestions = likeList;
            mainModel.QuestionListWithOwnerAndLastParticipantViewModels = listModel.ToPagedList<QuestionListWithOwnerAndLastParticipantViewModel>(pageNumber, pageSize);
            return View(mainModel);
        }

        [AllowAnonymous]
        public ActionResult QuestionList(int q, string sort, string search, int? page)
        {
            QuestionAndReportViewModel mainModel = new QuestionAndReportViewModel();
            HtmlToText convert = new HtmlToText();

            ViewBag.NewestSort = sort == "newestsearch" ? "newest_desc" : "newest_search";
            ViewBag.OldestSort = sort == "oldestsearch" ? "oldest_asec" : "oldest_search";

            ViewBag.CurrentSort = sort;
            ViewBag.CurrentSearch = search;

            var questModel = _applicationDbContext.Questions.Where(catId => catId.DiscussionCategoryId == q).Include(d => d.DiscussionCategory)
                .OrderByDescending(dt => dt.DateAsked).ToList();

            var questList = new List<QuestionListViewModel>();
            var reportList = new List<TopTenReport>();
            var topQList = new List<TopSevenQuestions>();
            var likeList = new List<MostLikeQuestion>();


            foreach (var quest in questModel)
            {
                if (!String.IsNullOrEmpty(search))
                {
                    if(quest.Title.ToLower().Contains(search)|| quest.Title.ToLower().Contains(search) || quest.Tag.ToLower().Contains(search))
                    {
                        QuestionListViewModel model = new QuestionListViewModel();

                        var answers = _applicationDbContext.AnswerToQuestions.Where(id => id.QuestionId == quest.QuestionId).ToList();
                        model.QuestionId = quest.QuestionId;
                        model.Title = quest.Title;
                        model.DiscussionCategory = quest.DiscussionCategory.Name;
                        model.DateAsked = quest.DateAsked.ToString("d,MMMM yy");

                        DateTime commentDate = DateTime.UtcNow.Date;
                        DateTime answerDate = DateTime.UtcNow.Date;
                        var userName = _applicationDbContext.Users.Find(quest.UserId);
                        var getQuestionComment = _applicationDbContext.QuestionComments.Where(id => id.QuestionId == quest.QuestionId).OrderByDescending(p => p.QuestionCommentId).FirstOrDefault();
                        var getAnswerComment = _applicationDbContext.AnswerToQuestions.Where(id => id.QuestionId == quest.QuestionId).OrderByDescending(p => p.AnswerToQuestionId).FirstOrDefault();
                        model.QuestionOwner = userName.NameExtension;
                        if (answers != null)
                        {
                            model.AnswerCount = answers.Count;
                        }
                        else
                        {
                            model.AnswerCount = 0;
                        }

                        if (getQuestionComment == null)
                        {
                            model.LastParticipantOwner = userName.NameExtension;
                            model.LastParticipantDate = quest.DateAsked.ToString("d,MMMM yy");
                            commentDate = quest.DateAsked;
                        }
                        else
                        {
                            commentDate = getQuestionComment.DateCommented;
                        }
                        if (getAnswerComment == null)
                        {
                            model.LastParticipantOwner = userName.NameExtension;
                            model.LastParticipantDate = quest.DateAsked.ToString("d,MMM yy");
                            answerDate = quest.DateAsked;
                        }
                        else
                        {
                            answerDate = getAnswerComment.DateAnswered;
                        }
                        if (getQuestionComment != null)
                        {
                            if (commentDate >= answerDate)
                            {
                                var commentOwner = _applicationDbContext.Users.Find(getQuestionComment.UserId);
                                model.LastParticipantOwner = commentOwner.NameExtension;
                                model.LastParticipantDate = getQuestionComment.DateCommented.ToString("d,MMMM yy");
                            }
                        }
                        if (getAnswerComment != null)
                        {
                            if (commentDate <= answerDate)
                            {
                                var questionOwner = _applicationDbContext.Users.Find(getAnswerComment.UserId);
                                model.LastParticipantOwner = questionOwner.NameExtension;
                                model.LastParticipantDate = getAnswerComment.DateAnswered.ToString("d,MMMM yy");
                            }
                        }

                        questList.Add(model);
                        string title = quest.DiscussionCategory.Description;
                        string PageTitle = Regex.Replace(title, "[^A-Za-z0-9]", "-");
                        ViewBag.PageTitle = PageTitle;
                    }
                    else { }

                }
                else
                {
                    QuestionListViewModel model = new QuestionListViewModel();

                    var answers = _applicationDbContext.AnswerToQuestions.Where(id => id.QuestionId == quest.QuestionId).ToList();
                    model.QuestionId = quest.QuestionId;
                    model.Title = quest.Title;
                    model.DiscussionCategory = quest.DiscussionCategory.Name;
                    model.DateAsked = quest.DateAsked.ToString("d,MMMM yy");

                    DateTime commentDate = DateTime.UtcNow.Date;
                    DateTime answerDate = DateTime.UtcNow.Date;
                    var userName = _applicationDbContext.Users.Find(quest.UserId);
                    var getQuestionComment = _applicationDbContext.QuestionComments.Where(id => id.QuestionId == quest.QuestionId).OrderByDescending(p => p.QuestionCommentId).FirstOrDefault();
                    var getAnswerComment = _applicationDbContext.AnswerToQuestions.Where(id => id.QuestionId == quest.QuestionId).OrderByDescending(p => p.AnswerToQuestionId).FirstOrDefault();
                    model.QuestionOwner = userName.NameExtension;
                    if (answers != null)
                    {
                        model.AnswerCount = answers.Count;
                    }
                    else
                    {
                        model.AnswerCount = 0;
                    }

                    if (getQuestionComment == null)
                    {
                        model.LastParticipantOwner = userName.NameExtension;
                        model.LastParticipantDate = quest.DateAsked.ToString("d,MMMM yy");
                        commentDate = quest.DateAsked;
                    }
                    else
                    {
                        commentDate = getQuestionComment.DateCommented;
                    }
                    if (getAnswerComment == null)
                    {
                        model.LastParticipantOwner = userName.NameExtension;
                        model.LastParticipantDate = quest.DateAsked.ToString("d,MMM yy");
                        answerDate = quest.DateAsked;
                    }
                    else
                    {
                        answerDate = getAnswerComment.DateAnswered;
                    }
                    if (getQuestionComment != null)
                    {
                        if (commentDate >= answerDate)
                        {
                            var commentOwner = _applicationDbContext.Users.Find(getQuestionComment.UserId);
                            model.LastParticipantOwner = commentOwner.NameExtension;
                            model.LastParticipantDate = getQuestionComment.DateCommented.ToString("d,MMMM yy");
                        }
                    }
                    if (getAnswerComment != null)
                    {
                        if (commentDate <= answerDate)
                        {
                            var questionOwner = _applicationDbContext.Users.Find(getAnswerComment.UserId);
                            model.LastParticipantOwner = questionOwner.NameExtension;
                            model.LastParticipantDate = getAnswerComment.DateAnswered.ToString("d,MMMM yy");
                        }
                    }

                    questList.Add(model);
                    string title = quest.DiscussionCategory.Description;
                    string PageTitle = Regex.Replace(title, "[^A-Za-z0-9]", "-");
                    ViewBag.PageTitle = PageTitle;
                }
            }
            if (questList.Count == 0)
            {
                mainModel.QuestionCount = 0;
            }
            if (questList.Count > 0)
            {
                mainModel.QuestionCount = 1;
            }
            var CategoryName = _applicationDbContext.DiscussionCategories.SingleOrDefault(d => d.DiscussionCategoryId == q);

            
            var topReport = _applicationDbContext.Reports
                                .OrderByDescending(dt => dt.DateCreated)
                                .Take(7);
            foreach(var top in topReport)
            {
                TopTenReport rmodel = new TopTenReport();
                rmodel.ReportText = convert.Convert(top.ReportText);
                rmodel.ReportId = top.ReportId;
                rmodel.Title = top.Title;
                rmodel.ReportDate = top.DateCreated;
                reportList.Add(rmodel);
                
            }

            var mostLikedQuestion = _applicationDbContext.QuestionLikes.ToList();
            var itemsWithTotals = mostLikedQuestion
                                .GroupBy(item => item.QuestionId)

                                    .Select(groupByQId => new
                                    {
                                        QuestionId = groupByQId.Key,
                                        Total = groupByQId.Count()
                                    })
                                    .OrderByDescending(t => t.Total)
                                    .Take(7);
            foreach (var item in itemsWithTotals)
            {
                var getQuestion = this._applicationDbContext.Questions.SingleOrDefault(id => id.QuestionId == item.QuestionId);
                MostLikeQuestion mModel = new MostLikeQuestion();
                mModel.QuestionId = item.QuestionId;
                mModel.QuestionLikeCount = item.Total;
                mModel.QuestionTitle = getQuestion.Title;
                mModel.QuestionText = getQuestion.QuestionText;
                mModel.DateAsked = getQuestion.DateAsked;
                likeList.Add(mModel);
            }

            var topSevenQuestion = _applicationDbContext.Questions
                                    .OrderByDescending(dt => dt.DateAsked)
                                    .Take(7);
            foreach(var qs in topSevenQuestion)
            {
                TopSevenQuestions qmodel = new TopSevenQuestions();
                qmodel.QuestionId = qs.QuestionId;
                qmodel.QuestionTitle = qs.Title;
                qmodel.QuestionText = convert.Convert(qs.QuestionText);
                topQList.Add(qmodel);
            }
            int pageSize = 50;
            int pageNumber = page ?? 1;
            
            ViewBag.Category = CategoryName.Name;
            ViewBag.q = q;
            
            mainModel.TopTenReports = reportList;
            mainModel.TopSevenQuestions = topQList;
            mainModel.MostLikeQuestions = likeList;
            mainModel.QuestionListViewModels = questList.ToPagedList<QuestionListViewModel>(pageNumber, pageSize);
            return View(mainModel);
        }

        // GET: Question
        [AllowAnonymous]
        public ActionResult Index()
        {
            QuestionwithCategoryandMaxEntryViewModel qcm = new QuestionwithCategoryandMaxEntryViewModel();
            var dcCat = _applicationDbContext.DiscussionCategories.OrderBy(n => n.Name).ToList();
            var dcList = new List<DiscussionModel>();
            foreach(var item in dcCat)
            {
                var questionsByCatId = _applicationDbContext.Questions.Where(id => id.DiscussionCategoryId == item.DiscussionCategoryId).ToList();
                DiscussionModel dcModel = new DiscussionModel();
                dcModel.Description = item.Description;
                dcModel.DiscussionName = item.Name;
                dcModel.DiscussionCategoryId = item.DiscussionCategoryId;
                dcModel.TotalEntryForCategory = questionsByCatId.Count();
                dcList.Add(dcModel);
            }
            qcm.DiscussionModels = dcList;
            return View(qcm);
        }

        public ActionResult Create(ManageMessageId? message)
        {
            var UserId = User.Identity.GetUserId();
            var loggedUserByUserId = _applicationDbContext.Users.SingleOrDefault(i => i.Id == UserId);
            if (loggedUserByUserId.Career == "General")
            {

                ViewBag.StatusMessage =
                message == ManageMessageId.Error ? "Field cannot be left blank."
                : "";
                ViewBag.DiscussionCategory = new SelectList(_applicationDbContext.DiscussionCategories, "DiscussionCategoryId", "Name");
                return View();
            }
            else
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Reply(int page,string title,string iD)
        {
            var UserId = User.Identity.GetUserId();
            var loggedUserByUserId = _applicationDbContext.Users.SingleOrDefault(i => i.Id == UserId);
            if (loggedUserByUserId.Career == "General")
            {

                CreateAnswerViewModel ansModel = new CreateAnswerViewModel();
                var getAnswerByanswerId = _applicationDbContext.AnswerToQuestions.SingleOrDefault(i => i.AnswerToQuestionId == page);
                ansModel.AnswerToQuestionId = getAnswerByanswerId.AnswerToQuestionId;
                ansModel.AnswerText = getAnswerByanswerId.Answer;

                return View(ansModel);
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
        public async Task<ActionResult> CreateAnswerComment(AnswerComment answercom)
        {

                var getQuestionIdByAnswerToQuestionId = _applicationDbContext.AnswerToQuestions.SingleOrDefault(i => i.AnswerToQuestionId == answercom.AnswerToQuestionId);
                if (ModelState.IsValid)
                {
                    answercom.UserId = User.Identity.GetUserId();
                    answercom.DateCommented = DateTime.UtcNow;

                    _applicationDbContext.AnswerComments.Add(answercom);
                    await _applicationDbContext.SaveChangesAsync();


                    var question = _applicationDbContext.Questions.SingleOrDefault(i => i.QuestionId == getQuestionIdByAnswerToQuestionId.QuestionId);
                   string title = question.Title;
                    string titleC = Regex.Replace(title, "[^A-Za-z0-9]", "-");

                    string cat = question.DiscussionCategory.Name;
                     string catC = Regex.Replace(cat, "[^A-Za-z0-9]", "-");
                     string titleCat = titleC + "&" + catC;

                    string id = Guid.NewGuid().ToString();


                    return RedirectToAction("Thread", new { Controller = "Question", action = "Thread", page = question.QuestionId, title = titleCat , iD = id });

                    //return Json(new { success = true });
                }

            else
            {
                string iD = Guid.NewGuid().ToString();
                return RedirectToAction("CreateAnswerComment", new { Controller = "Question", action = "CreateAnswerComment", page = answercom.AnswerToQuestionId, id = iD });        

            }

        }

        [HttpPost, ValidateJsonAntiForgeryTokenAttribute]
        public async Task<ActionResult> CreateAnswerToQuestion(string Answer,int QuestionId)
        {
            HtmlToText convert = new HtmlToText();

            AnswerToQuestion an = new AnswerToQuestion();

            an.QuestionId = QuestionId;
            an.Answer = Answer;
            an.UserId = User.Identity.GetUserId();
            an.DateAnswered = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                _applicationDbContext.AnswerToQuestions.Add(an);
                await _applicationDbContext.SaveChangesAsync();
                
                var getQuestionByAnswerToQuestionId = _applicationDbContext.Questions.SingleOrDefault(q => q.QuestionId == an.QuestionId);
                string id = Guid.NewGuid().ToString();
                var getQuestionOwnerEmail = UserManager.FindByIdAsync(getQuestionByAnswerToQuestionId.UserId);
                var getCommenterDisplayName = UserManager.FindByIdAsync(an.UserId);
                if (UserManager.EmailService != null && getQuestionByAnswerToQuestionId.NotifyMe == true && an.UserId != getQuestionByAnswerToQuestionId.UserId) 
                {
                    var callbackUrl = Url.Action(
                        "Thread",
                        "Question",
                        new { title = getQuestionByAnswerToQuestionId.Title, page = getQuestionByAnswerToQuestionId.QuestionId, iD = id }, protocol: Request.Url.Scheme);

                    string Body = 
                            "<p><h3>Rip-Off NG</h3></p>" +
                             "<p>Hi " + getQuestionOwnerEmail.Result.Email + ",</p>" +
                            "<p class=\"lead\">An answer has been posted to your question by " + getCommenterDisplayName.Result.NameExtension+
                            "<p>----------------------------------------------------------------------------------------------------------------------------------------</p>";

                    await UserManager.SendEmailAsync(getQuestionByAnswerToQuestionId.UserId, "Rip-Off Ng | Someone replied to your question: " +getQuestionByAnswerToQuestionId.Title, Body + " <a href=\"" + callbackUrl + "\">Click here to view and reply</a>" + "<p></p><p>Do not reply to this email</p><p>Regards,</p><p>Rip-Off NG Team</p><p>Psst! Remember - this is not a marketing email.Since you have a Rip-Off NG Account,we want to keep you informed about operational updates or changes to our websites.</p>");
                }

                var updateQuestion = _applicationDbContext.Questions.SingleOrDefault(iQ => iQ.QuestionId == getQuestionByAnswerToQuestionId.QuestionId);

                updateQuestion.isAnswered = true;
                var updateResult = _applicationDbContext.Entry(updateQuestion).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();
                

                return Json(new { success = true });
            }
            else
            {}

            return View();
        }


        [AllowAnonymous]
        public ActionResult Thread(int page, string title, string iD)
        {
            FullQuestionViewModel mainModel = new FullQuestionViewModel();
            HtmlToText convert = new HtmlToText();

            int questionId = page;

            var SingleQuestion = _applicationDbContext.Questions
                .Where(q => q.QuestionId == page)
                .Include(a => a.AnswerToQuestions)
                .Include(c => c.QuestionComments)
                .Include(d => d.DiscussionCategory)
                .Include(l => l.QuestionLikes)
                .SingleOrDefault();

            string loggedInUser = User.Identity.GetUserId();
            mainModel.CurrentUser = loggedInUser;

            mainModel.QuestionLikes = SingleQuestion.QuestionLikes.Count(e => e.UserLike);
            var UserLikeStatus = SingleQuestion.QuestionLikes.SingleOrDefault(i => i.UserId == loggedInUser && i.UserLike == true);
            if(UserLikeStatus == null)
            {
                mainModel.hasUserLiked = false;
            }
             else
            {
                mainModel.hasUserLiked = true;
            }

            var acList = new List<AnswerCommentViewModel>();
            var atqList = new List<AnswerToQuestionViewModel>();
            var relQlist = new List<RelatedQuestionViewModel>();
            
            if (SingleQuestion != null)
            {
                var User = UserManager.FindByIdAsync(SingleQuestion.UserId);
                var UserImage = _applicationDbContext.UserProfilePhotos.SingleOrDefault(u => u.UserId == SingleQuestion.UserId);
                if(UserImage == null)
                {
                    mainModel.QuestionOwnerDP = "person.gif";
                }
                else
                {
                    mainModel.QuestionOwnerDP = UserImage.ImageName;
                }
                var QuestionOwner = User.Result.NameExtension;

                //string QuestionTime = SingleQuestion.DateAsked.ToString("H:mm");
                mainModel.QuestionId = SingleQuestion.QuestionId;
                mainModel.QuestionOwner = QuestionOwner;
                mainModel.Title = SingleQuestion.Title;
                mainModel.QuestionText = SingleQuestion.QuestionText;
                mainModel.Discussion = SingleQuestion.DiscussionCategory.Name;
                mainModel.DateAsked = SingleQuestion.DateAsked.ToString("d,MMMM yy");
                mainModel.TimeAsked = SingleQuestion.DateAsked.ToString("H:mm tt");


                var answer = _applicationDbContext.AnswerToQuestions.Where(q => q.QuestionId == SingleQuestion.QuestionId)
                    .Include(c => c.AnswerComments)
                    .OrderBy(d => d.DateAnswered).ToList();

                foreach (var atq in answer)
                {
                    
                    var AUserImage = _applicationDbContext.UserProfilePhotos.SingleOrDefault(u => u.UserId == atq.UserId);
                    var lastAnswerDate = _applicationDbContext.AnswerToQuestions.OrderByDescending(d => d.DateAnswered).FirstOrDefault(id => id.QuestionId == atq.QuestionId );
                    lastAnswerToDate = lastAnswerDate.DateAnswered;

                    var atqModel = new AnswerToQuestionViewModel();

                    if (AUserImage == null)
                    {
                        atqModel.AToQuestionOwnerDP = "person.gif";
                    }
                    else
                    {
                        atqModel.AToQuestionOwnerDP = AUserImage.ImageName;
                    }
                    var atqbUser = UserManager.FindByIdAsync(atq.UserId);
                    var atqOwner = atqbUser.Result.NameExtension;

                    atqModel.AToQuestionOwner = atqOwner;
                    atqModel.AnswerToQuestionId = atq.AnswerToQuestionId;
                    atqModel.AToQuestion = atq.Answer;
                    atqModel.AToCDateCommented = atq.DateAnswered.ToString("d,MMMM yy");
                    atqModel.AToTimeCommented = atq.DateAnswered.ToString("H:mm tt");

                    
                    foreach (AnswerComment com in atq.AnswerComments.OrderBy(d => d.DateCommented))
                    {
                        var AnswerUserImage = _applicationDbContext.UserProfilePhotos.SingleOrDefault(u => u.UserId == com.UserId);
                        var lastAnswerCommentDate = _applicationDbContext.AnswerComments.OrderByDescending(d => d.DateCommented).FirstOrDefault(id => id.AnswerToQuestionId == atq.AnswerToQuestionId);
                        lastAnswerComment = lastAnswerCommentDate.DateCommented;

                        var acModel = new AnswerCommentViewModel();

                        if (AnswerUserImage == null)
                        {
                            acModel.ACommentOnwerDP = "person.gif";
                        }
                        else
                        {
                            acModel.ACommentOnwerDP = AnswerUserImage.ImageName;
                        }

                        acModel.AnswerToQuestionId = com.AnswerToQuestionId;
                        acModel.AComment = com.Comment;
                        acModel.ACDateCommented = com.DateCommented.ToString("d,MMMM yy");
                        acModel.ACTimeCommented = com.DateCommented.ToString("H:mm tt");

                        var acUser = UserManager.FindByIdAsync(com.UserId);
                        var acOwner = acUser.Result.NameExtension;
                        acModel.ACommentOwner = acOwner;



                        acList.Add(acModel);

                    }

                    atqList.Add(atqModel);
                    if (lastAnswerToDate > lastAnswerComment)
                    {
                        mainModel.LastActiveDate = lastAnswerToDate.ToString("dd,MMMM yy");
                        mainModel.LastActiveTime = lastAnswerToDate.ToString("H:mm tt");
                    }
                    if(lastAnswerToDate < lastAnswerComment)
                    {
                        mainModel.LastActiveDate = lastAnswerComment.ToString("dd,MMMM yy");
                        mainModel.LastActiveTime = lastAnswerComment.ToString("H:mm tt");
                    }

                }

                //var relQuestionByTitle = from p in 
                //         where search.Any(val => p.Description.Contains(val))
                //         select p;

                //var relQuestionByTitle = _applicationDbContext.Questions.OrderByDescending(d => d.DateAsked)
                //    .Where(i => i.Title.StartsWith(SingleQuestion.Title) || i.Title.Contains(SingleQuestion.Title)).ToList();


                IQueryable<Question> questions = _applicationDbContext.Questions.OrderByDescending(d => d.DateAsked)
                    .Take(10);
                if (!String.IsNullOrEmpty(SingleQuestion.Title))
                    questions =
                        questions.Where(
                        ii => ii.Title.Contains(SingleQuestion.Tag) || ii.QuestionText.Contains(SingleQuestion.Tag));
                
                foreach(var q in questions)
                {
                    RelatedQuestionViewModel relModel = new RelatedQuestionViewModel();

                        relModel.QuestionText = convert.Convert(q.QuestionText);
                        relModel.QuestionId = q.QuestionId;
                        relModel.QuestionTitle = q.Title;
                         
                        relQlist.Add(relModel); 

                }
                

                

                //foreach (var rel in relQuestionByTitle)
                //{

                //    RelatedQuestionViewModel relModel = new RelatedQuestionViewModel();
                //    relModel.QuestionText = convert.Convert(rel.QuestionText);

                //    var Count = relQuestionByTitle.Count;

                //    relQlist.Add(relModel);
                //}


            }
            if (lastAnswerToDate.ToString() == "1/1/0001 12:00:00 AM")
            {
                mainModel.LastActiveDate = SingleQuestion.DateAsked.ToString("dd,MMMM yy");
                mainModel.LastActiveTime = SingleQuestion.DateAsked.ToString("H:mm tt");
            }

            mainModel.AnswerToQuestionViewModels = atqList;
            mainModel.AnswerCommentViewModels = acList;
            mainModel.RelatedQuestionViewModels = relQlist;

            string loggedInUserId = User.Identity.GetUserId();
            var loggedPicture = _applicationDbContext.UserProfilePhotos.SingleOrDefault(i => i.UserId == loggedInUserId);
            if(loggedPicture == null)
            {
                ViewBag.loggedInUserImage = "person.gif";
            }
            else
            {
                ViewBag.loggedInUserImage = loggedPicture.ImageName;
            }
            if(mainModel.RelatedQuestionViewModels.Count <= 1)
            {
                mainModel.Count = 1;
            }
            else
            {
                mainModel.Count = 2;
            }
            string rawUrl = this.Request.RawUrl.ToString();
            ViewBag.absoluteUrl = "https://www.ripoff.com.ng/" + rawUrl;
            ViewBag.page = SingleQuestion.DiscussionCategoryId;
            ViewBag.TitlePage = SingleQuestion.DiscussionCategory.Description;
            ViewBag.Title = mainModel.Title ;
            return View(mainModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "QuestionId,Title,QuestionText,UserId,DiscussionCategoryId,VoteScore,isAnswered,DateAsked,NotifyMe,Tag")]Question question)
        {
            HtmlToText convert = new HtmlToText();
            ManageMessageId? message;

            string randomId = Guid.NewGuid().ToString();

            question.UserId = User.Identity.GetUserId();
            question.DateAsked = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _applicationDbContext.Questions.Add(question);
                await _applicationDbContext.SaveChangesAsync();

                int page = 0;
                page = question.QuestionId;

                string title = question.Title;
                string titleC = Regex.Replace(title, "[^A-Za-z0-9]", "-");
               

                string iD = Guid.NewGuid().ToString();
                var catName = _applicationDbContext.DiscussionCategories.Where(id => id.DiscussionCategoryId == question.DiscussionCategoryId).SingleOrDefault();
                string cat = catName.Name;
                string catC = Regex.Replace(cat, "[^A-Za-z0-9]", "-");
                string titleCat = titleC + "&" + catC;

                RealQuestionsHub.NotifyRecentQuestionToAllClients();
                //page is used instead of QuestionId
                return RedirectToAction("Thread",new {Controller = "Question", action = "Thread",page = page,title = titleCat, iD = randomId});
            }

            message = ManageMessageId.Error;
            return View(message);
        }


        #region Helpers

        public enum ManageMessageId
        {
            Error,
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