using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace testRAF
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //     name: "TmdbApi",
            //     url: "TmdbApi/{id}/",
            //     defaults: new { controller = "TmdbApi", action = "GetPerson", id = "" },
            //     constraints: new { id = @"^[0-9]+$" }
            // );

            //routes.MapRoute(
            //    name: "TmdbApiPaging",
            //    url: "TmdbApi/{peopleName}/{page}",
            //    defaults: new { controller = "TmdbApi", action = "Index", peopleName = "", page = "" },
            //    constraints: new { peopleName = @"^[a-zA-Z]+$", page = @"^[0-9]+$" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }

    }
}
