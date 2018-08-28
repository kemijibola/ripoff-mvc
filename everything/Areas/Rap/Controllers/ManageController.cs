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
using System.IO;
using System.Web.Helpers;

namespace everything.Areas.Rap.Controllers
{
    [Authorize]
    public class ManageController : ApplicationBaseController
    {
        ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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
        // GET: Admin/Users
        public async Task<ActionResult> Index(int? page, string search)
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
                    ViewBag.CurrentSearch = search;
                    var listUserRoles = new List<UserandRolesViewModel>();
                    var listUsersInRoleOnly = new List<UserInRoleModel>();
                    UserandRolesViewModel mainModel = new UserandRolesViewModel();

                    var Users = UserManager.Users;

                    foreach (var user in Users)
                    {
                        var r = new UserandRolesViewModel
                        {
                            FullName = user.FullName,
                            UserName = user.UserName,
                            Email = user.Email,
                            UserId = user.Id
                        };
                        listUserRoles.Add(r);
                    }
                    foreach (var user in listUserRoles)
                    {
                        if (!String.IsNullOrEmpty(search))
                        {
                            user.RoleNames = await UserManager.GetRolesAsync(Users.First(i => i.UserName == search).Id);
                            foreach (var role in user.RoleNames)
                            {
                                if (user.Email.Equals(search))
                                {
                                    UserInRoleModel model = new UserInRoleModel();
                                    model.Email = user.Email;
                                    model.FullName = user.FullName;
                                    model.RoleName = role;
                                    model.UserId = user.UserId;
                                    listUsersInRoleOnly.Add(model);
                                }
                                else
                                { }

                            }
                        }

                        else
                        {
                            user.RoleNames = await UserManager.GetRolesAsync(Users.First(i => i.UserName == user.UserName).Id);
                            foreach (var role in user.RoleNames)
                            {
                                if (role != null)
                                {
                                    UserInRoleModel model = new UserInRoleModel();
                                    model.Email = user.Email;
                                    model.FullName = user.FullName;
                                    model.RoleName = role;
                                    model.UserId = user.UserId;
                                    listUsersInRoleOnly.Add(model);
                                }
                            }
                        }

                    }
                    int pageSize = 50;
                    int pageNumber = page ?? 1;

                    mainModel.UserInRoleModels = listUsersInRoleOnly.ToPagedList<UserInRoleModel>(pageNumber, pageSize);


                    return View(mainModel);
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");


        }

