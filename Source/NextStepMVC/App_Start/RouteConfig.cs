namespace NextStepMVC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "nyrut",
                url: "Home/{mainId}/{subId}",
                defaults: new { controller = "Home", action = "Panel", subId = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" });

            routes.MapRoute(
                name: "moveUnderUp",
                url: "{controller}/{action}/{index}/{underIndex}",
                defaults: new { controller = "Edit", action = "MoveUnderMenuIndexUp", index = UrlParameter.Optional, underIndex = UrlParameter.Optional });

            routes.MapRoute(
                name: "Login",
                url: "{action}",
                defaults: new { controller = "Admin", action = "LogOn" });
        }
    }
}
