using XProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace XProject.Domain.Abstract
{
    public interface IUnitRepository
    {

        #region Currency
        IEnumerable<Currency> GetAllCurrencies();
        Currency GetCurrency(int currencyID);
        Currency GetCurrency(string name);
        Currency CreateCurrency(Currency info);
        Currency QuickCreateCurrency(string name);
        bool UpdateCurrency(Currency info);
        bool DeleteCurrency(int currencyID);
        Currency GetDefaultCurrency();
        void SetDefaultCurrency(int currencyID);
        #endregion Currency

      
        #region Language
        IEnumerable<Language> GetAllLanguages();
        Language GetLanguage(string value);
        Language GetLanguage(int id);
        Language CreateLanguage(Language language);
        bool UpdateLanguage(Language language);
        bool DeleteLanguage(int languageId);
        #endregion Language

    }
}
