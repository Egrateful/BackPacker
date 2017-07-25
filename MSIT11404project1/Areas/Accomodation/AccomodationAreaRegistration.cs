using System.Web.Mvc;

namespace MSIT11404project1.Areas.Accomodation
{
    public class AccomodationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Accomodation";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            

            context.MapRoute(
                "Accomodation_default",
                "Accomodation/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}