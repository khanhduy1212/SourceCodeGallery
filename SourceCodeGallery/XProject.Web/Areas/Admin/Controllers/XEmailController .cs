using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using XProject.Web.Models;
using Resources;
using XProject.Domain.Abstract;
using XProject.Domain.Entities;
using XProject.Web.Areas.Admin.Models;
using XProject.Web.Infrastructure.Filters;
using XProject.Web.Infrastructure.Utility;

namespace XProject.Web.Areas.Admin.Controllers
{
    [RequestAuthorize]
    public class XEmailController : Controller
    {
        private readonly IGeneralRepository<XEmail> _newGeneralRepository;
       

        public XEmailController(IGeneralRepository<XEmail> newGeneralRepository)
        {
            _newGeneralRepository = newGeneralRepository;
           
        }

        public ActionResult Index()
        {
        
            return View();
        }
        [AjaxOnly]
        public JsonResult IndexAjax(DataTableJS data)
        {

            int id = CurrentUser.Identity.ID;
            String search = null;
            if (data.search != null && data.search["value"] != null)
            {
                search = data.search["value"];
                search = search.UrlFrendly(Int32.MaxValue);
            }
            var column = data.order[0]["column"];
            var dir = data.order[0]["dir"];
            string columnName = ((String[])data.columns[int.Parse(column)]["data"])[0];
            var queryFilter = _newGeneralRepository.GetIQueryableItems().OrderByDescending(x => x.ID).ToList();

            queryFilter = queryFilter.Where(T => T.Active == 1 &&
                                          (search == "" || (search != null &&
                                                            (T.Email.ToLower().Contains(search.ToLower()) || T.Description.ToLower().Contains(search.ToLower()) || T.Name.ToLower().Contains(search.ToLower()) || T.Phone.ToLower().Contains(search.ToLower()))))).ToList();


            data.recordsTotal = _newGeneralRepository.GetIQueryableItems().Count(T => T.Active == 1);
            data.recordsFiltered = queryFilter.Count();
            data.data = queryFilter.Skip(data.start)
                .Take(data.length == -1 ? data.recordsTotal : data.length)
                .ToList();
            return Json(data, JsonRequestBehavior.AllowGet);

        }

      
        [HttpPost]
        [ActionName("Delete"), DisplayName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _newGeneralRepository.DeleteItem(id);
            return RedirectToAction("Index");
        }

        [ActionName("DeActive"), DisplayName("Delete")]
        public JsonResult DeActiveConfirmed(int id)
        {
            var model = _newGeneralRepository.GetItem(id);
            model.Active = 0;
            _newGeneralRepository.UpdateItem(model);
            return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create()
        {
            ViewBag.HideBase = true;
            var model = new XEmail();
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(XEmail model)
        {
            int id = CurrentUser.Identity.ID;
            string type = Request.Form["_Type"];
            if (!ModelState.IsValid)
            {
                if (type == "ajax")
                {
                    return Content(new JavaScriptSerializer().Serialize(model), "application/json");
                }
                return View(model);
            }
            
            model.CreateUser = id;
            model.CreateDate = DateTime.Now;

            String keys = model.Name.NormalizeD() + "\n" +
                      (String.IsNullOrEmpty(model.Description)
                          ? ""
                          : model.Description.NormalizeD()) + "\n" + model.Email.NormalizeD();
            model.KeySearch = keys;
            _newGeneralRepository.CreateItem(model);



            if (type == "ajax")
            {
                return Content(new JavaScriptSerializer().Serialize(model), "application/json");
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            ViewBag.HideBase = true;
            XEmail model = _newGeneralRepository.GetItem(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(XEmail model)
        {
            int id = CurrentUser.Identity.ID;

            if (!ModelState.IsValid)
            {

                ViewBag.Success = false;
                ViewBag.Message = Resource.SaveFailed;
                return View(model);
            }
           
            model.ModifiedUser = id;
            model.ModifiedDate = DateTime.Now;
            String keys = model.Name.NormalizeD() + "\n" +
                      (String.IsNullOrEmpty(model.Description)
                          ? ""
                          : model.Description.NormalizeD()) + "\n" + model.Email.NormalizeD();
            model.KeySearch = keys;
            _newGeneralRepository.UpdateItem(model);
            ViewBag.Success = true;
            ViewBag.Message = Resource.SaveSuccessful;
            ModelState.Clear();
            return RedirectToAction("Index");
        }




    }
}
