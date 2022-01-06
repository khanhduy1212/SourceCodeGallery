using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
namespace XProject.Web.Areas.Admin.Controllers
{

    public class XTypeController : Controller
    {
        //
        // GET: /Admin/BDSNews/
        private readonly IGeneralRepository<XType> _typeRepository;
        private readonly IGeneralRepository<XCountry> _countryRepository;
        private readonly IGeneralRepository<XMenu> _menuRepository;
        private readonly string lang = ConfigurationManager.AppSettings["Culture"];


        public XTypeController(IGeneralRepository<XMenu> menuRepository, IGeneralRepository<XCountry> countryRepository, IGeneralRepository<XType> typeRepository)
        {
            _menuRepository = menuRepository;
            _countryRepository = countryRepository;
            _typeRepository = typeRepository;
         
        }

        public ActionResult Index()
        {


            return View();
        }
        [AjaxOnly]
        public JsonResult IndexAjax(DataTableJS data)
        {

            var listMenu = _typeRepository.GetIQueryableItems().Where(T => T.Active == 1 && T.Type != 1 && T.CodeLanguage ==lang);
            String search = null;
            if (data.search != null && data.search["value"] != null)
            {
                search = data.search["value"];
            }
            var column = data.order[0]["column"];
            var dir = data.order[0]["dir"];
            string columnName = ((String[])data.columns[int.Parse(column)]["data"])[0];
            var queryFilter = listMenu.Where(T => (search == "" || (search != null && (T.KeySearch.ToLower().Contains(search.ToLower())))));
            if (dir == "asc")
            {
                queryFilter = queryFilter.OrderByField(columnName, true);
            }
            else
            {
                queryFilter = queryFilter.OrderByField(columnName, false);
            }
           
            data.recordsTotal = listMenu.Count();
            data.recordsFiltered = queryFilter.Count();
            data.data = queryFilter.Skip(data.start).Take(data.length == -1 ? data.recordsTotal : data.length).ToList();
           
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Create()
        {
            ViewBag.HideBase = true;
            ViewBag.Country = _countryRepository.GetIQueryableItems().Where(x => x.Active == 1 && x.IsLangauge == true).ToList();
            ViewBag.Menu = _menuRepository.GetIQueryableItems().Where(x => x.Active == 1 && x.CodeLanguage ==lang).ToList();
            var count = _menuRepository.GetIQueryableItems().Count(T => T.Active == 1 && T.CodeLanguage == lang);
            var model = new XType();
            model.Order = count + 1;
           
            return View(model);
        }
     
        [HttpPost]
        public ActionResult Create(XType model, HttpPostedFileBase[] files)
        {

            int id = CurrentUser.Identity.ID;
            string type = Request.Form["_Type"];
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

                            string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
                            var pic = new PictureModel
                            {
                                PersonalId = 0,
                                cfile = base64String,
                                typeId = 1,
                                NameTypeUpload = "SizeImageType"

                            };
                            var fileName = Infrastructure.Helpers.ImageHelper.SaveCroppedImage(Image.FromStream(Request.Files[item].InputStream), pic);
                            model.UrlImage = fileName.GetConvertedFileName();
                            model.GetType().GetProperties().FirstOrDefault(t => t.Name == prop)?.SetValue(model, model.UrlImage, null);

                        }

                    }

                }
            }



            model.CreateUser = id;
            model.CreateDate = DateTime.Now;

            if (model.XTypesArray != null)
            {

                model.CodeLanguage = model.XTypesArray[0].CodeLanguage;
                model.Name = model.XTypesArray[0].Name;
                model.Description = model.XTypesArray[0].Description;
                model.ShortDescription = model.XTypesArray[0].ShortDescription;
                model.Link = model.XTypesArray[0].Link;
                model.KeySearch = model.XTypesArray[0].Name.NormalizeD() + "\n" + (String.IsNullOrEmpty(model.XTypesArray[0].Description) ? "" : model.XTypesArray[0].Description.NormalizeD()); ;

                var result = _typeRepository.CreateItem(model);
                var tblMenu = _menuRepository.GetItem(result.ID);
                if (tblMenu != null)
                {
                    tblMenu.CurrentId = result.ID;
                    _menuRepository.UpdateItem(tblMenu);
                }
                foreach (var item in model.XTypesArray.Skip(1).ToList())
                {
                    model.CodeLanguage = item.CodeLanguage;
                    model.Name = item.Name;
                    model.Description = item.Description;
                    model.ShortDescription = item.ShortDescription;
                    model.Link = item.Link;
                    model.CurrentId = result.ID;
                    model.KeySearch = item.Name.NormalizeD() + "\n" + (String.IsNullOrEmpty(item.Description) ? "" : item.Description.NormalizeD()); ;




                    _typeRepository.CreateItem(model);
                }
            }





            if (type == "ajax")
            {
                return Content(new JavaScriptSerializer().Serialize(model), "application/json");
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            ViewBag.HideBase = true;
            ViewBag.Country = _countryRepository.GetIQueryableItems().Where(x => x.Active == 1 && x.IsLangauge == true).ToList();
            ViewBag.Menu = _menuRepository.GetIQueryableItems().Where(x => x.Active == 1 && x.CodeLanguage ==lang).ToList();
            var model = _typeRepository.GetItem(id);
            foreach (var itemLang in (List<XCountry>)ViewBag.Country)
            {
                var pro = _typeRepository.GetIQueryableItems().FirstOrDefault(x => x.CurrentId == model.CurrentId && x.CodeLanguage == itemLang.Code);
                if (pro == null)
                {
                    if (model != null)
                    {
                        var tblItem = new XType
                        {
                            CodeLanguage = itemLang.Code,
                            Name = model.Name,
                            Description = model.Description,
                            ShortDescription = model.ShortDescription,
                            Link = model.Link,
                            UrlImage = model.UrlImage,
                            CurrentId = model.CurrentId,
                            KeySearch = model.KeySearch,
                            Active = model.Active,
                            CreateDate = model.CreateDate,
                            CreateUser = model.CreateUser,
                            ModifiedDate = model.ModifiedDate,
                            ModifiedUser = model.ModifiedUser,
                            Order = model.Order,
                            Type = model.Type,
                           

                        };

                        _typeRepository.CreateItem(tblItem);

                    }

                }

            }

            var listCode = ((List<XCountry>)ViewBag.Country).Select(y => y.Code).ToList();
            model.XTypesList = _typeRepository.GetIQueryableItems().Where(x => x.CurrentId == model.CurrentId && listCode.Contains(x.CodeLanguage)).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(XType model, HttpPostedFileBase[] files)
        {
            int id = CurrentUser.Identity.ID;



            if (Request.Files.Count > 0)
            {
                foreach (String item in Request.Files.AllKeys)
                {
                    string prop = item.Replace("_input", "");
                    if (model.GetType().GetProperties().Any(t => t.Name == prop))
                    {
                        if (Request.Files[item]!=null && Request.Files[item].ContentLength>0)
                        {

                            byte[] binaryData;
                            binaryData = new Byte[Request.Files[item].InputStream.Length];
                            Request.Files[item].InputStream.Read(binaryData, 0, (int)Request.Files[item].InputStream.Length);
                         
                            string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
                            var pic = new PictureModel
                            {
                                PersonalId = 0,
                                cfile = base64String,
                                typeId = 1,
                                NameTypeUpload = "SizeImageType"

                            };
                            var fileName = Infrastructure.Helpers.ImageHelper.SaveCroppedImage(Image.FromStream(Request.Files[item].InputStream), pic);
                            model.UrlImage = fileName.GetConvertedFileName();
                            model.GetType().GetProperties().FirstOrDefault(t => t.Name == prop)?.SetValue(model, model.UrlImage, null);



                        }

                    }

                }
            }
            if (model.XTypesArray != null)
            {
                foreach (var item in model.XTypesArray)
                {
                    String keys = item.Name.NormalizeD() + "\n" +
                                  (String.IsNullOrEmpty(item.Description)
                                      ? ""
                                      : item.Description.NormalizeD());
                    var tblmenu = _typeRepository.GetItem(item.ID);
                    tblmenu.CodeLanguage = item.CodeLanguage;
                    tblmenu.Name = item.Name;
                    tblmenu.Link = item.Link;
                    tblmenu.Description = item.Description;
                    tblmenu.ShortDescription = item.ShortDescription;
                    tblmenu.UrlImage = model.UrlImage;
                    tblmenu.KeySearch = keys;
                    tblmenu.Active = model.Active;
                    tblmenu.CreateDate = model.CreateDate;
                    tblmenu.CreateUser = model.CreateUser;
                    tblmenu.ModifiedDate = DateTime.Now;
                    tblmenu.ModifiedUser = id;
                    tblmenu.Order = model.Order;
                    tblmenu.Type = model.Type;
                    _typeRepository.UpdateItem(tblmenu);
                }
            }

            ViewBag.Success = true;
            ViewBag.Message = Resource.SaveSuccessful;
            ModelState.Clear();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ActionName("Delete"), DisplayName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _menuRepository.DeleteItem(id);
            return RedirectToAction("Index");
        }
      
        [ActionName("DeActive"), DisplayName("Delete")]
        public JsonResult DeActiveConfirmed(int id)
        {
            var item = _menuRepository.GetIQueryableItems().Where(x => x.CurrentId == id).ToList();
            foreach (var it in item)
            {
                var model = _menuRepository.GetItem(it.ID);
                model.Active = 0;
                _menuRepository.UpdateItem(model);
            }
            return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
        }
    }
}
