using System;
using System.Linq;
using XProject.Domain.Abstract;
using XProject.Domain.Entities;
using XProject.Web.Infrastructure.Utility;

namespace XProject.Web.Infrastructure.Helpers
{
    public static class MoneyHelper
    {
        private static Currency _baseCurrency;
        public static Currency DefaultCurrency
        {
            get
            {
                if (_baseCurrency == null)
                {
                    var repo = DependencyHelper.GetService<IUnitRepository>();
                    _baseCurrency = repo.GetDefaultCurrency();
                }

                return _baseCurrency;
            }
        }
        public static void SetDefaultCurrency()
        {
            var repo = DependencyHelper.GetService<IUnitRepository>();
            _baseCurrency = repo.GetDefaultCurrency();
        }

        public static string ToMoneyString(this decimal money)
        {
            return DefaultCurrency.Format(money);
        }
        public static string ToMoneyStringWithNoSymbol(this decimal money)
        {
            return DefaultCurrency.Format(money, false);
        }
        //public static string ToMoneyString(this decimal money, Currency currency)
        //{
        //    return currency.Format(money);
        //}

        public static decimal DecimalFormat(this decimal money)
        {
            return decimal.Round(money, 2, MidpointRounding.AwayFromZero);
        }
        public static string AsNumber(this int value)
        {
            return value.ToString("N0");
        }

        public static string AsMoney(this decimal value, Currency currency, bool withUnit = false)
        {
            if (currency == null)
                throw new ArgumentNullException("currency", "Currency cannot be null.");

            return currency.Format(value, withUnit);
        }
    }
}
