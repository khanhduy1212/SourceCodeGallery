using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using XProject.Domain.Abstract;
using XProject.Web.Areas.Admin.Models;

using XProject.Web.Infrastructure.Filters;

namespace XProject.Web.Areas.Admin.Controllers
{
    [RequestAuthorize]
    public class SettingsController : Controller
    {
        private readonly ISettingRepository _settingRepo;
        public static readonly List<string> LoginRequired = new List<string> { "Home" };
        public SettingsController(ISettingRepository settingRepo)
        {
            _settingRepo = settingRepo;
        }

        //
        // GET: /Settings/
        [DisplayName(@"Setting management")]
        public ViewResult Index()
        {
            return View(_settingRepo.GetSettings());
        }

        [HttpPost]
        [DisplayName(@"Setting management")]
        public ActionResult Index(SettingModel setting)
        {
            if (ModelState.IsValid)
            {
                var lstId = Request["Id"].Split(',');
                foreach (var id in lstId)
                {
                    var item = _settingRepo.GetSetting(Convert.ToInt32(id));
                    item.Value = Request[item.Name].Split(',')[0];
                    _settingRepo.UpdateSetting(item);
                }
            }

            return View(_settingRepo.GetSettings());
        }
    }
}
