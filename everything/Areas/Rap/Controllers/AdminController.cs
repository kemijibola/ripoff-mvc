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

namespace everything.Areas.Rap.Controllers
{
    [Authorize]
    public class AdminController : ApplicationBaseController
    {
        ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        public AdminController()
        {
        }

        public AdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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

        public async Task<ActionResult> Index()
        {
            // ApplicationRole applicationRole = await RoleManager.FindByIdAsync()
            return View(await RoleManager.Roles.ToListAsync());
        }


        // GET: ApplicationRoles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole applicationRole = await RoleManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }
            return View(applicationRole);
        }

        // GET: ApplicationRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<JsonResult> EmailAlreadyExistsAsync(string email)
        {
            var user = await this.UserManager.FindByEmailAsync(email);

            return Json(user == null, JsonRequestBehavior.AllowGet);


        }

        [AllowAnonymous]
        public JsonResult NameAlreadyExistsAsync(string nameextension)
        {
            var user = _applicationDbContext.Users.SingleOrDefault(u => u.NameExtension == nameextension);

            return Json(user == null, JsonRequestBehavior.AllowGet);

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
        public ActionResult Register()
        {
            var rolesAssigneed = canLoggedInUserView();
            string roleCanView = "Admin";

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
        public async Task<ActionResult> UserRole(string page)
        {

            var rolesAssigneed = canLoggedInUserView();
            string roleCanView = "Admin";

            if (rolesAssigneed != null)
            {
                var element = rolesAssigneed.Where(x => x.StartsWith(roleCanView)).FirstOrDefault();
                if (element != roleCanView)
                {
                    return RedirectToAction("Unauthorized", "Access");
                }
                else
                {
                    AddUserToRoleModel mainModel = new AddUserToRoleModel();
                    var roleList = new List<ExistingRole>();

                    var UserToAssignRole = _applicationDbContext.Users.SingleOrDefault(i => i.Id == page);
                    string FullName = UserToAssignRole.FullName;
                    ViewBag.FullName = FullName;

                    var roleModel = await RoleManager.Roles.ToListAsync();

                    foreach (var role in roleModel)
                    {
                        ExistingRole model = new ExistingRole();
                        model.Id = role.Id;
                        model.Name = role.Name;
                        model.Checked = false;
                        roleList.Add(model);
                    }
                    mainModel.UserId = page;
                    mainModel.ExistingRole = roleList;
                    return View(mainModel);
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserRole(AddUserToRoleModel model)
        {
            foreach (var allRoles in model.ExistingRole)
            {
                if (allRoles.Checked == true)
                {
                    string[] roles = new string[] { allRoles.Name };
                    foreach (var role in roles)
                    {
                        var isInRole = await UserManager.IsInRoleAsync(model.UserId, role);

                        if (!isInRole)
                        {
                            await UserManager.AddToRoleAsync(model.UserId, role);

                        }
                        else
                        {
                            ModelState.AddModelError("", role + "has already been assigned to user");
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Manage");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                int flength = model.NameExtension.IndexOf("_");

                string firstname = null;
                if (flength > 0)
                {
                    firstname = model.NameExtension.Substring(0, flength);
                }
                int index = model.NameExtension.IndexOf("_");
                string lastname = model.NameExtension.Substring(index + 1);
                var user = new ApplicationUser
                {
                    NameExtension = model.NameExtension,
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = firstname,
                    LastName = lastname,
                    CountryId = model.CountryId,
                    IsInterestedInLawyer = model.IsInterestedInLawyer,
                    DateRegistered = DateTime.UtcNow,
                    Career = "Admin",
                    EmailConfirmed = true,
                    DisableRP = false

                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    // AddErrors(result);
                }


                await UserManager.SetTwoFactorEnabledAsync(user.Id, false);

                if (result.Succeeded)
                {
                    string iD = Guid.NewGuid().ToString();
                    return RedirectToAction("UserRole", new { Controller = "Admin", action = "UserRole", page = user.Id, id = iD });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form

            return View(model);
        }
        // POST: ApplicationRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name")] ApplicationRoleViewModel applicationRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole applicationRole = new ApplicationRole { Name = applicationRoleViewModel.Name };

                var roleResult = await RoleManager.CreateAsync(applicationRole);
                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError("", roleResult.Errors.First());
                    return View();
                }

                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: ApplicationRoles/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole applicationRole = await RoleManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }
            ApplicationRoleViewModel applicationRoleViewModel = new ApplicationRoleViewModel
            {
                Id = applicationRole.Id,
                Name = applicationRole.Name
            };
            return View(applicationRoleViewModel);
        }

        // POST: ApplicationRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] ApplicationRoleViewModel applicationRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole applicationRole = await RoleManager.FindByIdAsync(applicationRoleViewModel.Id);
                string originalName = applicationRole.Name;

                if (originalName == "Admin" && applicationRoleViewModel.Name != "Admin")
                {
                    ModelState.AddModelError("", "You cannot change the name of the Admin role.");
                    return View(applicationRoleViewModel);
                }

                if (originalName != "Admin" && applicationRoleViewModel.Name == "Admin")
                {
                    ModelState.AddModelError("", "You cannot change the name of a role to Admin.");
                    return View(applicationRoleViewModel);
                }

                applicationRole.Name = applicationRoleViewModel.Name;
                await RoleManager.UpdateAsync(applicationRole);

                return RedirectToAction("Index");
            }
            return View(applicationRoleViewModel);
        }

        // GET: ApplicationRoles/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole applicationRole = await RoleManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }
            return View(applicationRole);
        }

        // POST: ApplicationRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ApplicationRole applicationRole = await RoleManager.FindByIdAsync(id);

            if (applicationRole.Name == "Admin")
            {
                ModelState.AddModelError("", "You cannot delete the Admin role.");
                return View(applicationRole);
            }

            await RoleManager.DeleteAsync(applicationRole);
            return RedirectToAction("Index");
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
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