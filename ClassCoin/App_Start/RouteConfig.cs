using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ClassCoin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(name: "Dashboard_1", url: "Dashboard/{id}",
                defaults: new { controller = "Dashboard", action = "TeacherDashboard", id = UrlParameter.Optional });

            routes.MapRoute(name: "Dashboard_2", url: "Dashboard/{id}",
                defaults: new { controller = "Dashboard", action = "StudentDashboard", id = UrlParameter.Optional });

            routes.MapRoute(name: "Alternate", url: "{action}/{id}", 
                defaults: new { controller = "Home", action = "Landing", id = UrlParameter.Optional });

            routes.MapRoute(name: "Default", url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Landing", id = UrlParameter.Optional });
        }
    }
}
