using System.Linq;
using System.Web.Mvc;
using XProject.Web.Infrastructure.Filters;

namespace XProject.Web.Areas.Admin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            //filters.Add(new RequestAuthorizeAttribute());// not working
        }
    }
}
