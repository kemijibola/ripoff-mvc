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
using System.Diagnostics;
namespace everything.Controllers
{
    public class SearchWidgetController : ApplicationBaseController
    {
        public SearchWidgetController()
        {
        }
        public SearchWidgetController(ApplicationUserManager userManager)
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
        ApplicationDbContext _applicationDbContext = new ApplicationDbContext();


        // GET: SearchWidget
        
        [AllowAnonymous]
   
        
        public ActionResult Index()
        {
            return PartialView("_Search");
        }

        [AllowAnonymous]
        public ActionResult SearchIndex(string sort, string keyword, int? page)
        {

            int ReportId = 0;
            string s = keyword;
            int result;

            if (int.TryParse(s, out result))
            {
                ReportId = Convert.ToInt32(keyword);
            }
            else { }
            //sort by state and city
            ViewBag.NewestSort = sort == "newestsearch" ? "newest_desc" : "newest_search";
            ViewBag.OldestSort = sort == "oldestsearch" ? "oldest_asec" : "oldest_search";

            ViewBag.CurrentSort = sort;
            ViewBag.CurrentSearch = keyword;

            IQueryable<Report> Reports = _applicationDbContext.Reports.Include(c => c.Category).Include(t => t.Topic).Include(ci => ci.City).Include(st => st.State);


            if (!String.IsNullOrEmpty(keyword))
            {
                Reports = Reports

                    .Where(d => d.CompanyorIndividual.ToLower().Contains(keyword) || d.ReportId == ReportId ||
                    d.ReportText.ToLower().Contains(keyword) || d.Website.ToLower().Contains(keyword)
                    || d.Address.ToLower().Contains(keyword) || d.Category.Name.ToLower().Contains(keyword));
            }
            if(Reports.ToList().Count < 1)
            {

                    sort = "empty_search";

            }
           // if(Reports = null)
            switch (sort)
            {
                case "newest_search":
                    Reports =
                        Reports
                            .OrderByDescending(r => r.DateCreated)
                            .ThenBy(r => r.CompanyorIndividual);
                    break;
                case "oldest_search":
                    Reports =
                        Reports
                        .OrderBy(r => r.ReportId);
                    break;
                case "empty_search":
                    return RedirectToAction("SearchError", new { Controller = "Error", action = "SearchError", query = keyword });
                   // break;
                default:
                    Reports =
                        Reports
                            .OrderBy(ii => ii.Category.Name);
                    break;

            }
            int pageSize = 25;
            int pageNumber = page ?? 1;
            return View(Reports.ToPagedList(pageNumber, pageSize));


        }

        [AllowAnonymous]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Index(string keyword)
        {
            HtmlToText convert = new HtmlToText();
            int ReportId = 0;
            string s = keyword;
            var searchResultList = new List<SearchResultViewModel>();
            int result;

            if (int.TryParse(s, out result))
            {
                ReportId = Convert.ToInt32(keyword);
            }
            else { }

            IQueryable<Report> Reports = _applicationDbContext.Reports.Include(c => c.Category).Include(t => t.Topic).Include(ci => ci.City).Include(st => st.State);


            if (!String.IsNullOrEmpty(keyword))
            {
                Reports = Reports

                    .Where(d => d.CompanyorIndividual.ToLower().Contains(keyword) || d.ReportId == ReportId ||
                    d.ReportText.ToLower().Contains(keyword) || d.Website.ToLower().Contains(keyword)
                    || d.Address.ToLower().Contains(keyword) || d.Category.Name.ToLower().Contains(keyword)).OrderByDescending(d => d.DateCreated);
            }

            foreach(var report in Reports)
            {
                SearchResultViewModel model = new SearchResultViewModel();

                string title = report.CompanyorIndividual + ":" + convert.Convert(report.ReportText.Substring(0, 90));
                string titleC = Regex.Replace(title, "[^A-Za-z0-9]", "-");

                model.iD = Guid.NewGuid().ToString();
                model.page = report.ReportId;
                model.title = titleC;
                model.company = report.CompanyorIndividual;
                model.Topic = report.Topic.Name;
                model.Category = report.Category.Name;
                searchResultList.Add(model);
            }
            
            return Json(searchResultList, JsonRequestBehavior.AllowGet);
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