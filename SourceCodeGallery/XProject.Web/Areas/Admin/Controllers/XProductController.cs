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
using MvcSiteMapProvider.Linq;
using XProject.Web.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
namespace XProject.Web.Areas.Admin.Controllers
{

    public class XProductController : Controller
    {
        //
        // GET: /Admin/BDSNews/
        private readonly IGeneralRepository<XProduct> _productRepository;
        private readonly IGeneralRepository<XCountry> _countryRepository;
        private readonly IGeneralRepository<XPicture> _pictureRepository;
        private readonly IGeneralRepository<XMenu> _menuRepository;
        private readonly string lang = ConfigurationManager.AppSettings["Culture"];

        public XProductController(IGeneralRepository<XMenu> menuRepository,IGeneralRepository<XProduct> productRepository, IGeneralRepository<XCountry> countryRepository, IGeneralRepository<XPicture> pictureRepository)
        {
            _productRepository = productRepository;
            _countryRepository = countryRepository;
            _pictureRepository = pictureRepository;
            _menuRepository = menuRepository;
        }

        public ActionResult Index()
        {

            var listMenuParent = _menuRepository.GetIQueryableItems().Where(x => x.Active == 1 && x.CodeLanguage == lang && x.IsParent == 1 && x.IdParentMenu == 0).ToList();
            ViewBag.ListMenuParent = listMenuParent;
            var listMenuChild = _menuRepository.GetIQueryableItems().Where(x => x.Active == 1 && x.CodeLanguage == lang && x.IsParent == 0 && x.IdParentMenu != 0).ToList();
            foreach (var item in listMenuChild.ToList())
            {
                var firstOrDefault = listMenuParent.FirstOrDefault(x => x.CurrentId == item.IdParentMenu);
                if (firstOrDefault != null)
                    item.Name = firstOrDefault.Name+" - "+item.Name;
            }
            ViewBag.ListMenuChild = listMenuChild;
            return View();
        }
        [AjaxOnly]
        public JsonResult IndexAjax(DataTableJS data,int idMenu = 0)
        {

            var listMenu = _productRepository.GetIQueryableItems().Where(T => T.CodeLanguage ==lang);
            if (idMenu != 0)
            {
                listMenu = listMenu.Where(x => x.TypeId == idMenu);
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
                var check = _pictureRepository.GetIQueryableItems().FirstOrDefault(x => x.ProductsId == item.CurrentId && x.TypeId == 1);
                item.UrlImage = check != null ? check.ConvertedFilename : "";
                var pic = _pictureRepository.GetIQueryableItems().Where(x => x.ProductsId == item.CurrentId && x.TypeId ==1).ToList();

                item.CountImg = pic.Count();
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
            ViewBag.Country = _countryRepository.GetIQueryableItems().Where(x => x.Active == 1 && x.IsLangauge == true).ToList();
            ViewBag.Menu = _menuRepository.GetIQueryableItems().Where(x => x.Active == 1 && x.CodeLanguage ==lang && x.IdParentMenu!=0).ToList();
            var count = _menuRepository.GetIQueryableItems().Count(T => T.Active == 1 && T.CodeLanguage == lang);
            var model = new XProduct();
            model.Order = count + 1;
         
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(XProduct model)
        {

            int id = CurrentUser.Identity.ID;
            string type = Request.Form["_Type"];




            model.CreateUser = id;
            model.CreateDate = DateTime.Now;

            if (model.XProductsArray != null)
            {

                model.CodeLanguage = model.XProductsArray[0].CodeLanguage;
                model.Name = model.XProductsArray[0].Name;
                model.Title = model.XProductsArray[0].Title;
                model.Includes = model.XProductsArray[0].Includes;
                model.Description = model.XProductsArray[0].Description;
                model.ShortDescription = model.XProductsArray[0].ShortDescription;
                model.Link = model.XProductsArray[0].Link;
                model.KeySearch = model.XProductsArray[0].Name.NormalizeD() + "\n" + (String.IsNullOrEmpty(model.XProductsArray[0].Description) ? "" : model.XProductsArray[0].Description.NormalizeD()); ;

                var result = _productRepository.CreateItem(model);
                var tblProduct = _productRepository.GetItem(result.ID);
                if (tblProduct != null)
                {
                    tblProduct.CurrentId = tblProduct.ID;
                    _productRepository.UpdateItem(tblProduct);
                }
                foreach (var item in model.XProductsArray.Skip(1).ToList())
                {
                    model.CodeLanguage = item.CodeLanguage;
                    model.Name = item.Name;
                    model.Description = item.Description;
                    model.Includes = item.Includes;
                    model.ShortDescription = item.ShortDescription;
                    model.Link = item.Link;
                    model.KeySearch = item.Name.NormalizeD() + "\n" + (String.IsNullOrEmpty(item.Description) ? "" : item.Description.NormalizeD()); ;
                    model.Title = item.Title;
                    model.Includes = item.Includes;
                    _productRepository.CreateItem(model);
                }
                if (string.IsNullOrEmpty(model.UrlImage))
                {
                    model.UrlImage = "";
                }
                var fileCv = model.UrlImage.Split(',').ToList();
                var imgDeletes = _pictureRepository.GetIQueryableItems().Where(x => x.ProductsId == model.ID && x.TypeId == 1 && !fileCv.Contains(x.ConvertedFilename)).ToList();
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

                                    idProducts = tblProduct.CurrentId,
                                    cfile = base64String,
                                    typeId = 1,
                                    NameTypeUpload = "SizeImageProduct"

                                };

                                var fileName = Infrastructure.Helpers.ImageHelper.SaveCroppedImage(Image.FromStream(Request.Files[i].InputStream), pic);
                                if (i == 0)
                                {
                                    var tblProduct1 = _productRepository.GetItem(tblProduct.ID);
                                    tblProduct1.UrlImage = fileName.XPicture.ConvertedFilename;
                                    _productRepository.UpdateItem(tblProduct1);
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
            ViewBag.Menu = _menuRepository.GetIQueryableItems().Where(x => x.Active == 1 && x.CodeLanguage ==lang && x.IdParentMenu != 0).ToList();
            var model = _productRepository.GetItem(id);
            ViewBag.ListImages = _pictureRepository.GetIQueryableItems().Where(x=>x.ProductsId== id && x.TypeId==1).ToList();
          
            foreach (var itemLang in (List<XCountry>)ViewBag.Country)
            {
                var pro = _productRepository.GetIQueryableItems().FirstOrDefault(x => x.CurrentId == model.CurrentId && x.CodeLanguage == itemLang.Code);
                if (pro == null)
                {
                    if (model != null)
                    {
                        var tblItem = new XProduct
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
                            TypeId = model.TypeId,
                            Title = model.Title,
                            Includes=model.Includes
                        

                        };

                        _productRepository.CreateItem(tblItem);

                    }

                }

            }

            var listCode = ((List<XCountry>)ViewBag.Country).Select(y => y.Code).ToList();
            model.XProductsList = _productRepository.GetIQueryableItems().Where(x => x.CurrentId == model.CurrentId && listCode.Contains(x.CodeLanguage)).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(XProduct model)
        {
            int id = CurrentUser.Identity.ID;

            var idCu = 0;

          
            if (model.XProductsArray != null)
            {
                foreach (var item in model.XProductsArray)
                {
                    String keys = item.Name.NormalizeD() + "\n" +
                                  (String.IsNullOrEmpty(item.Description)
                                      ? ""
                                      : item.Description.NormalizeD());
                    var tblProduct = _productRepository.GetItem(item.ID);
                    tblProduct.CodeLanguage = item.CodeLanguage;
                    tblProduct.Name = item.Name;
                    tblProduct.Link = item.Link;
                    tblProduct.Description = item.Description;
                    tblProduct.ShortDescription = item.ShortDescription;
                    tblProduct.UrlImage = model.UrlImage;
                    tblProduct.KeySearch = keys;
                    tblProduct.Active = model.Active;
                    tblProduct.CreateDate = model.CreateDate;
                    tblProduct.CreateUser = model.CreateUser;
                    tblProduct.ModifiedDate = DateTime.Now;
                    tblProduct.ModifiedUser = id;
                    tblProduct.Order = model.Order;
                    tblProduct.TypeId = model.TypeId;
                    idCu = tblProduct.CurrentId;
                    tblProduct.Title = item.Title;
                    tblProduct.Includes = item.Includes;
                   
                    _productRepository.UpdateItem(tblProduct);
                }
            }
            if (string.IsNullOrEmpty(model.UrlImage))
            {
                model.UrlImage = "";
            }
            var fileCv=   model.UrlImage.Split(',').ToList();
            var imgDeletes=   _pictureRepository.GetIQueryableItems().Where(x => x.ProductsId == model.ID && x.TypeId == 1 && !fileCv.Contains(x.ConvertedFilename)).ToList();
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
                                typeId = 1,
                                NameTypeUpload = "SizeImageProduct"

                            };
                            var fileName = Infrastructure.Helpers.ImageHelper.SaveCroppedImage(Image.FromStream(Request.Files[i].InputStream), pic);
                            if (i == 0)
                            {
                                var tblProduct1 = _productRepository.GetItem(model.ID);
                                tblProduct1.UrlImage = fileName.XPicture.ConvertedFilename;
                                _productRepository.UpdateItem(tblProduct1);
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
            var item = _productRepository.GetIQueryableItems().Where(x => x.CurrentId == id).ToList();
            foreach (var it in item)
            {
                _productRepository.DeleteItem(it.ID);
            }
          
            return RedirectToAction("Index");
        }

        [ActionName("DeActive"), DisplayName("Delete")]
        public JsonResult DeActiveConfirmed(int id)
        {

            var item = _productRepository.GetIQueryableItems().Where(x => x.CurrentId == id).ToList();
            foreach (var it in item)
            {
                var model = _productRepository.GetItem(it.ID);
                model.Active = 0;
                _productRepository.UpdateItem(model);
            }
            
            return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        public JsonResult GetImageById()
        {

            var listPic = _pictureRepository.GetIQueryableItems().Select(x=> x.ConvertedFilename).ToList();
            var arr = listPic.ToArray();
            var tt = new InputImage
            {
                initialPreview = arr,
                initialPreviewAsData = true,
                overwriteInitial = true,
                showCaption = false,
                showRemove = true,
                showUpload = true,
            };
            return Json(tt, JsonRequestBehavior.AllowGet);

        }



        [ActionName("DeleteImg"), DisplayName("DeleteImg")]
        public ActionResult DeleteImg(int id)
        {

            var size = "683x384;1920x1280;569x320;1600x900;583x325;455x683;911x683";
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

            var size = "683x384;1920x1280;569x320;1600x900;583x325;455x683;911x683";
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
                            typeId =1,
                            NameTypeUpload = "SizeImageProduct"

                        };

                        var fileName = Infrastructure.Helpers.ImageHelper.SaveCroppedImage(Image.FromStream(Request.Files[i].InputStream), pic);
                        if (i == 0)
                        {
                            var tblmenu = _productRepository.GetItem(idPro);
                            tblmenu.UrlImage = fileName.XPicture.ConvertedFilename;
                            _productRepository.UpdateItem(tblmenu);
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

            var listProduct = _pictureRepository.GetIQueryableItems().Where(x => x.ProductsId == idpro && x.TypeId == 1).ToList();
            foreach (var item in listProduct)
            {
                var tblPict = _pictureRepository.GetItem(item.ID);
                if (item.ID == idpic)
                {
                    tblPict.Position = 1;

                    var lisMenu = _productRepository.GetIQueryableItems().Where(x => x.CurrentId == idpro).ToList();
                    foreach (var itemMenu in lisMenu)
                    {
                        var tblPro = _productRepository.GetItem(itemMenu.ID);
                        tblPro.UrlImage = tblPict.ConvertedFilename;
                        _productRepository.UpdateItem(tblPro);
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
        public ActionResult AdGetImageById(int id)
        {
            var listPic = _pictureRepository.GetIQueryableItems().Where(x => x.ProductsId == id && x.TypeId == 1).OrderByDescending(x => x.Position).ToList();
            return PartialView(listPic);
        }
    }
}
