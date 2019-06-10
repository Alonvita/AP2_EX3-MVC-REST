using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Exercise3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Route to save panel
            routes.MapRoute("savePanel", "save/{ip}/{port}/{time}/{timeout}/{filePath}",
            defaults: new { controller = "Home", action = "savePanel" });

            // Route to display Panel
            routes.MapRoute("displayPanel", "display/{ip}/{port}/{time}",
            defaults: new { controller = "Home", action = "displayPanel", time = UrlParameter.Optional });

            // Route to default panel
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
