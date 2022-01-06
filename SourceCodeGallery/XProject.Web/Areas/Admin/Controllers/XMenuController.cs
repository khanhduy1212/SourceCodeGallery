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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using MvcSiteMapProvider.Linq;

namespace XProject.Web.Areas.Admin.Controllers
{

    public class XMenuController : Controller
    {
        //
        // GET: /Admin/BDSNews/
        private readonly IGeneralRepository<XMenu> _menuRepository;
        private readonly IGeneralRepository<XCountry> _countryRepository;
        private readonly IGeneralRepository<XPicture> _pictureRepository;
        private readonly string lang=ConfigurationManager.AppSettings["Culture"];

        public XMenuController(IGeneralRepository<XMenu> menuRepository, IGeneralRepository<XCountry> countryRepository, IGeneralRepository<XPicture> pictureRepository)
        {
            _menuRepository = menuRepository;
            _countryRepository = countryRepository;
            _pictureRepository = pictureRepository;
        }

        public ActionResult Index()
        {
            var listMenuParent = _menuRepository.GetIQueryableItems().Where(x => x.Active == 1 && x.CodeLanguage== lang && x.IsParent==1 && x.IdParentMenu==0).ToList();
            ViewBag.ListMenuParent = listMenuParent;
            return View();
        }
        [AjaxOnly]
        public JsonResult IndexAjax(DataTableJS data,int idMenu=0)
        {

            var listMenu = _menuRepository.GetIQueryableItems().Where(T =>  T.CodeLanguage ==lang);
            if (idMenu != 0)
            {
                listMenu = listMenu.Where(x => x.IdParentMenu == idMenu);
            }
            
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
            foreach (var item in queryFilter.ToList())
            {
                var pic = _pictureRepository.GetIQueryableItems().Where(x => x.ProductsId == item.ID && x.TypeId==3).ToList();
               
                item.CountImg = pic.Count();
                var getName = listMenu.FirstOrDefault(x => x.CurrentId == item.IdParentMenu);
                item.NameParent = getName != null ? getName.Name : "Is Parent";
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
            ViewBag.MultiImage = true;
            ViewBag.HideBase = true;
            ViewBag.ListMenu = _menuRepository.GetIQueryableItems().Where(x => x.Active == 1 && x.IsParent == 1 && x.CodeLanguage ==lang).ToList();
            ViewBag.Country = _countryRepository.GetIQueryableItems().Where(x => x.Active == 1 && x.IsLangauge == true).ToList();

            var count = _menuRepository.GetIQueryableItems().Count(T => T.Active == 1 && T.CodeLanguage == lang);
            var model = new XMenu();
            model.Order = count + 1;
           
            return View(model);
        }
       
        [HttpPost]
        public ActionResult Create(XMenu model)
        {

            int id = CurrentUser.Identity.ID;
            string type = Request.Form["_Type"];
            //if (Request.Files.Count > 0)
            //{
            //    foreach (String item in Request.Files.AllKeys)
            //    {
            //        string prop = item.Replace("_input", "");
            //        if (model.GetType().GetProperties().Any(t => t.Name == prop))
            //        {
            //            if (Request.Files[item] != null && Request.Files[item].ContentLength > 0)
            //            {

            //                byte[] binaryData;
            //                binaryData = new Byte[Request.Files[item].InputStream.Length];
            //                Request.Files[item].InputStream.Read(binaryData, 0, (int)Request.Files[item].InputStream.Length);

            //                string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
            //                var pic = new PictureModel
            //                {
            //                    PersonalId = 0,
            //                    cfile = base64String,
            //                    typeId = 1,
            //                    NameTypeUpload = "SizeImageMenu"

            //                };
            //                var fileName = Infrastructure.Helpers.ImageHelper.SaveCroppedImage(Image.FromStream(Request.Files[item].InputStream), pic);
            //                model.UrlImage = fileName.GetConvertedFileName();
            //                model.GetType().GetProperties().FirstOrDefault(t => t.Name == prop)?.SetValue(model, model.UrlImage, null);


            //            }

            //        }

            //    }
            //}


            model.CreateUser = id;
            model.CreateDate = DateTime.Now;

            if (model.XMenusArray != null)
            {

                model.CodeLanguage = model.XMenusArray[0].CodeLanguage;
                model.Name = model.XMenusArray[0].Name;
                model.Description = model.XMenusArray[0].Description;
                model.ShortDescription = model.XMenusArray[0].ShortDescription;
                model.LinkMenu = model.XMenusArray[0].LinkMenu;
                model.KeySearch = model.XMenusArray[0].Name.NormalizeD() + "\n" + (String.IsNullOrEmpty(model.XMenusArray[0].Description) ? "" : model.XMenusArray[0].Description.NormalizeD()); ;

                var result = _menuRepository.CreateItem(model);
                var tblMenu = _menuRepository.GetItem(result.ID);
                if (tblMenu != null)
                {
                    tblMenu.CurrentId = tblMenu.ID;
                    _menuRepository.UpdateItem(tblMenu);
                }
                foreach (var item in model.XMenusArray.Skip(1).ToList())
                {
                    model.CodeLanguage = item.CodeLanguage;
                    model.Name = item.Name;
                    model.Description = item.Description;
                    model.ShortDescription = item.ShortDescription;
                    model.LinkMenu = item.LinkMenu;
                    model.KeySearch = item.Name.NormalizeD() + "\n" + (String.IsNullOrEmpty(item.Description) ? "" : item.Description.NormalizeD()); ;




                    _menuRepository.CreateItem(model);
                }

                if (string.IsNullOrEmpty(model.UrlImage))
                {
                    model.UrlImage = "";
                }
                var fileCv = model.UrlImage.Split(',').ToList();
                var imgDeletes = _pictureRepository.GetIQueryableItems().Where(x => x.ProductsId == model.ID && x.TypeId == 3 && !fileCv.Contains(x.ConvertedFilename)).ToList();
                foreach (var img in imgDeletes)
                {
                    _pictureRepository.DeleteItem(img.ID);
                }

                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.AllKeys.Length; i++)
                    {
                        String item = Request.Files.AllKeys[i];
                        string prop = item.Replace("_input", "");
                        if (model.GetType().GetProperties().Any(t => t.Name == prop))
                        {
                            if (Request.Files[i] != null && Request.Files[i].ContentLength > 0)
                            {
                                byte[] binaryData;
                                binaryData = new Byte[Request.Files[i].InputStream.Length];
                                Request.Files[i].InputStream.Read(binaryData, 0, (int)Request.Files[i].InputStream.Length);

                                string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
                                var pic = new PictureModel
                                {

                                    idProducts = tblMenu.CurrentId,
                                    cfile = base64String,
                                    typeId = 3,
                                    NameTypeUpload = "SizeImageMenu"

                                };
                                var fileName = Infrastructure.Helpers.ImageHelper.SaveCroppedImage(Image.FromStream(Request.Files[i].InputStream), pic);
                                if (i == 0)
                                {
                                    var tblmenu = _menuRepository.GetItem(tblMenu.ID);
                                    tblmenu.UrlImage = fileName.XPicture.ConvertedFilename;
                                    _menuRepository.UpdateItem(tblmenu);
                                    fileName.XPicture.Position = 1;
                                }
                                else
                                {
                                    fileName.XPicture.Position = 2;
                                }
                                _pictureRepository.CreateItem(fileName.XPicture);

                            }
                        }
                    }


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
            ViewBag.MultiImage = true;
            ViewBag.HideBase = true;
            ViewBag.Country = _countryRepository.GetIQueryableItems().Where(x => x.Active == 1 && x.IsLangauge == true).ToList();
            ViewBag.ListMenu = _menuRepository.GetIQueryableItems().Where(x => x.Active == 1 && x.IsParent == 1 && x.CodeLanguage== "en-GB").ToList();
            ViewBag.ListImages = _pictureRepository.GetIQueryableItems().Where(x => x.ProductsId == id && x.TypeId == 3).ToList();
            var model = _menuRepository.GetItem(id);
            foreach (var itemLang in (List<XCountry>)ViewBag.Country)
            {
                var pro = _menuRepository.GetIQueryableItems().FirstOrDefault(x => x.CurrentId == model.CurrentId && x.CodeLanguage == itemLang.Code);
                if (pro == null)
                {
                    if (model != null)
                    {
                        var tblItem = new XMenu
                        {
                            CodeLanguage = itemLang.Code,
                            Name = model.Name,
                            Description = model.Description,
                            ShortDescription = model.ShortDescription,
                            LinkMenu = model.LinkMenu,
                            UrlImage = model.UrlImage,
                            CurrentId = model.CurrentId,
                            KeySearch = model.KeySearch,
                            Active = model.Active,
                            CreateDate = model.CreateDate,
                            CreateUser = model.CreateUser,
                            ModifiedDate = model.ModifiedDate,
                            ModifiedUser = model.ModifiedUser,
                            IdParentMenu = model.IdParentMenu,
                            Order = model.Order,
                            IsParent = model.IsParent,
                            IsCategoryMenu = model.IsCategoryMenu,
                            ShowHomeMenu = model.ShowHomeMenu,

                        };

                        _menuRepository.CreateItem(tblItem);

                    }

                }

            }

            var listCode = ((List<XCountry>)ViewBag.Country).Select(y => y.Code).ToList();
            model.XMenusList = _menuRepository.GetIQueryableItems().Where(x => x.CurrentId == model.CurrentId && listCode.Contains(x.CodeLanguage)).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(XMenu model, HttpPostedFileBase[] files)
        {
            int id = CurrentUser.Identity.ID;
            var idCu = 0;

            //if (Request.Files.Count > 0)
            //{
            //    foreach (String item in Request.Files.AllKeys)
            //    {
            //        string prop = item.Replace("_input", "");
            //        if (model.GetType().GetProperties().Any(t => t.Name == prop))
            //        {
            //            if (Request.Files[item] != null && Request.Files[item].ContentLength > 0)
            //            {


            //                byte[] binaryData;
            //                binaryData = new Byte[Request.Files[item].InputStream.Length];
            //                Request.Files[item].InputStream.Read(binaryData, 0, (int)Request.Files[item].InputStream.Length);

            //                string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
            //                var pic = new PictureModel
            //                {
            //                    PersonalId = 0,
            //                    cfile = base64String,
            //                    typeId = 1,
            //                    NameTypeUpload = "SizeImageMenu"

            //                };
            //                var fileName = Infrastructure.Helpers.ImageHelper.SaveCroppedImage(Image.FromStream(Request.Files[item].InputStream), pic);
            //                model.UrlImage = fileName.GetConvertedFileName();
            //                model.GetType().GetProperties().FirstOrDefault(t => t.Name == prop)?.SetValue(model, model.UrlImage, null);


            //            }

            //        }

            //    }
            //}
            if (model.XMenusArray != null)
            {
                foreach (var item in model.XMenusArray)
                {
                    String keys = item.Name.NormalizeD() + "\n" +
                                  (String.IsNullOrEmpty(item.Description)
                                      ? ""
                                      : item.Description.NormalizeD());
                    var tblmenu = _menuRepository.GetItem(item.ID);
                    tblmenu.CodeLanguage = item.CodeLanguage;
                    tblmenu.Name = item.Name;
                    tblmenu.LinkMenu = item.LinkMenu;
                    tblmenu.Description = item.Description;
                    tblmenu.ShortDescription = item.ShortDescription;
                    tblmenu.UrlImage = model.UrlImage;
                    idCu = tblmenu.CurrentId;
                    tblmenu.KeySearch = keys;
                    tblmenu.Active = model.Active;
                    tblmenu.CreateDate = model.CreateDate;
                    tblmenu.CreateUser = model.CreateUser;
                    tblmenu.ModifiedDate = DateTime.Now;
                    tblmenu.ModifiedUser = id;
                    tblmenu.IdParentMenu = model.IdParentMenu;
                    tblmenu.Order = model.Order;
                    tblmenu.IsParent = model.IsParent;
                    tblmenu.IsCategoryMenu = model.IsCategoryMenu;
                    tblmenu.ShowHomeMenu = model.ShowHomeMenu;
                    _menuRepository.UpdateItem(tblmenu);
                }
            }
            if (string.IsNullOrEmpty(model.UrlImage))
            {
                model.UrlImage = "";
            }
            var fileCv = model.UrlImage.Split(',').ToList();
            var imgDeletes = _pictureRepository.GetIQueryableItems().Where(x => x.ProductsId == model.ID && x.TypeId ==3 && !fileCv.Contains(x.ConvertedFilename)).ToList();
            foreach (var img in imgDeletes)
            {
                _pictureRepository.DeleteItem(img.ID);
            }

            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.AllKeys.Length; i++)
                {
                    String item = Request.Files.AllKeys[i];
                    string prop = item.Replace("_input", "");
                    if (model.GetType().GetProperties().Any(t => t.Name == prop))
                    {
                        if (Request.Files[i] != null && Request.Files[i].ContentLength > 0)
                        {
                            byte[] binaryData;
                            binaryData = new Byte[Request.Files[i].InputStream.Length];
                            Request.Files[i].InputStream.Read(binaryData, 0, (int)Request.Files[i].InputStream.Length);

                            string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
                            var pic = new PictureModel
                            {

                                idProducts = idCu,
                                cfile = base64String,
                                typeId = 3,
                                NameTypeUpload = "SizeImageMenu"

                            };
                            var fileName = Infrastructure.Helpers.ImageHelper.SaveCroppedImage(Image.FromStream(Request.Files[i].InputStream), pic);
                            if (i==0)
                            {
                                var tblmenu = _menuRepository.GetItem(model.ID);
                             tblmenu.UrlImage = fileName.XPicture.ConvertedFilename;
                                _menuRepository.UpdateItem(tblmenu);
                                fileName.XPicture.Position = 1;
                            }
                            else
                            {
                                fileName.XPicture.Position = 2;
                            }

                            _pictureRepository.CreateItem(fileName.XPicture);

                        }
                    }
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
        [ActionName("DeleteImg"), DisplayName("DeleteImg")]
        public ActionResult DeleteImg(int id)
        {

            var size = "1366x768;583x325;455x683;911x683";
            var pic = _pictureRepository.GetItem(id);
            if (pic != null)
            {

                Infrastructure.Helpers.ImageHelper.DeleteImg(pic.ConvertedFilename, size);
                _pictureRepository.DeleteItem(id);
            }
            return Json("1");
        }
        [AjaxOnly]
        public JsonResult DeleteImg1(int id)
        {

            var size = "1366x768;583x325;455x683;911x683";
            var pic = _pictureRepository.GetItem(id);
            var idPro = 0;
            if (pic != null)
            {
                if (pic.ProductsId != null) idPro = (int)pic.ProductsId;
                Infrastructure.Helpers.ImageHelper.DeleteImg(pic.ConvertedFilename, size);
                _pictureRepository.DeleteItem(id);
            }
            return Json(idPro, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpPost]
        public JsonResult UploadImg(HttpPostedFileBase[] files, int idPro)
        {
            if (files != null && files.Length > 0)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i] != null)
                    {
                        byte[] binaryData;
                        binaryData = new Byte[files[i].InputStream.Length];
                        long bytesRead = files[i].InputStream.Read(binaryData, 0, (int)files[i].InputStream.Length);
                       
                        string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
                        var pic = new PictureModel
                        {

                            idProducts = idPro,
                            cfile = base64String,
                            typeId = 3,
                            NameTypeUpload = "SizeImageMenu"

                        };

                        var fileName = Infrastructure.Helpers.ImageHelper.SaveCroppedImage(Image.FromStream(Request.Files[i].InputStream), pic);
                        if (i == 0)
                        {
                            var tblmenu = _menuRepository.GetItem(idPro);
                            tblmenu.UrlImage = fileName.XPicture.ConvertedFilename;
                            _menuRepository.UpdateItem(tblmenu);
                            fileName.XPicture.Position = 1;
                        }


                        _pictureRepository.CreateItem(fileName.XPicture);

                    }
                }
            }
            return Json(idPro);
        }


        [AjaxOnly]
        public JsonResult UpdatePosition(int idpro, int idpic)
        {

            var listProduct = _pictureRepository.GetIQueryableItems().Where(x => x.ProductsId == idpro && x.TypeId==3).ToList();
            foreach (var item in listProduct)
            {
                var tblPict = _pictureRepository.GetItem(item.ID);
                if (item.ID == idpic)
                {
                    tblPict.Position = 1;

                    var lisMenu = _menuRepository.GetIQueryableItems().Where(x => x.CurrentId == idpro).ToList();
                    foreach (var itemMenu in lisMenu)
                    {
                        var tblPro = _menuRepository.GetItem(itemMenu.ID);
                        tblPro.UrlImage = tblPict.ConvertedFilename;
                        _menuRepository.UpdateItem(tblPro);
                    }
                   
                }
                else
                {
                    tblPict.Position = 0;
                }
                _pictureRepository.UpdateItem(tblPict);
            }



            return Json(idpro, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetImageById(int id)
        {
            var listPic = _pictureRepository.GetIQueryableItems().Where(x => x.ProductsId == id && x.TypeId == 3).OrderByDescending(x => x.Position).ToList();
            return PartialView(listPic);
        }

    }
}
