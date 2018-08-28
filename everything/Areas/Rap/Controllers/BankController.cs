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
    public class BankController : ApplicationBaseController
    {
        ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        public BankController()
        {
        }

        public BankController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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
        // GET: Rap/Bank
        public ActionResult Index()
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
                    IQueryable<Bank> banks = _applicationDbContext.Banks.OrderBy(n => n.Name);
                    return View(banks);
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name")] Bank bank)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Banks.Add(bank);
                await _applicationDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Unable to add request");
            return View();
        }

        public async Task<ActionResult> Edit(int? id)
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
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Bank bank = await _applicationDbContext.Banks.FindAsync(id);
                    if (bank == null)
                    {
                        return HttpNotFound();
                    }
                    return View(bank);
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BankId,Name")] Bank bank)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Entry(bank).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(bank);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(Bank id)
        {
            Bank bank = await _applicationDbContext.Banks.FindAsync(id.BankId);
            _applicationDbContext.Banks.Remove(bank);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
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