        public async Task<ActionResult> RemoveUserFromRole(string userId, string role)
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
                    if (ModelState.IsValid)
                    {
                        var result = await UserManager.RemoveFromRolesAsync(userId, role);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index", "Manage");
                        }

                    }

                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");
        }

        public async Task<ActionResult> Edit(string id)
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
                    ViewBag.Country = _applicationDbContext.Countries.ToList();
                    ViewBag.State = _applicationDbContext.States.ToList();
                    ViewBag.City = _applicationDbContext.Cities.ToList();
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);
                    if (applicationUser == null)
                    {
                        return HttpNotFound();
                    }

                    var userRoles = await UserManager.GetRolesAsync(applicationUser.Id);
                    applicationUser.RolesList = RoleManager.Roles.ToList().Select(r => new SelectListItem
                    {
                        Selected = userRoles.Contains(r.Name),
                        Text = r.Name,
                        Value = r.Name
                    });

                    return View(applicationUser);
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser([Bind(Include = "Id,Address,Email,PhoneNumber,CountryId,StateId,CityId")]ApplicationUser applicationUser)
        {
            ApplicationUser retrievedApplicationUser = await UserManager.FindByIdAsync(applicationUser.Id);
            retrievedApplicationUser.Email = applicationUser.Email;
            retrievedApplicationUser.PhoneNumber = applicationUser.PhoneNumber;
            retrievedApplicationUser.Address = applicationUser.Address;
            retrievedApplicationUser.CityId = applicationUser.CityId;
            retrievedApplicationUser.StateId = applicationUser.StateId;
            retrievedApplicationUser.CountryId = applicationUser.CountryId;

            // Update the Profile
            var result = await UserManager.UpdateAsync(retrievedApplicationUser);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else

                return View(applicationUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id")] ApplicationUser applicationUser, params string[] rolesSelectedOnView)
        {
            if (ModelState.IsValid)
            {
                // If the user is currently stored having the Admin role,
                var rolesCurrentlyPersistedForUser = await UserManager.GetRolesAsync(applicationUser.Id);
                bool isThisUserAnAdmin = rolesCurrentlyPersistedForUser.Contains("Admin");

                // and the user did not have the Admin role checked,
                rolesSelectedOnView = rolesSelectedOnView ?? new string[] { };
                bool isThisUserAdminDeselected = !rolesSelectedOnView.Contains("Admin");

                // and the current stored count of users with the Admin role == 1,
                var role = await RoleManager.FindByNameAsync("Admin");
                bool isOnlyOneUserAnAdmin = role.Users.Count == 1;

                // (populate the roles list in case we have to return to the Edit view)
                applicationUser = await UserManager.FindByIdAsync(applicationUser.Id);
                applicationUser.RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = rolesCurrentlyPersistedForUser.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                });

                // then prevent the removal of the Admin role.
                if (isThisUserAnAdmin && isThisUserAdminDeselected && isOnlyOneUserAnAdmin)
                {
                    ModelState.AddModelError("", "At least one user must retain the Admin role; you are attempting to delete the Admin role from the last user who has been assigned to it.");
                    return View(applicationUser);
                }

                var result = await UserManager.AddToRolesAsync(
                    applicationUser.Id,
                    rolesSelectedOnView.Except(rolesCurrentlyPersistedForUser).ToArray());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View(applicationUser);
                }

                result = await UserManager.RemoveFromRolesAsync(
                    applicationUser.Id,
                    rolesCurrentlyPersistedForUser.Except(rolesSelectedOnView).ToArray());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View(applicationUser);
                }

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Something failed.");
            return View(applicationUser);
        }

        public ActionResult ChangePassword(ManageMessageId? message)
        {
            var rolesAssigneed = canLoggedInUserView();
            string[] rolesCanView = { "Admin", "Legal Team", "Customer Interface" };
            bool loggedInRoleExist = false;


            if (rolesAssigneed != null)
            {
                for (int i = 0; i < rolesCanView.Length; i++)
                {
                    var element = rolesAssigneed.Where(x => x.StartsWith(rolesCanView[i])).FirstOrDefault();
                    if (element != null)
                    {
                        loggedInRoleExist = true;
                    }
                }
                if (!loggedInRoleExist)
                {
                    return RedirectToAction("Unauthorized", "Access");
                }
                else
                {
                    ViewBag.StatusMessage =
                    message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                    : "";
                    return View();
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");
        }

        public ActionResult ChangePicture()
        {
            var rolesAssigneed = canLoggedInUserView();
            string[] rolesCanView = { "Admin", "Legal Team", "Customer Interface" };
            bool loggedInRoleExist = false;


            if (rolesAssigneed != null)
            {
                for (int i = 0; i < rolesCanView.Length; i++)
                {
                    var element = rolesAssigneed.Where(x => x.StartsWith(rolesCanView[i])).FirstOrDefault();
                    if (element != null)
                    {
                        loggedInRoleExist = true;
                    }
                }
                if (!loggedInRoleExist)
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

        [HttpPost]
        public async Task<ActionResult> ChangePicture(HttpPostedFileBase file)
        {
            UserProfilePhoto userP = null;
            string userId = User.Identity.GetUserId();
            var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", ".jpeg", ".JPG", ".GIF", ".Gif", ".gif" };
            var ext = Path.GetExtension(file.FileName);

            var retrievedUserPhoto = _applicationDbContext.UserProfilePhotos.SingleOrDefault(u => u.UserId == userId);
            if (retrievedUserPhoto == null)
            {

            }
            else
            {
                string userImageToDelete = retrievedUserPhoto.ImageName;
                string path = Request.MapPath("~/ProfilePhoto/" + userImageToDelete);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

            }

            if (file != null && allowedExtensions.Contains(ext))
            {
                userP = new UserProfilePhoto();
                string filename = null;
                //string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                //string filename = Guid.NewGuid() + file.FileName;

                WebImage img = new WebImage(file.InputStream);
                if (img.Width > 700)
                    img.Resize(700, 500);
                int length = file.FileName.IndexOf(".");
                if (length > 0)
                {
                    filename = file.FileName.Substring(0, length);
                }

                string randFileName = Guid.NewGuid() + Path.GetExtension(filename);
                //string filename = file.FileName.Substring(file.FileName.IndexOf(".") + 1).Trim();
                img.Save(Path.Combine(Server.MapPath("~/ProfilePhoto"), filename));


                if (retrievedUserPhoto == null)
                {
                    userP.UserId = User.Identity.GetUserId();
                    userP.ImageName = filename + Path.GetExtension(img.FileName);
                    userP.DateCreated = DateTime.UtcNow;

                    _applicationDbContext.UserProfilePhotos.Add(userP);
                    await _applicationDbContext.SaveChangesAsync();


                    return RedirectToAction("Dashboard", "Access");
                }
                else {
                    var userPictureToUpdate = _applicationDbContext.UserProfilePhotos
                            .Where(i => i.UserId == userId)
                            .Single();
                    if (TryUpdateModel(userPictureToUpdate, "",
                         new string[] { "UserProfilePhotoId", "UserId", "ImageName", "DateCreated" }))
                    {
                        try
                        {
                            userPictureToUpdate.ImageName = filename + Path.GetExtension(img.FileName);
                            _applicationDbContext.Entry(userPictureToUpdate).State = EntityState.Modified;
                            await _applicationDbContext.SaveChangesAsync();


                            return RedirectToAction("Dashboard", "Access");
                        }
                        catch (Exception /* dex */)
                        {
                            ViewBag.Error = "Failed to upload.Please try again";
                            return View();

                        }
                    }

                }
            }
            ViewBag.Error = "Select a valid image file";
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (ModelState.IsValid)
            {
                if (model.OldPassword.Contains(model.NewPassword))
                {
                    ViewBag.Password = "Your current password cannot be same as new password";
                    return View(model);
                }
            }

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("ChangePassword", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }
        // POST: /Account/ForgotPassword
        public async Task<ActionResult> ForgotPassword(string Email, string Name)
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
                    if (ModelState.IsValid)
                    {
                        var user = await UserManager.FindByNameAsync(Email);
                        if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                        {
                            // Don't reveal that the user does not exist or is not confirmed
                            return View("ForgotPasswordConfirmation");
                        }

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                        var getUserEmail = UserManager.FindByIdAsync(user.Id);
                        var callbackUrl = Url.Action("ResetPassword", "Manage", new { userId = user.Id, code = code , area = "Rap" }, protocol: Request.Url.Scheme);
                        string Body =
                                "<p><h3>Rip-Off NG</h3></p>" +
                                 "<p>Hi " + getUserEmail.Result.Email + ",</p>" +
                                "<p class=\"lead\">You recently asked to reset your Rip-Off NG password.</p>";

                        await UserManager.SendEmailAsync(user.Id, "Rip-Off NG | You requested a new password for your Rip-Off NG account", Body + "<a href=\"" + callbackUrl + "\">Click here to change your password</a>" + "<p></p><p><b>Didn't request this change?</b></p>");

                        return RedirectToAction("ForgotPasswordConfirmation", "Manage", new { user = user.FullName });
                    }

                    // If we got this far, something failed, redisplay form

                    return View();
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                 return RedirectToAction("Login", "Access");
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation(string user)
        {
            ViewBag.User = user;
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Manage");
            }
            if (user.DisableRP == true)
            {
                return RedirectToAction("InvalidRequest", "Manage");
            }
            else
            {
                var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Manage");
                }
                AddErrors(result);
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        public async Task<ActionResult> LockAccount([Bind(Include = "Id")] string id)
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
                    await UserManager.ResetAccessFailedCountAsync(id);
                    await UserManager.SetLockoutEndDateAsync(id, DateTime.UtcNow.AddYears(100));
                    return RedirectToAction("Index");
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");
        }

        public async Task<ActionResult> UnlockAccount([Bind(Include = "Id")] string id)
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
                    await UserManager.ResetAccessFailedCountAsync(id);
                    await UserManager.SetLockoutEndDateAsync(id, DateTime.UtcNow.AddYears(-1));
                    return RedirectToAction("Index");
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");


        }

        private IList<State> GetState(int countryId)
        {

            return _applicationDbContext.States.Where(m => m.CountryId == countryId).ToList();
        }
        private IList<City> GetCity(int stateId)
        {
            return _applicationDbContext.Cities.Where(m => m.StateId == stateId).ToList();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetStateByCountryId(string countryid)
        {
            try
            {
                var stateList = this.GetState(Convert.ToInt32(countryid));
                var stateData = stateList.Select(m => new SelectListItem()
                {
                    Text = m.Name,
                    Value = m.StateId.ToString(),

                });
                return Json(stateData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) { }

            return Json("Not Found", JsonRequestBehavior.AllowGet);
        }
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
            catch (Exception ex) { }

            return Json("Not Found", JsonRequestBehavior.AllowGet);
        }

        #region Helpers

        public enum ManageMessageId
        {  
            ChangePasswordSuccess,
            Error,

        }
        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

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