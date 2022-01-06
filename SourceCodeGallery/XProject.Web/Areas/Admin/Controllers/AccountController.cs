using System.ComponentModel;
using System.Web.Mvc;
using System.Web.Security;
using XProject.Domain.Abstract;
using XProject.Domain.Concrete;
using XProject.Domain.Helpers;
using XProject.Web.Areas.Admin.Models;
using XProject.Web.Infrastructure.Filters;
using XProject.Web.Infrastructure.Helpers;
using XProject.Web.Infrastructure.Utility;

using Resources;

namespace XProject.Web.Areas.Admin.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly ILoginTracker _loginTracker;
        private readonly IMembershipService _membership;

        public AccountController(IMembershipService membership, ILoginTracker loginTracker, IUnitRepository unitRepo)
        {
            _membership = membership;
            _loginTracker = loginTracker;
        }

        //public ActionResult Index()
        //{
        //    return RedirectToAction("Login");
        //}

        private RedirectResult RedirectToLocal(string returnUrl)
        {
            return Redirect(Url.IsLocalUrl(returnUrl)
                                ? returnUrl
                                : Url.Action("Index", "Dashboard"));
        }

        public ActionResult Login(string returnUrl)
        {
            //Session["DbName"] = null;

            //CurrentUser.Logout();
            if (CurrentUser.IsAuthenticated)
            {
                return Redirect(returnUrl);
            }
            ViewBag.returnUrl = returnUrl;
            var model = new LoginViewModel()
                        {
                            RememberMe = true,
                        };
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    if (!CurrentUser.Login(model.Email, model.Password, model.RememberMe))
                    {
                        ModelState.AddModelError("", Resource.InvalidEmailOrPassword);
                    }
                    else
                    {
                        MoneyHelper.SetDefaultCurrency();
                        return RedirectToLocal(returnUrl);
                    }
                }
                catch (ConcurrentLoginLimitException ex)
                {
                    ModelState.AddModelError("", "Your account is currently logged in somewhere else. Please try again later!");
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        public ActionResult Logout(string returnUrl = "/admin")
        {
            //Session["DbName"] = null;
            //if (Request.Cookies["DatabaseName"] != null)
            //{
            //    var cookie = Request.Cookies["DatabaseName"];
            //    cookie.Value = null;
            //    Response.Cookies.Set(cookie);
            //}

            CurrentUser.Logout();
            //var cookie = Request.Cookies["DatabaseName"];
            //cookie.Expires = DateTime.Now.AddDays(-1);
            //Response.Cookies.Set(cookie);
            return RedirectToAction("Login", "Account", new { returnUrl });
        }

        [RequestAuthorize]
        [ActionName("Profile")]
        [DisplayName("Edit Profile")]
        public ActionResult EditProfile()
        {
            var model = new AccountModel(CurrentUser.Identity);
            return View(model);
        }

        [RequestAuthorize]
        [HttpPost]
        [ActionName("Profile")]
        [DisplayName("Edit Profile")]
        public ActionResult EditProfile(AccountModel model)
        {
            var oldPassword = EncryptHelper.EncryptPassword(model.OldPassword);
            if (oldPassword != CurrentUser.Identity.Password)
            {
                ModelState.AddModelError("OldPassword", Resource.TheOldPasswordDoNotMatch);
            }
            if (ModelState.IsValid)
            {
                // Update UserLogin profile picture
                if (model.Picture != null && model.Picture.ContentLength > 0)
                {
                    UserPicture.Delete(CurrentUser.Identity.ID, CurrentUser.Identity.Picture);
                    string pictureFileName = UserPicture.Upload(CurrentUser.Identity.ID, model.Picture);
                    CurrentUser.Identity.Picture = pictureFileName;
                }

                // Update UserLogin primitive info
                //CurrentUser.Identity.Password = model.Password;
                CurrentUser.Identity.DisplayName = model.Username;
                CurrentUser.Identity.Email = model.Email;
                CurrentUser.Identity.Phone = model.Phone;
                CurrentUser.Identity.MobilePhone = model.MobilePhone;
                if (!string.IsNullOrEmpty(model.Password))
                {
                    CurrentUser.Identity.Password = EncryptHelper.EncryptPassword(model.Password);
                }
                if (_membership.UpdateUser(CurrentUser.Identity))
                    FormsAuthentication.SetAuthCookie(CurrentUser.Identity.Email, false);

                _loginTracker.ReloadUser(CurrentUser.Identity.Email, CurrentUser.Identity);


                ViewBag.Success = true;
                ViewBag.Message = Resource.YourProfileHasBeenUpdated;
                return EditProfile();
            }

            return View(model);
        }

        [RequestAuthorize]
        public ActionResult BypassLogin(string token)
        {
            return Redirect(BypassAuth.Decrypt(token));
        }
    }
}
