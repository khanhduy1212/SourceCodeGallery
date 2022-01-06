using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using XProject.Domain.Abstract;
using XProject.Domain.Entities;

using XProject.Web.Infrastructure;
using XProject.Web.Infrastructure.Helpers;
using XProject.Web.Infrastructure.Utility;
using XProject.Web.Models;
using MvcSiteMapProvider;

namespace XProject.Web.Controllers
{
    public class HomeController : Controller
    {



        private readonly IGeneralRepository<XNew> _newGeneralRepository;
        private readonly IGeneralRepository<XMenu> _menuGeneralRepository;
        private readonly IGeneralRepository<XProduct> _productGeneralRepository;
        private readonly IGeneralRepository<XType> _typeGeneralRepository;
        private readonly IGeneralRepository<XEmail> _emailRepository;
        private readonly IGeneralRepository<XSeo> _seoRepository;
        private readonly IGeneralRepository<XPicture> _pictureGeneralRepository;
        private readonly IGeneralRepository<XContentDetail> _contentDetailGeneralRepository;

        public HomeController(IGeneralRepository<XSeo> seoRepository, IGeneralRepository<XEmail> emailRepository, IGeneralRepository<XContentDetail> contentDetailGeneralRepository, IGeneralRepository<XPicture> pictureGeneralRepository, IGeneralRepository<XMenu> menuGeneralRepository, IGeneralRepository<XNew> newGeneralRepository, IGeneralRepository<XProduct> productGeneralRepository, IGeneralRepository<XType> typeGeneralRepository)
        {
            _seoRepository = seoRepository;
            _newGeneralRepository = newGeneralRepository;
            _menuGeneralRepository = menuGeneralRepository;
            _productGeneralRepository = productGeneralRepository;
            _typeGeneralRepository = typeGeneralRepository;
            _pictureGeneralRepository = pictureGeneralRepository;
            _contentDetailGeneralRepository = contentDetailGeneralRepository;
            _emailRepository = emailRepository;
        }


