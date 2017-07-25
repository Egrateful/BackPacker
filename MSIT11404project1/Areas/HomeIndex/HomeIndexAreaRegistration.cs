using System.Web.Mvc;

namespace MSIT11404project1.Areas.HomeIndex
{
    public class HomeIndexAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "HomeIndex";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "HomeIndex_default",
                "HomeIndex/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}