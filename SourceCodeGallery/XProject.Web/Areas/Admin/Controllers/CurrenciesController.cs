using System.ComponentModel;
using System.Web.Mvc;
using XProject.Domain.Abstract;
using XProject.Domain.Entities;
using XProject.Web.Infrastructure.Helpers;
using XProject.Web.Infrastructure.Filters;

namespace XProject.Web.Areas.Admin.Controllers
{
    [RequestAuthorize]
    public class CurrenciesController : Controller
    {
        private readonly IUnitRepository _unitRepo;

        public CurrenciesController(IUnitRepository unitRepo)
        {
            this._unitRepo = unitRepo;
        }

        //
        // GET: /Currencies/
        [DisplayName(@"Currencies management")]
        public ViewResult Index()
        {
            return View(_unitRepo.GetAllCurrencies());
        }

        //
        // GET: /Currencies/Details/5

        public ViewResult Details(int id)
        {
            return View(_unitRepo.GetCurrency(id));
        }

        //
        // GET: /Currencies/Create

        public ActionResult Create()
        {
            return View(new Currency());
        }

        //
        // POST: /Currencies/Create

        [HttpPost]
        public ActionResult Create(Currency currency)
        {
            if (ModelState.IsValid && _unitRepo.CreateCurrency(currency) != null)
            {
                MoneyHelper.SetDefaultCurrency();
                return RedirectToAction("Index");
            }

            return View(currency);
        }

        //
        // GET: /Currencies/Edit/5

        public ActionResult Edit(int id)
        {
            return View(_unitRepo.GetCurrency(id));
        }

        //
        // POST: /Currencies/Edit/5

        [HttpPost]
        public ActionResult Edit(Currency currency)
        {
            if (ModelState.IsValid && _unitRepo.UpdateCurrency(currency))
            {
                MoneyHelper.SetDefaultCurrency();
                return RedirectToAction("Index");
            }

            return View(currency);
        }

        //
        // GET: /Currencies/Delete/5
        [DisplayName(@"Delete")]
        public ActionResult Delete(int id)
        {
            return View(_unitRepo.GetCurrency(id));
        }

        //
        // POST: /Currencies/Delete/5

        [HttpPost, ActionName("Delete")]
        [DisplayName(@"Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _unitRepo.DeleteCurrency(id);
            MoneyHelper.SetDefaultCurrency();
            return RedirectToAction("Index");
        }
    }
}