        public ActionResult Index()
        {
            var lang = "";
            var httpCookie = Request.Cookies["Language"];
            lang = httpCookie != null ? httpCookie.Value : ConfigurationManager.AppSettings["Culture"];
            var titleMeta = "";
            var descriptionMeta = "";
            var keyWordMeta = "";
            var urlMeta = "";
            var imageMeta = "";

            var url = ConfigurationManager.AppSettings["UrlWeb"];
            var listMeta = _seoRepository.GetIQueryableItems().FirstOrDefault(x => x.Link.Equals(url) && x.CodeLanguage == lang);
            if (listMeta != null)
            {
                titleMeta = listMeta.Name;
                descriptionMeta = listMeta.Description;
                keyWordMeta = listMeta.KeySearch;
                urlMeta = listMeta.Link;
                imageMeta = !string.IsNullOrEmpty(listMeta.UrlImage) ? (ConfigurationManager.AppSettings["UrlImage"] + listMeta.UrlImage.Split('_')[0] + "/" + String.Format(listMeta.UrlImage, 853)) : ConfigurationManager.AppSettings["UrlWeb"] + "/Template/images/logo-fb.png";
            }

            ViewBag.TitleMeta = titleMeta;
            ViewBag.DescriptionMeta = descriptionMeta;
            ViewBag.KeyWordMeta = keyWordMeta;
            ViewBag.UrlMeta = urlMeta;
            ViewBag.ImageMeta = imageMeta;
            return View();

        }
        public ActionResult ListCategoy(string id)
        {
            var lang = "";
            var httpCookie = Request.Cookies["Language"];
            lang = httpCookie != null ? httpCookie.Value : ConfigurationManager.AppSettings["Culture"];
            var _id = int.Parse(id.Split('-').Last());

            var tblMenu = _menuGeneralRepository.GetIQueryableItems().FirstOrDefault(x => x.CurrentId == _id && x.CodeLanguage == lang);
            var listMenu = _menuGeneralRepository.GetIQueryableItems().Where(x => x.IdParentMenu == _id && x.CodeLanguage == lang).ToList();

            var allModel = new AllModel
            {
                TblMenu = tblMenu,
                ListMenu = listMenu
            };
            var titleMeta = "";
            var descriptionMeta = "";
            var keyWordMeta = "";
            var urlMeta = "";
            var imageMeta = "";
            if (tblMenu != null)
            {
                var url = ConfigurationManager.AppSettings["UrlWeb"] + Url.Action("ListCategoy", "Home", new { id = tblMenu.Name.UrlFrendly() + "-" + tblMenu.CurrentId });
                var listMeta = _seoRepository.GetIQueryableItems().FirstOrDefault(x => x.Link.Equals(url) && x.CodeLanguage == lang);
                if (listMeta != null)
                {
                    titleMeta = listMeta.Name;
                    descriptionMeta = listMeta.Description;
                    keyWordMeta = listMeta.KeySearch;
                    urlMeta = listMeta.Link;
                    imageMeta = !string.IsNullOrEmpty(listMeta.UrlImage) ? (ConfigurationManager.AppSettings["UrlImage"] + listMeta.UrlImage.Split('_')[0] + "/" + String.Format(listMeta.UrlImage, 853)) : ConfigurationManager.AppSettings["UrlWeb"] + "/Template/images/logo-fb.png";
                }
            }
            ViewBag.TitleMeta = titleMeta;
            ViewBag.DescriptionMeta = descriptionMeta;
            ViewBag.KeyWordMeta = keyWordMeta;
            ViewBag.UrlMeta = urlMeta;
            ViewBag.ImageMeta = imageMeta;
            return View(allModel);

        }
        public ActionResult DetailCategoy(string id)
        {
            var lang = "";
            var httpCookie = Request.Cookies["Language"];
            lang = httpCookie != null ? httpCookie.Value : ConfigurationManager.AppSettings["Culture"];
            var _id = int.Parse(id.Split('-').Last());

            var tblMenu = _menuGeneralRepository.GetIQueryableItems().FirstOrDefault(x => x.CurrentId == _id && x.CodeLanguage == lang);
            ViewBag.Product = _productGeneralRepository.GetIQueryableItems().Where(x => x.TypeId == _id && x.CodeLanguage == lang && x.Active == 1).ToList();


            var _tblMenu = _menuGeneralRepository.GetIQueryableItems().FirstOrDefault(x => x.CurrentId == tblMenu.IdParentMenu && x.CodeLanguage == lang);
            var _listMenu = _menuGeneralRepository.GetIQueryableItems().Where(x => x.IdParentMenu == tblMenu.IdParentMenu && x.CodeLanguage == lang).ToList();
            var allModel = new AllModel
            {
                TblMenu = _tblMenu,
                ListMenu = _listMenu
            };

            ViewBag.AllModel = allModel;

            var listMenuParent = _menuGeneralRepository.GetIQueryableItems().FirstOrDefault(x => x.CodeLanguage == lang && x.CurrentId == tblMenu.IdParentMenu);
            if (listMenuParent != null)
            {
                var listMenu = _menuGeneralRepository.GetIQueryableItems().OrderBy(x => x.Order).FirstOrDefault(x => x.CodeLanguage == lang && x.Order > listMenuParent.Order && x.IsParent == 1 && x.IsCategoryMenu == 1 && x.IdParentMenu == 0);
                if (listMenu != null)
                {
                    ViewBag.NameMenuCategory = "/home/list-category/" + listMenu.Name.UrlFrendly() + "-" + listMenu.CurrentId;
                }
                else
                {
                    ViewBag.NameMenuCategory = "/home/list-category/nghe-thuat-nhiep-anh-khach-san-9";
                }

            }

            var titleMeta = "";
            var descriptionMeta = "";
            var keyWordMeta = "";
            var urlMeta = "";
            var imageMeta = "";
            if (tblMenu != null)
            {
                var url = ConfigurationManager.AppSettings["UrlWeb"] + Url.Action("DetailCategoy", "Home", new { id = tblMenu.Name.UrlFrendly() + "-" + tblMenu.CurrentId });
                var listMeta = _seoRepository.GetIQueryableItems().FirstOrDefault(x => x.Link.Equals(url) && x.CodeLanguage == lang);
                if (listMeta != null)
                {
                    titleMeta = listMeta.Name;
                    descriptionMeta = listMeta.Description;
                    keyWordMeta = listMeta.KeySearch;
                    urlMeta = listMeta.Link;
                    imageMeta = !string.IsNullOrEmpty(listMeta.UrlImage) ? (ConfigurationManager.AppSettings["UrlImage"] + listMeta.UrlImage.Split('_')[0] + "/" + String.Format(listMeta.UrlImage, 853)) : ConfigurationManager.AppSettings["UrlWeb"] + "/Template/images/logo-fb.png";
                }
            }
            ViewBag.TitleMeta = titleMeta;
            ViewBag.DescriptionMeta = descriptionMeta;
            ViewBag.KeyWordMeta = keyWordMeta;
            ViewBag.UrlMeta = urlMeta;
            ViewBag.ImageMeta = imageMeta;


            return View(tblMenu);

        }
        public ActionResult Contact()
        {
            return View();

        }
        public ActionResult SetLanguage(string lang)
        {
            Session["language"] = lang;
            return Json("1", JsonRequestBehavior.AllowGet);

        }
        public ActionResult DetailNew(string id)
        {
            var lang = "";
            var httpCookie = Request.Cookies["Language"];
            lang = httpCookie != null ? httpCookie.Value : ConfigurationManager.AppSettings["Culture"];
            var _id = int.Parse(id.Split('-').Last());


            var listNews = _newGeneralRepository.GetIQueryableItems().Where(t => t.Active == 1 && t.CodeLanguage == lang).OrderBy(t => t.Order).ThenBy(t => t.CreateDate).ToList();

            var tblNew = listNews.FirstOrDefault(x => x.CurrentId == _id);
            var index = listNews.IndexOf(tblNew);
            if (index > 0)
            {
                ViewBag.Prev = listNews[index - 1];
            }
            if (index < listNews.Count - 1)
            {
                ViewBag.Next = listNews[index + 1];
            }


            return View(tblNew);

        }
        public ActionResult DetailProduct(string id)
        {
            var lang = "";
            var httpCookie = Request.Cookies["Language"];
            lang = httpCookie != null ? httpCookie.Value : ConfigurationManager.AppSettings["Culture"];
            var _id = int.Parse(id.Split('-').Last());





            var _listProduct =
               _productGeneralRepository.GetIQueryableItems().Where(x => x.CodeLanguage == lang && x.Active == 1).OrderBy(t => t.Order).ThenBy(t => t.CreateDate).ToList();


            var ids = _listProduct.Select(t => t.CurrentId).ToList();
            var pics =
                _pictureGeneralRepository.GetIQueryableItems()
                    .Where(x => x.ProductsId.HasValue && ids.Contains(x.ProductsId.Value) && x.TypeId == 1).OrderByDescending(x => x.Position).ToList();

            foreach (var item in _listProduct)
            {
                var pic = pics.FirstOrDefault(t => t.ProductsId == item.CurrentId && t.TypeId == 1);
                item.UrlImage = pic != null ? pic.ConvertedFilename : "";
            }
            var tblPrduct = _listProduct.FirstOrDefault(x => x.CurrentId == _id);
            //var listProduct = _listProduct.Where(t=>t.CurrentId!=_id).Skip(_listProduct.IndexOf(tblPrduct)).Take(3).ToList();
            //if (listProduct.Count<3)
            //{
            //    var index = 3 - listProduct.Count-1;
            //    listProduct= _listProduct.Where(t => t.CurrentId != _id).Skip(_listProduct.IndexOf(tblPrduct) - index).Take(3).ToList();
            //}
            ViewBag.ListProduct = _listProduct.Take(3).ToList();
            ViewBag.ListProductAll = _listProduct;
            var titleMeta = "";
            var descriptionMeta = "";
            var keyWordMeta = "";
            var urlMeta = "";
            var imageMeta = "";
            if (tblPrduct != null)
            {
                var url = ConfigurationManager.AppSettings["UrlWeb"] + Url.Action("DetailProduct", "Home", new { id = tblPrduct.Name.UrlFrendly() + "-" + tblPrduct.CurrentId });
                var listMeta = _seoRepository.GetIQueryableItems().FirstOrDefault(x => x.Link.Equals(url) && x.CodeLanguage == lang);
                if (listMeta != null)
                {
                    titleMeta = listMeta.Name;
                    descriptionMeta = listMeta.Description;
                    keyWordMeta = listMeta.KeySearch;
                    urlMeta = listMeta.Link;
                    imageMeta = !string.IsNullOrEmpty(listMeta.UrlImage) ? (ConfigurationManager.AppSettings["UrlImage"] + listMeta.UrlImage.Split('_')[0] + "/" + String.Format(listMeta.UrlImage, 853)) : ConfigurationManager.AppSettings["UrlWeb"] + "/Template/images/logo-fb.png";
                }
            }
            ViewBag.TitleMeta = titleMeta;
            ViewBag.DescriptionMeta = descriptionMeta;
            ViewBag.KeyWordMeta = keyWordMeta;
            ViewBag.UrlMeta = urlMeta;
            ViewBag.ImageMeta = imageMeta;
            return View(tblPrduct);

        }

