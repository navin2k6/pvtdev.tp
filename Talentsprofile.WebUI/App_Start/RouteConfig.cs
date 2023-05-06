using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Talentsprofile.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Home",
                url: "{action}",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "Feedback",
                url: "{controller}/{action}",
                defaults: new { controller = "Feedback", action = "Index" }
            );

            routes.MapRoute(
                name: "Contact",
                url: "{controller}/{action}",
                defaults: new { controller = "Contact", action = "Index" }
            );

            routes.MapRoute(
                name: "SearchJob",
                url: "search/{controller}/{action}/id",
                defaults: new { controller = "Jobs", action = "Index", id = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //    name: "Employer",
            //    url: "emp/{controller}/{action}/id",
            //    defaults: new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            //);


            routes.MapRoute(
                name: "Default",
                url: "{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //    name: "Error",
            //    url: "{action}/{id}",
            //    defaults: new { controller = "Error", action = "Index", id = UrlParameter.Optional }
            //);

            //routes.MapRoute(name: "Talent",
            //    url: "{controller}/{action}/{val}",
            //    defaults: new { controller = "Talent", action = "Index", val = UrlParameter.Optional });
        }
    }
}
