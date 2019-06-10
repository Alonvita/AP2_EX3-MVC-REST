﻿using System.Web.Mvc;
using System.Web.Routing;

namespace REST_WEB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "displayPanel",
                url: "display/{ip1}.{ip2}.{ip3}.{ip4}/{port}/{time}",
                 defaults: new { controller = "Home", action = "displayPanel", time = 0 }
            );

            routes.MapRoute(
                name: "savePanel",
                url: "save/{ip}/{port}/{second}/{time}/{name}",
                defaults: new { controller = "Home", action = "savePanel" }
            );

            routes.MapRoute(
                name: "drawPath",
                url: "display/{name}/{interval}",
                defaults: new { controller = "Home", action = "drawPath" }
            );

            routes.MapRoute(
            name: "Default",
            url: "{action}/{id}",
            defaults: new { controller = "My", action = "Def", id = UrlParameter.Optional }
    );
        }

    }

}
