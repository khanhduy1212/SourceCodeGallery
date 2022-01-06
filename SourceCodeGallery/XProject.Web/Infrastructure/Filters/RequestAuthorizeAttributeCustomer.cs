using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using XProject.Web.Infrastructure.Utility;
using XProject.Domain.Abstract;
using XProject.Domain.Entities;
namespace XProject.Web.Infrastructure.Filters
{
    public class RequestAuthorizeAttributeCustomer : FilterAttribute, IAuthorizationFilter
    {
        
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            RouteData rd = filterContext.RouteData;
          
            ////////////////////////////////////////
            // Bypass if this action has excluded the RequestAuthorize filter
            // aka allow anonymous access
            ////////////////////////////////////////

            var filters = FilterProviders.Providers.GetFilters(filterContext.Controller.ControllerContext, filterContext.ActionDescriptor);
            if (!filters.Select(filter => filter.Instance.GetType()).Contains(typeof(RequestAuthorizeAttributeCustomer)))
                return;

            ////////////////////////////////////////
            // Login by token
            ////////////////////////////////////////

            string token = filterContext.RequestContext.HttpContext.Request.QueryString.Get("token");
         
            if (token != null )
            {
                
                if (TokenLogin.ValidateToken(token))
                    return;
                HandleUnauthorizedRequest(filterContext);
            }

            ////////////////////////////////////////
            // Normal request
            ////////////////////////////////////////
            if (HttpContext.Current.Session == null )
            {
                HandleUnauthorizedRequest(filterContext);
            }
            else
            {
          
                //if (CurrentCustomer.IsAuthenticated && CurrentCustomer.IdentityInfo.TypeCustomer == 1)
                //{
                
                //    var em = CurrentCustomer.IdentityInfo.Employer;
                //    if (em != null)
                //    {
                //        if (CurrentCustomer.IdentityInfo.Customer.UpdateInfo == 0)
                //        {
                //            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                //            {
                //                controller = "BDSEmployer",
                //                action = "EmployerEditInfo",
                //                id = em.ID

                //            }));
                //        }
                //    }
                //}
                //if (CurrentCustomer.IsAuthenticated && CurrentCustomer.IdentityInfo.TypeCustomer == 2)
                //{
                //    var per = CurrentCustomer.IdentityInfo.Personal;
                //    if (per != null)
                //    {
                //        if (CurrentCustomer.IdentityInfo.Customer.UpdateInfo==0)
                //        {
                //            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                //            {
                //                controller = "BDSPersonal",
                //                action = "EditPersonal",
                //                id = per.ID
                //            }));

                //        }
                //    }
                //}
            }


            //SetLanguages();
        }

        protected void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var urlHelper = new UrlHelper(filterContext.RequestContext);
            string loginUrl = urlHelper.Action("Index", "Home", new { area = "" });

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                    {
                        Data = new
                            {
                                Error = "NotAUthorized",
                                LogOnUrl = loginUrl
                            },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                filterContext.HttpContext.Response.StatusCode = 403;
            }
            else
            {
                //if (CurrentCustomer.IsAuthenticated)
                //{
                //    if (filterContext.IsChildAction)
                //        filterContext.Result = new EmptyResult();
                //    else
                //        filterContext.Result = new ViewResult { ViewName = "Index" };
                //}
                //else
                //{


                //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                //    {
                //        controller = "Home",
                //        action = "Index",
                //        area = "",
                //        returnUrl = filterContext.HttpContext.Request.RawUrl
                //    }));
                //}
            }
        }


        
    }
}
