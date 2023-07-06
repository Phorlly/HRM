using HRM.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HRM
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            // Add your custom routes here
            routes.MapRoute(
                name: "home",
                url: "admin-dashboard",
                defaults: new { controller = "Home", action = "Index" }
            );

            // Default route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Auth", action = "LogIn", id = UrlParameter.Optional }
            ).RouteHandler = new LowerCaseRouteHandler();
        }
    }
}
