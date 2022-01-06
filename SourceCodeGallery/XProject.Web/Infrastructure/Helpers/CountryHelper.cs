using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NS.Mvc.Helpers;
using XProject.Domain.Abstract;
using XProject.Domain.Entities;

namespace XProject.Web.Infrastructure.Helpers
{
    public static class CountryHelper
    {
        //public static IEnumerable<SelectListItem> GetAllCountries()
        //{

        //    var countries = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
        //         .ToSelectList(x => new RegionInfo(x.LCID).Name, x => new RegionInfo(x.LCID).EnglishName + " [" + new RegionInfo(x.LCID).Name + "]")
        //                         .GroupBy(c => c.Value)
        //                         .Select(c => c.First())
        //                         .OrderBy(x => x.Text);
        //    return countries;
        //}

      /*  public static IEnumerable<Country> GetAllCountries() {
            var countryRepo = DependencyHelper.GetService<IGeneralRepository<Country>>();
            var countries = countryRepo.GetAllTEntities(x=> x.IsPublished).ToList();
            return countries;
        }*/

        public static IEnumerable<string> GetAllCountriesForDropdown()
        {
            //create a new Generic list to hold the country names returned WorldWide
            List<string> cultureList = new List<string>();

            //create an array of CultureInfo to hold all the cultures found, these include the users local cluture, and all the
            //cultures installed with the .Net Framework

            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(c => c.Name.Contains("-"));//CultureInfo.GetCultures(CultureTypes.AllCultures & CultureTypes.NeutralCultures);

            //loop through all the cultures found
            foreach (CultureInfo culture in cultures.Where(m => !string.IsNullOrEmpty(m.Name)))
            {
                try
                {
                    var region = new RegionInfo(culture.LCID);
                    if (!(cultureList.Contains(region.EnglishName)))
                        cultureList.Add(region.EnglishName);
                }
                catch (Exception)
                {
                }
            }
            return cultureList.OrderBy(m => m);
        }
    }
}