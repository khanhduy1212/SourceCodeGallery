using System;
using System.ComponentModel;
using System.Linq;
using System.Web;
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
using XProject.Domain.Entities;
using System.Web.Script.Serialization;
using XProject.Web.Models;

namespace XProject.Web.Areas.Admin.Controllers
{
    
    public class XCountryController : Controller
    {
        //
        // GET: /Admin/BDSNews/
        private readonly IGeneralRepository<XCountry> _CountryRepository;


        public XCountryController(IGeneralRepository<XCountry> CountryRepository)
        {
            _CountryRepository = CountryRepository;

        }

        public ActionResult Index()
        {


            return View();
        }
        [AjaxOnly]
        public JsonResult IndexAjax(DataTableJS data)
        {

            var itmes = _CountryRepository.GetIQueryableItems().Where(x => x.Active == 1).ToList(); ;
            String search = null;
            if (data.search != null && data.search["value"] != null)
            {
                search = data.search["value"];
            }
            var column = data.order[0]["column"];
            var dir = data.order[0]["dir"];
            string columnName = ((String[])data.columns[int.Parse(column)]["data"])[0];
            var queryFilter =
                _CountryRepository.GetIQueryableItems()
                    .Where(
                        T => T.Active == 1 &&
                             (search == "" || (search != null &&
                                               (T.KeySearch.ToLower().Contains(search.ToLower())))));
            if (dir == "asc")
            {
                queryFilter = queryFilter.OrderByField(columnName, true);
            }
            else
            {
                queryFilter = queryFilter.OrderByField(columnName, false);
            }
            data.recordsTotal = _CountryRepository.GetIQueryableItems().Count(T => T.Active == 1);
            data.recordsFiltered = queryFilter.Count();
            data.data = queryFilter.Skip(data.start)
                .Take(data.length == -1 ? data.recordsTotal : data.length)
                .ToList();
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Create()
        {
            ViewBag.HideBase = true;
            var count = _CountryRepository.GetIQueryableItems().Count(T => T.Active == 1);
            var model = new XCountry();
            model.Order = count + 1;
          
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(XCountry model, HttpPostedFileBase[] files)
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
            if (Request.Files.Count > 0)
            {
                foreach (String item in Request.Files.AllKeys)
                {
                    string prop = item.Replace("_input", "");
                    if (model.GetType().GetProperties().Any(t => t.Name == prop))
                    {
                        if (Request.Files[item] != null && Request.Files[item].ContentLength > 0)
                        {

                            byte[] binaryData;
                            binaryData = new Byte[Request.Files[item].InputStream.Length];
                            Request.Files[item].InputStream.Read(binaryData, 0, (int)Request.Files[item].InputStream.Length);
                            Request.Files[item].InputStream.Close();
                            string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
                            var pic = new PictureModel
                            {
                                PersonalId = 0,
                                cfile = base64String,
                                typeId = 1,
                                NameTypeUpload = "SizeImageSlider"

                            };
                            var fileName = Infrastructure.Helpers.ImageHelper.SaveImgToString(pic);
                             model.GetType().GetProperties().FirstOrDefault(t => t.Name == prop)?.SetValue(model, fileName.XPicture.ConvertedFilename, null);
                        }

                    }

                }
            }
            model.CreateUser = id;
            model.CreateDate = DateTime.Now;
       
            String keys= model.Name.NormalizeD() + "\n" +
                      (String.IsNullOrEmpty(model.Description)
                          ? ""
                          : model.Description.NormalizeD()) + "\n" + model.Code.NormalizeD();
            model.KeySearch = keys;
            _CountryRepository.CreateItem(model);

        

            if (type == "ajax")
            {
                return Content(new JavaScriptSerializer().Serialize(model), "application/json");
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            ViewBag.HideBase = true;
            XCountry model = _CountryRepository.GetItem(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(XCountry model, HttpPostedFileBase[] files)
        {
            int id = CurrentUser.Identity.ID;

            if (!ModelState.IsValid)
            {

                ViewBag.Success = false;
                ViewBag.Message = Resource.SaveFailed;
                return View(model);
            }
            if (Request.Files.Count > 0)
            {
                foreach (String item in Request.Files.AllKeys)
                {
                    string prop = item.Replace("_input", "");
                    if (model.GetType().GetProperties().Any(t => t.Name == prop))
                    {
                        if (Request.Files[item] != null && Request.Files[item].ContentLength > 0)
                        {

                            byte[] binaryData;
                            binaryData = new Byte[Request.Files[item].InputStream.Length];
                            Request.Files[item].InputStream.Read(binaryData, 0, (int)Request.Files[item].InputStream.Length);
                            Request.Files[item].InputStream.Close();
                            string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
                            var pic = new PictureModel
                            {
                                PersonalId = 0,
                                cfile = base64String,
                                typeId = 1,
                                NameTypeUpload = "SizeImageSlider"

                            };
                            var fileName = Infrastructure.Helpers.ImageHelper.SaveImgToString(pic);
                             model.GetType().GetProperties().FirstOrDefault(t => t.Name == prop)?.SetValue(model, fileName.XPicture.ConvertedFilename, null);
                        }

                    }

                }
            }
            model.ModifiedUser = id;
            model.ModifiedDate = DateTime.Now;
            String keys = model.Name.NormalizeD() + "\n" +
                      (String.IsNullOrEmpty(model.Description)
                          ? ""
                          : model.Description.NormalizeD()) + "\n" + model.Code.NormalizeD();
            model.KeySearch = keys;
            _CountryRepository.UpdateItem(model);
            ViewBag.Success = true;
            ViewBag.Message = Resource.SaveSuccessful;
            ModelState.Clear();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ActionName("Delete"), DisplayName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _CountryRepository.DeleteItem(id);
            return RedirectToAction("Index");
        }

        [ActionName("DeActive"), DisplayName("Delete")]
        public JsonResult DeActiveConfirmed(int id)
        {


            var model = _CountryRepository.GetItem(id);
            model.Active = 0;
            _CountryRepository.UpdateItem(model);
            return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
        }
    }
}
