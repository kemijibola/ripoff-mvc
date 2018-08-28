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
    public class LawFirmController : ApplicationBaseController
    {
        ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        public LawFirmController()
        {
        }

        public LawFirmController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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
        // GET: Rap/LawFirm
        public ActionResult Index(int? page)
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
                    IQueryable<Lawfirm> firms = _applicationDbContext.Lawfirms
                        .OrderByDescending(n => n.FirmName);

                    int pageSize = 50;
                    int pageNumber = page ?? 1;

                    return View(firms.ToPagedList(pageNumber, pageSize));
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");
        }

        public ActionResult Create()
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
                    ViewBag.Country = _applicationDbContext.Countries.ToList();
                    ViewBag.State = _applicationDbContext.States.ToList();
                    ViewBag.City = _applicationDbContext.Cities.ToList();

                    return View();
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "LawfirmId,FirmName,HolderName,PhoneNumber,ContactPerson,ContactNumber,Email,Address,CityId,StateId,CountryId,DateRegistered")]Lawfirm firm)
        {
            ViewBag.Country = _applicationDbContext.Countries.ToList();
            ViewBag.State = _applicationDbContext.States.ToList();
            ViewBag.City = _applicationDbContext.Cities.ToList();

            firm.DateRegistered = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _applicationDbContext.Lawfirms.Add(firm);
                await _applicationDbContext.SaveChangesAsync();

                //page is used instead of QuestionId
                return RedirectToAction("Index");
            }


            return View(firm);
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
                    ViewBag.Country = _applicationDbContext.Countries.ToList();
                    ViewBag.State = _applicationDbContext.States.ToList();
                    ViewBag.City = _applicationDbContext.Cities.ToList();

                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Lawfirm firm = await _applicationDbContext.Lawfirms.FindAsync(id);
                    if (firm == null)
                    {
                        return HttpNotFound();
                    }
                    return View(firm);
                }
            }
            else

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Access");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit([Bind(Include = "LawfirmId,FirmName,HolderName,PhoneNumber,ContactPerson,ContactNumber,Email,Address,CityId,StateId,CountryId,DateRegistered")]Lawfirm firm)
        {
            ViewBag.Country = _applicationDbContext.Countries.ToList();
            ViewBag.State = _applicationDbContext.States.ToList();
            ViewBag.City = _applicationDbContext.Cities.ToList();
            if (ModelState.IsValid)
            {
                _applicationDbContext.Entry(firm).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }


            return View(firm);
        }
        public ActionResult DisplayFirmDetails(int page)
        {
            var firmDetailsById = _applicationDbContext.Lawfirms.Include("City").Include("State").Include("Country")
                .Where(m => m.LawfirmId == page).SingleOrDefault();
            if (firmDetailsById != null)
            {
                LawFirmViewModel firmModel = new LawFirmViewModel();
                firmModel.FirmName = firmDetailsById.FirmName;
                firmModel.HolderName = firmDetailsById.HolderName;
                firmModel.PhoneNumber = firmDetailsById.PhoneNumber;
                firmModel.ContactPerson = firmDetailsById.ContactPerson;
                firmModel.ContactNumber = firmDetailsById.ContactNumber;
                firmModel.Email = firmDetailsById.Email;
                firmModel.Address = firmDetailsById.Address;
                firmModel.City = firmDetailsById.City.Name;
                firmModel.State = firmDetailsById.State.Name;
                firmModel.Country = firmDetailsById.Country.Name;

                return PartialView("_Details", firmModel);
            }

            return RedirectToAction("Index");
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