        [HttpPost]
        public ActionResult GetLanguage()
        {
            var lang = "";
            var httpCookie = Request.Cookies["Language"];
            lang = httpCookie != null ? httpCookie.Value : ConfigurationManager.AppSettings["Culture"];
            var dataVo = _contentDetailGeneralRepository.GetIQueryableItems().Where(x => x.Active == 1 && x.CodeLanguage == lang).ToList();

            return Json(dataVo.ToList());



        }

        [HttpPost]
        public ActionResult GetNewById(int id)
        {
            var lang = "";
            var httpCookie = Request.Cookies["Language"];
            lang = httpCookie != null ? httpCookie.Value : ConfigurationManager.AppSettings["Culture"];
            var dataVo = _newGeneralRepository.GetIQueryableItems().FirstOrDefault(x => x.Active == 1 && x.CodeLanguage == lang && x.CurrentId == id);
            if (dataVo != null)
            {
                dataVo.UrlImage = ConfigurationManager.AppSettings["UrlImage"] + dataVo.UrlImage.Split('_')[0] + "/" +
                                  String.Format(dataVo.UrlImage, 800800);
                return Json(dataVo);
            }
            return Json('1');



        }


        [HttpGet]
        public PartialViewResult _pLoadDetail(string id)
        {

            var lang = "";
            var httpCookie = Request.Cookies["Language"];
            lang = httpCookie != null ? httpCookie.Value : ConfigurationManager.AppSettings["Culture"];
            var _id = int.Parse(id.Split('-').Last());

            var tblMenu = _menuGeneralRepository.GetIQueryableItems().FirstOrDefault(x => x.CurrentId == _id && x.CodeLanguage == lang);
            ViewBag.Product = _productGeneralRepository.GetIQueryableItems().Where(x => x.TypeId == _id && x.CodeLanguage == lang && x.Active == 1).ToList();


            var _tblMenu = _menuGeneralRepository.GetIQueryableItems().FirstOrDefault(x => x.CurrentId == tblMenu.IdParentMenu && x.CodeLanguage == lang);
            var _listMenu = _menuGeneralRepository.GetIQueryableItems().Where(x => x.IdParentMenu == tblMenu.IdParentMenu && x.CodeLanguage == lang).ToList();
            var allModel = new AllModel
            {
                TblMenu = _tblMenu,
                ListMenu = _listMenu
            };
            ViewBag.AllModel = allModel;

            var listMenuParent = _menuGeneralRepository.GetIQueryableItems().FirstOrDefault(x => x.CodeLanguage == lang && x.CurrentId == tblMenu.IdParentMenu);
            if (listMenuParent != null)
            {
                var listMenu = _menuGeneralRepository.GetIQueryableItems().OrderBy(x => x.Order).FirstOrDefault(x => x.CodeLanguage == lang && x.Order > listMenuParent.Order && x.IsParent == 1 && x.IsCategoryMenu == 1 && x.IdParentMenu == 0);
                if (listMenu != null)
                {
                    ViewBag.NameMenuCategory = "/home/list-category/" + listMenu.Name.UrlFrendly() + "-" + listMenu.CurrentId;
                }
                else
                {
                    ViewBag.NameMenuCategory = "/home/list-category/nghe-thuat-nhiep-anh-khach-san-9";
                }

            }




            return PartialView();
        }
        [HttpPost]
        public ActionResult Subscribe(string id)
        {
            var code = 0;
            var mess = "";
            if (id != null)
            {
                var check = _emailRepository.GetIQueryableItems().FirstOrDefault(x => x.Email.ToLower() == id.ToLower());
                if (check != null)
                {
                    code = -1;

                }
                else
                {
                    var tblEmail = new XEmail { Email = id, CreateDate = DateTime.Now, Active = 1 };
                    _emailRepository.CreateItem(tblEmail);
                    code = 1;
                }
            }
            else
            {
                code = 0;
            }
            return Json(new { code = code });

        }


