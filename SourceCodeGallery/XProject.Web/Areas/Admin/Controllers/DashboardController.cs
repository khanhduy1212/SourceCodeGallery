using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using CsQuery.ExtensionMethods;
using XProject.Domain.Abstract;
using XProject.Domain.Entities;
using XProject.Web.Areas.Admin.Models;
using XProject.Web.Infrastructure;
using XProject.Web.Infrastructure.Filters;
using XProject.Web.Infrastructure.Helpers;
using XProject.Web.Infrastructure.Utility;
using NS.Mail;
using StringHelper = XProject.Domain.Helpers.StringHelper;

namespace XProject.Web.Areas.Admin.Controllers
{
    [RequestAuthorize]
    public class DashboardController : Controller
    {

        [DisplayName(@"Dashboard")]
        public ActionResult Index()
        {
           
            return View();
        }

        [DisplayName(@"Set language")]
        public ActionResult SetLanguage(string cult, string returnUrl)
        {
            var accountService = DependencyHelper.GetService<IMembershipService>();
            var unitService = DependencyHelper.GetService<IUnitRepository>();
            var language = unitService.GetLanguage(cult);
            CurrentUser.Identity.LanguageID = language.ID;
            CurrentUser.Identity.Language = language;
            accountService.UpdateUserLanguage(CurrentUser.Identity.ID, language.ID);
            //if (HttpContext.Request.Url != null)
            //{
            //    var url = HttpContext.Request.Url.LocalPath;
            //    return RedirectToLocal(url);
            //}
            return RedirectToLocal(returnUrl);
        }

        [AjaxOnly]
        private RedirectResult RedirectToLocal(string returnUrl)
        {
            return Redirect(Url.IsLocalUrl(returnUrl)
                                ? returnUrl
                                : Url.Action("Index", "Home", new { area = "" }));
        }

        [AjaxOnly]
        public ActionResult GetOnOffSession()
        {
            var r = System.Web.HttpContext.Current.Session.SessionID + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [AjaxOnly]
        public ActionResult GetToken(string user, string pass, string session)
        {
            var token = StringHelper.GetToken(session, pass, user);
            return Json(token, JsonRequestBehavior.AllowGet);
        }

        

    }
}
