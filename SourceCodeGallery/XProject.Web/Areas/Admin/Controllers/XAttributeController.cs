using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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

    public class XAttributeController : Controller
    {
        //
        // GET: /Admin/BDSNews/
        private readonly IGeneralRepository<XAttribute> _menuRepository;
        private readonly IGeneralRepository<XCountry> _countryRepository;
        private readonly string lang = ConfigurationManager.AppSettings["Culture"];

        public XAttributeController(IGeneralRepository<XAttribute> menuRepository, IGeneralRepository<XCountry> countryRepository)
        {
            _menuRepository = menuRepository;
            _countryRepository = countryRepository;
        }

        public ActionResult Index()
        {


            return View();
        }
        [AjaxOnly]
        public JsonResult IndexAjax(DataTableJS data)
        {

            var listMenu = _menuRepository.GetIQueryableItems().Where(T => T.Active == 1 && T.CodeLanguage ==lang);
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
            data.data = queryFilter.Skip(data.start)
                .Take(data.length == -1 ? data.recordsTotal : data.length)
                .ToList();
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Create()
        {
            ViewBag.HideBase = true;
            ViewBag.Country = _countryRepository.GetIQueryableItems().Where(x => x.Active == 1 && x.IsLangauge == true).ToList();
            var count = _menuRepository.GetIQueryableItems().Count(T => T.Active == 1 && T.CodeLanguage == lang);
            var model = new XAttribute();
            model.Order = count + 1;
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(XAttribute model, HttpPostedFileBase[] files)
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

            if (model.XAttributesArray != null)
            {

                model.CodeLanguage = model.XAttributesArray[0].CodeLanguage;
                model.Name = model.XAttributesArray[0].Name;
                model.Description = model.XAttributesArray[0].Description;
                model.ShortDescription = model.XAttributesArray[0].ShortDescription;
               
                model.KeySearch = model.XAttributesArray[0].Name.NormalizeD() + "\n" + (String.IsNullOrEmpty(model.XAttributesArray[0].Description) ? "" : model.XAttributesArray[0].Description.NormalizeD()); ;

                var result = _menuRepository.CreateItem(model);
                var tblMenu = _menuRepository.GetItem(result.ID);
                if (tblMenu != null)
                {
                    tblMenu.CurrentId = tblMenu.ID;
                    _menuRepository.UpdateItem(tblMenu);
                }
                foreach (var item in model.XAttributesArray.Skip(1).ToList())
                {
                    model.CodeLanguage = item.CodeLanguage;
                    model.Name = item.Name;
                    model.Description = item.Description;
                    model.ShortDescription = item.ShortDescription;
                   
                    model.KeySearch = item.Name.NormalizeD() + "\n" + (String.IsNullOrEmpty(item.Description) ? "" : item.Description.NormalizeD()); ;




                    _menuRepository.CreateItem(model);
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
          
            var model = _menuRepository.GetItem(id);
            foreach (var itemLang in (List<XCountry>)ViewBag.Country)
            {
                var pro = _menuRepository.GetIQueryableItems().FirstOrDefault(x => x.CurrentId == model.CurrentId && x.CodeLanguage == itemLang.Code);
                if (pro == null)
                {
                    if (model != null)
                    {
                        var tblItem = new XAttribute
                        {
                            CodeLanguage = itemLang.Code,
                            Name = model.Name,
                            Description = model.Description,
                            ShortDescription = model.ShortDescription,
                           
                            CurrentId = model.CurrentId,
                            KeySearch = model.KeySearch,
                            Active = model.Active,
                            CreateDate = model.CreateDate,
                            CreateUser = model.CreateUser,
                            ModifiedDate = model.ModifiedDate,
                            ModifiedUser = model.ModifiedUser,
                            Order = model.Order,
                           

                        };

                        _menuRepository.CreateItem(tblItem);

                    }

                }

            }

            var listCode = ((List<XCountry>)ViewBag.Country).Select(y => y.Code).ToList();
            model.XAttributesList = _menuRepository.GetIQueryableItems().Where(x => x.CurrentId == model.CurrentId && listCode.Contains(x.CodeLanguage)).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(XAttribute model, HttpPostedFileBase[] files)
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
                            Request.Files[item].InputStream.Close();
                            string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
                            var pic = new PictureModel
                            {
                                PersonalId = 0,
                                cfile = base64String,
                                typeId = 1,
                                NameTypeUpload= "SizeImageSlider"

                            };
                          var fileName=  Infrastructure.Helpers.ImageHelper.SaveImgToString(pic);
                             model.GetType().GetProperties().FirstOrDefault(t => t.Name == prop)?.SetValue(model, fileName.XPicture.ConvertedFilename, null);
                        }
                       
                    }

                }
            }
            if (model.XAttributesArray != null)
            {
                foreach (var item in model.XAttributesArray)
                {
                    String keys = item.Name.NormalizeD() + "\n" +
                                  (String.IsNullOrEmpty(item.Description)
                                      ? ""
                                      : item.Description.NormalizeD());
                    var tblmenu = _menuRepository.GetItem(item.ID);
                    tblmenu.CodeLanguage = item.CodeLanguage;
                    tblmenu.Name = item.Name;
                   
                    tblmenu.Description = item.Description;
                    tblmenu.ShortDescription = item.ShortDescription;
                  
                    tblmenu.KeySearch = keys;
                    tblmenu.Active = model.Active;
                    tblmenu.CreateDate = model.CreateDate;
                    tblmenu.CreateUser = model.CreateUser;
                    tblmenu.ModifiedDate = DateTime.Now;
                    tblmenu.ModifiedUser = id;
                    tblmenu.Order = model.Order;
                    _menuRepository.UpdateItem(tblmenu);
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
            var model = _menuRepository.GetItem(id);
            model.Active = 0;
            _menuRepository.UpdateItem(model);
            return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
        }
    }
}
