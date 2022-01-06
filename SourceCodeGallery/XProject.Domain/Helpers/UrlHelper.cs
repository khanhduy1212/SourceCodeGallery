using System;
using System.Configuration;
using System.Web;

namespace XProject.Domain.Helpers
{
    public class UrlHelper
    {
        public static string Root
        {
            get
            {
                if (HttpContext.Current==null)
                {
                    return ConfigurationManager.AppSettings["DomainRoot"];
                }
                var urlHelper = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext);
                var domain = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                //if (!domain.Contains("localhost") && domain.LastIndexOf(":8060") > 0)
                //{
                //    domain = domain.Substring(0, domain.LastIndexOf(":8060"));
                //}
                return domain + urlHelper.Content("~");
            }
        }
    }
}
