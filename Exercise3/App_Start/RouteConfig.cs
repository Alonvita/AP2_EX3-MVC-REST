using System.Web.Mvc;
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
                url: "display/{ipOffset0}.{ipOffset1}.{ipOffset2}.{ipOffset3}/{port}/{time}",
                 defaults: new { controller = "Home", action = "displayPanel", time = 0 }
            );

            routes.MapRoute(
                name: "saveDisplay",
                url: "save/{ip}/{port}/{second}/{time}/{name}",
                defaults: new { controller = "Home", action = "saveDisplay" }
            );

            routes.MapRoute(
                name: "displayFile",
                url: "display/{name}/{interval}",
                defaults: new { controller = "Home", action = "displayFile" }
            );

            routes.MapRoute(
            name: "Default",
            url: "{action}/{id}",
            defaults: new { controller = "Home", action = "Def", id = UrlParameter.Optional }
            );
        }

    }

}
