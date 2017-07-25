using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MSIT11404project1.Areas.HomeIndex.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeIndex/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}