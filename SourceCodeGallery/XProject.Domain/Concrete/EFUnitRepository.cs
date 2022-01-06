using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using XProject.Domain.Abstract;
using XProject.Domain.Entities;
using Humanizer;
using NS.Entity;
using ServiceStack;
using WebGrease.Css;

namespace XProject.Domain.Concrete
{
    public class EFUnitRepository : Repository, IUnitRepository
    {
        public EFUnitRepository(DbContext db)
            : base(db)
        {
        }

        #region Currencies

        public IEnumerable<Currency> GetAllCurrencies()
        {
            return GetAll<Currency>().AsEnumerable();
        }

        public Currency GetCurrency(int currencyID)
        {
            return Get<Currency>(currencyID);
        }

        public Currency GetCurrency(string name)
        {
            return Get<Currency>(m => String.Equals(m.Name.Trim(), name.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }

        public Currency CreateCurrency(Currency info)
        {
            Currency currency = Create(info);

            if (currency.IsDefault)
                SetDefaultCurrency(currency.ID);

            return currency;
        }

        public Currency QuickCreateCurrency(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            var currency = Get<Currency>(c => c.Name != null && c.Name.ToUpper() == name.ToUpper());
            if (currency == null)
            {
                currency = new Currency { Name = name, Symbol = "$", IsDefault = false };
                currency = Create(currency);
            }

            return currency;
        }

        public bool UpdateCurrency(Currency info)
        {
            bool result = Update(info);

            if (info.IsDefault)
                SetDefaultCurrency(info.ID);

            return result;
        }

        public bool DeleteCurrency(int currencyID)
        {
            return Delete<Currency>(currencyID);
        }

        public Currency GetDefaultCurrency()
        {
          
            return Get<Currency>(c => c.IsDefault) ??
                   Get<Currency>(c => true);
        }

        public void SetDefaultCurrency(int currencyID)
        {
            Currency currency = GetCurrency(currencyID);
            if (currency != null)
            {
                Currency baseCurrency = GetDefaultCurrency();
                if (baseCurrency != null && baseCurrency.ID != currencyID)
                {
                    baseCurrency.IsDefault = false;
                    UpdateCurrency(baseCurrency);

                    currency.IsDefault = true;
                    UpdateCurrency(currency);
                }
            }
        }
        #endregion

        

        
       

       

        #region Language Setting

        public IEnumerable<Language> GetAllLanguages()
        {
            return GetAll<Language>().ToList();
        }

        public Language GetLanguage(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            return
                Get<Language>(
                    m => m.Value.ToLower().Contains(value.ToLower()) || value.ToLower().Contains(m.Value.ToLower()));
        }
        public Language GetLanguage(int id)
        {
            return Single<Language>(m => m.ID == id);
        }

        public Language CreateLanguage(Language language)
        {
            language.Value = language.Value.Trim().ToLower();
            var cs = new CultureInfo(language.Value);
            var r = new RegionInfo(cs.LCID);
            language.Image = "flag-icon flag-icon-" + r.TwoLetterISORegionName.ToLower();
            language.DisplayName = cs.Parent.EnglishName;
            return Create(language);
        }

        public bool UpdateLanguage(Language language)
        {
            return Update(language);
        }

        public bool DeleteLanguage(int languageId)
        {
            return Delete<Language>(languageId);
        }
        #endregion
    }
}
