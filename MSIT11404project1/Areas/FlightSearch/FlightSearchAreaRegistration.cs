using System.Web.Mvc;

namespace MSIT11404project1.Areas.FlightSearch
{
    public class FlightSearchAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FlightSearch";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FlightSearch_default",
                "FlightSearch/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}