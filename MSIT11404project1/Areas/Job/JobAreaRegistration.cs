﻿using System.Web.Mvc;

namespace MSIT11404project1.Areas.Job
{
    public class JobAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Job";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Job_default",
                "Job/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}