using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MSIT11404project1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 設定和服務

            // Web API 路由
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute("Admin_defaultAPI", "api/Admin/{controller}/{id}", new { id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                name: "TravelApi",
                routeTemplate: "api/Travel/{controller}/{searchby}",
                defaults: new { searchby = RouteParameter.Optional }
             );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}
