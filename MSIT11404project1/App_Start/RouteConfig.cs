using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MSIT11404project1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.IgnoreRoute("{*allashx}", new { allashx = @".*\.ashx(/.*)?" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Member", action = "Login", id = UrlParameter.Optional },
                namespaces:new string[] { "MSIT11404project1.Controllers","MSIT11404project1.Areas.Member.Controllers" }
            ).DataTokens.Add("Area","Member");
        }
       
    }
}
