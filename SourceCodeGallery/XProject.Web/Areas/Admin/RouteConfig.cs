using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XProject.Web.Areas.Admin
{
    internal class RouteConfig
    {
        internal static void RegisterRoutes(AreaRegistrationContext context)
        {
            // context.MapRoute(
            //     name: "Logout",
            //     url: "Logout",
            //     defaults: new { controller = "Account", action = "Logout", id = UrlParameter.Optional },
            //     namespaces: new[] { "XProject.Web.Areas.Admin.Controllers" }
            // ).DataTokens["UseNamespaceFallback"] = false;
            context.MapRoute(
                name: "BypassAuth",
                url: "n/{token}",
                defaults: new { controller = "Account", action = "BypassLogin", id = UrlParameter.Optional },
                namespaces: new[] { "XProject.Web.Areas.Admin.Controllers" }
                ).DataTokens["UseNamespaceFallback"] = false; ;
            // context.MapRoute(
            //    name: "AccountLogin",
            //    url: "account/{action}/{id}",
            //    defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
            //    namespaces: new[] { "XProject.Web.Areas.Admin.Controllers" }
            //).DataTokens["UseNamespaceFallback"] = false;
            context.MapRoute(
                "Admin_default",
                "admin/{controller}/{action}/{id}",
                new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "XProject.Web.Areas.Admin.Controllers" }
            ).DataTokens["UseNamespaceFallback"] = false;

        }
    }
}