using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using everything.Models;
using everything.DataLayer;
using System.Collections.Generic;
using everything.ViewModels;
using System.IO;
using System.Web.Helpers;
using System.Data.Entity;

namespace everything.Controllers
{
    [Authorize]
    public class ProfileController : ApplicationBaseController
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();

        public ProfileController()
        {
        }

        public ProfileController(ApplicationUserManager userManager)
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

        public async Task<ActionResult> ChangeProfile()
        {
            var UserId = User.Identity.GetUserId();
            var loggedUserByUserId = _applicationDbContext.Users.SingleOrDefault(i => i.Id == UserId);
            if (loggedUserByUserId.Career == "General")
            {
                EditProfileViewModel profileModel = new EditProfileViewModel();
                ApplicationUser applicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                ViewBag.Country = _applicationDbContext.Countries.ToList();
                ViewBag.State = _applicationDbContext.States.ToList();
                ViewBag.City = _applicationDbContext.Cities.ToList();
                var UserProfileImage = _applicationDbContext.UserProfilePhotos.SingleOrDefault(id => id.UserId == applicationUser.Id);

                List<Country> countryList = new List<Country>();
                if (applicationUser != null)
                {

                    profileModel.FirstName = applicationUser.FirstName;
                    profileModel.LastName = applicationUser.LastName;
                    profileModel.Email = applicationUser.Email;
                    profileModel.Address = applicationUser.Address;
                    profileModel.PhoneNumber = applicationUser.PhoneNumber;
                    if (UserProfileImage == null)
                    {
                        profileModel.ImageName = "person.gif";
                    }
                    else
                    {
                        profileModel.ImageName = UserProfileImage.ImageName;
                    }

                    var Country = _applicationDbContext.Countries.Where(i => i.CountryId == applicationUser.CountryId).SingleOrDefault();
                    var State = _applicationDbContext.States.Where(i => i.StateId == applicationUser.StateId).SingleOrDefault();
                    var City = _applicationDbContext.Cities.Where(i => i.CityId == applicationUser.CityId).SingleOrDefault();
                    try
                    {
                        if (Country.Name == null)
                        {
                            profileModel.CountryName = "None";

                        }
                        else { profileModel.CountryName = Country.Name; }
                        if (State.Name == null)
                        {
                            profileModel.StateName = "None";

                        }
                        else { profileModel.StateName = State.Name; }
                        if (City.Name == null)
                        {
                            profileModel.CityName = "None";

                        }
                        else { profileModel.CityName = City.Name; }
                    }
                    catch (Exception ex) { }

                    return View(profileModel);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            else
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeProfile(ApplicationUser applicationUser)
        {
            ManageMessageId manageMessageId;

            ApplicationUser retrievedApplicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            retrievedApplicationUser.Email = applicationUser.Email;
            retrievedApplicationUser.FirstName = applicationUser.FirstName;
            retrievedApplicationUser.LastName = applicationUser.LastName;
            retrievedApplicationUser.PhoneNumber = applicationUser.PhoneNumber;
            retrievedApplicationUser.Address = applicationUser.Address;
            retrievedApplicationUser.CityId = applicationUser.CityId;
            retrievedApplicationUser.StateId = applicationUser.StateId;
            retrievedApplicationUser.CountryId = applicationUser.CountryId;

            // Update the Profile
            var result = await UserManager.UpdateAsync(retrievedApplicationUser);
            if (result.Succeeded)
            {
                manageMessageId = ManageMessageId.ChangeProfileSuccess;

                // If the Email address changed, sync both Email and UserName to it
                if (retrievedApplicationUser.Email != applicationUser.Email)
                {
                    // Update the UserName
                    string previousUserName = retrievedApplicationUser.UserName;
                    retrievedApplicationUser.UserName = applicationUser.Email;
                    result = await UserManager.UpdateAsync(retrievedApplicationUser);
                    if (result.Succeeded)
                    {
                        // Update the Email address
                        result = await UserManager.SetEmailAsync(retrievedApplicationUser.Id, applicationUser.Email);
                        if (result.Succeeded)
                        {
                            manageMessageId = ManageMessageId.ChangeEmailSuccess;

                            string code = await UserManager.GenerateEmailConfirmationTokenAsync(retrievedApplicationUser.Id);

                            var callbackUrl = Url.Action(
                                "ConfirmEmail",
                                "Account",
                                new { userId = retrievedApplicationUser.Id, code = code }, protocol: Request.Url.Scheme);

                            await UserManager.SendEmailAsync(
                                retrievedApplicationUser.Id,
                                "Confirm your account",
                                "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                        }
                        else
                        {
                            retrievedApplicationUser.UserName = previousUserName;
                            result = await UserManager.UpdateAsync(retrievedApplicationUser);
                            manageMessageId = ManageMessageId.Error;
                        }
                    }
                    else
                    {
                        retrievedApplicationUser.UserName = previousUserName;
                        result = await UserManager.UpdateAsync(retrievedApplicationUser);
                        manageMessageId = ManageMessageId.Error;
                    }
                }
            }
            else
                manageMessageId = ManageMessageId.Error;

            return RedirectToAction("Index", new { Message = manageMessageId });
        }


        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            string userDP = null;
            string UserId = User.Identity.GetUserId();
            var loggedUserByUserId = _applicationDbContext.Users.SingleOrDefault(i => i.Id == UserId);
            if (loggedUserByUserId.Career == "General")
            {
                ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.AddProfileImage ? "Your profile picture was updated."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : message == ManageMessageId.RemoveProfileImage ? "Your profile picture was removed."
                : message == ManageMessageId.ChangeProfileSuccess ? "Your profile has been changed."
                : message == ManageMessageId.ChangeEmailSuccess ? "Check your email."
                : "";


                var UserImage = _applicationDbContext.UserProfilePhotos.SingleOrDefault(u => u.UserId == UserId);

                if (UserImage == null)
                {
                    userDP = "person.gif";
                }
                else
                {
                    userDP = UserImage.ImageName;
                }
                var model = new IndexViewModel
                {

                    HasPassword = HasPassword(),
                    ProfilePhoto = userDP,
                    PhoneNumber = await UserManager.GetPhoneNumberAsync(User.Identity.GetUserId()),
                    TwoFactor = await UserManager.GetTwoFactorEnabledAsync(User.Identity.GetUserId()),
                    Logins = await UserManager.GetLoginsAsync(User.Identity.GetUserId()),
                    BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(User.Identity.GetUserId())
                };
                return View(model);
            }
            else
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }
        }

        //
        // GET: /Manage/RemoveLogin
        public ActionResult RemoveLogin()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return View(linkedAccounts);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            var UserId = User.Identity.GetUserId();
            var loggedUserByUserId = _applicationDbContext.Users.SingleOrDefault(i => i.Id == UserId);
            if (loggedUserByUserId.Career == "General")
            {
                return View();
            }
            else
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult ChangeProfilePicture()
        {
            var UserId = User.Identity.GetUserId();
            var loggedUserByUserId = _applicationDbContext.Users.SingleOrDefault(i => i.Id == UserId);
            if (loggedUserByUserId.Career == "General")
            {
                return View();
            }
            else
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeProfilePicture(HttpPostedFileBase file)
        {
            UserProfilePhoto userP = null;
            string userId = User.Identity.GetUserId();
            var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", ".jpeg", ".JPG", ".GIF", ".Gif", ".gif" };
            var ext = Path.GetExtension(file.FileName);

            var retrievedUserPhoto = _applicationDbContext.UserProfilePhotos.SingleOrDefault(u => u.UserId == userId);
            if(retrievedUserPhoto == null)
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
                if (img.Width > 120)
                    img.Resize(120, 44);
                int length = file.FileName.IndexOf(".");
                if(length > 0)
                {
                    filename = file.FileName.Substring(0, length);      
                }

                string randFileName = Guid.NewGuid() + Path.GetExtension(filename);
                //string filename = file.FileName.Substring(file.FileName.IndexOf(".") + 1).Trim();
                img.Save(Path.Combine(Server.MapPath("~/ProfilePhoto"), filename));

                    
                if(retrievedUserPhoto == null)
                {
                    userP.UserId = User.Identity.GetUserId();
                    userP.ImageName =filename+ Path.GetExtension(img.FileName);
                    userP.DateCreated = DateTime.UtcNow;

                    _applicationDbContext.UserProfilePhotos.Add(userP);
                    await _applicationDbContext.SaveChangesAsync();

                    return RedirectToAction("Index", new { Message = ManageMessageId.AddProfileImage });
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

                            return RedirectToAction("Index", new { Message = ManageMessageId.AddProfileImage });
                        }
                        catch (Exception /* dex */)
                        {
                            //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, do contact us.");
                        }
                    }

                }
                

            }
            ModelState.AddModelError("", "Failed upload valid photo");
            return View();

        }
        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.EmailService != null)
            {
                var Email = User.Identity.GetUserName();
                var message = new IdentityMessage
                { 
                    Destination = Email,
                    Body = "Your security code is: " + code
                };
                await UserManager.EmailService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        //[HttpPost]
        //public async Task<ActionResult> EnableTwoFactorAuthentication()
        //{
        //    await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
        //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //    if (user != null)
        //    {
        //        await SignInAsync(user, isPersistent: false);
        //    }
        //    return RedirectToAction("Index", "Manage");
        //}

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        //[HttpPost]
        //public async Task<ActionResult> DisableTwoFactorAuthentication()
        //{
        //    await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
        //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //    if (user != null)
        //    {
        //        await SignInAsync(user, isPersistent: false);
        //    }
        //    return RedirectToAction("Index", "Manage");
        //}

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // GET: /Manage/RemovePhoneNumber
        
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var UserId = User.Identity.GetUserId();
            var loggedUserByUserId = _applicationDbContext.Users.SingleOrDefault(i => i.Id == UserId);
            if (loggedUserByUserId.Career == "General")
            {
                var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
                if (!result.Succeeded)
                {
                    return RedirectToAction("Index", new { Message = ManageMessageId.Error });
                }
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
            }
            else
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }
        }

        public async Task<ActionResult> RemoveProfilePicture()
        {
            var UserId = User.Identity.GetUserId();
            var loggedUserByUserId = _applicationDbContext.Users.SingleOrDefault(i => i.Id == UserId);
            if (loggedUserByUserId.Career == "General")
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    UserProfilePhoto userP = _applicationDbContext.UserProfilePhotos.SingleOrDefault(Id => Id.UserId == user.Id);
                    // int Id = Convert.ToInt32(userP.UserProfilePhotoId);
                    _applicationDbContext.UserProfilePhotos.Remove(userP);
                    await _applicationDbContext.SaveChangesAsync();
                }
                else
                {
                    return RedirectToAction("Index", new { Message = ManageMessageId.Error });
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.RemoveProfileImage });
            }
            else
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }
        }
        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            var UserId = User.Identity.GetUserId();
            var loggedUserByUserId = _applicationDbContext.Users.SingleOrDefault(i => i.Id == UserId);
            if (loggedUserByUserId.Career == "General")
            {
                return View();
            }
            else
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }
        }

        //
        // POST: /Manage/ChangePassword
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
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            var UserId = User.Identity.GetUserId();
            var loggedUserByUserId = _applicationDbContext.Users.SingleOrDefault(i => i.Id == UserId);
            if (loggedUserByUserId.Career == "General")
            {
                return View();
            }
            else
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInAsync(user, isPersistent: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
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

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }
        private bool ProfilePhoto()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                var userImage = _applicationDbContext.UserProfilePhotos.Where(id => id.UserId == user.Id);

                if(userImage != null)
                {
                    return userImage != null;
                }

                return false;

            }
            return false;
        }
        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error,
            ChangeProfileSuccess,
            ChangeEmailSuccess,
            AddProfileImage,
            RemoveProfileImage
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