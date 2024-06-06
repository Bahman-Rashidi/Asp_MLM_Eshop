using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MLM_app
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // To display content as a custom URL in the browser address bar
            routes.MapMvcAttributeRoutes();

            // For the sitemap to be accessible to search engines
            routes.MapRoute("sitemap", "sitemap", new { controller = "Sitemap", action = "Sitemap" });
            routes.MapRoute("sitemap.xml", "sitemap.xml", new { controller = "Sitemap", action = "Sitemap" });


            routes.MapRoute("RSS", "rss", new { controller = "Home", action = "Rss" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
