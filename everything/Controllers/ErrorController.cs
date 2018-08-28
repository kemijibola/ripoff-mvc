using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace everything.Controllers
{
    public class ErrorController : ApplicationBaseController
    {
        //  
        // GET: /Error/  

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult SearchError(string query)
        {
            ViewBag.SearchQuery = query;
            return View();
        }
    }
}