        public ActionResult Recruitment()
        {
            var lang = "";
            var httpCookie = Request.Cookies["Language"];
            lang = httpCookie != null ? httpCookie.Value : ConfigurationManager.AppSettings["Culture"];


            var listNew = _newGeneralRepository.GetIQueryableItems().Where(x => x.Active == 1 && x.Type==2 && x.CodeLanguage == lang).ToList();


            var titleMeta = "";
            var descriptionMeta = "";
            var keyWordMeta = "";
            var urlMeta = "";
            var imageMeta = "";

            var url = ConfigurationManager.AppSettings["UrlWeb"] + Url.Action("Recruitment", "Home");
            var listMeta = _seoRepository.GetIQueryableItems().FirstOrDefault(x => x.Link.Equals(url) && x.CodeLanguage == lang);
            if (listMeta != null)
            {
                titleMeta = listMeta.Name;
                descriptionMeta = listMeta.Description;
                keyWordMeta = listMeta.KeySearch;
                urlMeta = listMeta.Link;
                imageMeta = !string.IsNullOrEmpty(listMeta.UrlImage) ? (ConfigurationManager.AppSettings["UrlImage"] + listMeta.UrlImage.Split('_')[0] + "/" + String.Format(listMeta.UrlImage, 853)) : ConfigurationManager.AppSettings["UrlWeb"] + "/Template/images/logo-fb.png";
            }

            ViewBag.TitleMeta = titleMeta;
            ViewBag.DescriptionMeta = descriptionMeta;
            ViewBag.KeyWordMeta = keyWordMeta;
            ViewBag.UrlMeta = urlMeta;
            ViewBag.ImageMeta = imageMeta;
            ViewBag.ListNews = listNew;


            return View();
        }

    }

}
