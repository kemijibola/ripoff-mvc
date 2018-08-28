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

namespace everything.Controllers
{
    public class CommentController : ApplicationBaseController
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]

        public async Task<ActionResult> Create([Bind(Include = "CommentId,ThreadId,UserId,CommentText,DateCreated")]CommentReplyViewModel comment)
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
                
            }

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