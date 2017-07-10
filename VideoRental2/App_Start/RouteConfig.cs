using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VideoRental2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes(); //need for attribute routing
            //conventional routing below. Not as clean as attribute routing
            /*routes.MapRoute(
                name: "MoviesByReleaseDate",
                url: "Movie/ByReleased/{year}/{month}",
                defaults: new { controller = "Movie", action = "ByReleased", year = 2000, month = 01 }
            );*/
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
