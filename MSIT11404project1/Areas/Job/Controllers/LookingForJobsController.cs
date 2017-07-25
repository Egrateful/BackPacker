using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSIT11404project1.Models;

namespace MSIT11404project1.Areas.Job.Controllers
{
    public class LookingForJobsController : Controller
    {
        private MSIT11404Entities db = new MSIT11404Entities();

        // GET: Job/LookingForJobs
        public ActionResult Index()
        {
            return View(db.JobList.ToList());
        }
    }
}