using System.Web.Mvc;
using XProject.Web.Infrastructure.Utility;

namespace XProject.Web.Infrastructure.Filters
{
    public class AuditAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Log.Info();
            base.OnActionExecuting(filterContext);
        }
    }
}
