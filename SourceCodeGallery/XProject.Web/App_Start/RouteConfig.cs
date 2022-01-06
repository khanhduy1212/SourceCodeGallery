using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace XProject.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
               name: "home",
               url: "home",
               defaults: new { controller = "Home", action = "Index" });
            routes.MapRoute(
              name: "ListCategoy",
              url: "home/list-category/{id}",
              defaults: new { controller = "Home", action = "ListCategoy", id = UrlParameter.Optional });
            routes.MapRoute(
             name: "DetailCategoy",
             url: "home/category/{id}",
             defaults: new { controller = "Home", action = "DetailCategoy", id = UrlParameter.Optional });
 routes.MapRoute(
             name: "DetailProduct",
             url: "home/detail/{id}",
             defaults: new { controller = "Home", action = "DetailProduct", id = UrlParameter.Optional });

 routes.MapRoute(
             name: "DetailNew",
             url: "home/detail-new/{id}",
             defaults: new { controller = "Home", action = "DetailNew", id = UrlParameter.Optional });
            routes.MapRoute(
                "/",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "XProject.Web.Controllers" });
        }
        public class SeoFriendlyRoute : Route
        {
            public SeoFriendlyRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
                : base(url, defaults, routeHandler)
            {
            }

            public override RouteData GetRouteData(HttpContextBase httpContext)
            {
                var routeData = base.GetRouteData(httpContext);

                if (routeData != null)
                {
                    if (routeData.Values.ContainsKey("id"))
                        routeData.Values["id"] = GetIdValue(routeData.Values["id"]);
                }

                return routeData;
            }

            private object GetIdValue(object id)
            {
                if (id != null)
                {
                    string idValue = id.ToString();

                    var regex = new Regex(@"^(?<id>\d+).*$");
                    var match = regex.Match(idValue);

                    if (match.Success)
                    {
                        return match.Groups["id"].Value;
                    }
                }

                return id;
            }

        }
    }
}
