using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSIT11404project1.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace MSIT11404project1.Areas.Transaction.Controllers
{
    public class putController : Controller
    {
        // GET: Transaction/put
        public ActionResult Index()
        {

            IRepository<Products> pros = new Repository<Products>();
            string a =  JsonConvert.SerializeObject(pros.GetAll().ToList());
            string path = @"D:\pro.txt";
            StreamWriter one = System.IO.File.CreateText(path);
            one.WriteLine(a);


            return Json(JsonConvert.SerializeObject(pros.GetAll().ToList()),JsonRequestBehavior.AllowGet);
        }
    }
}