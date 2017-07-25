using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSIT11404project1.Models;
using PagedList;
using PagedList.Mvc;


namespace MSIT11404project1.Areas.Admin.Controllers
{
    public class AirlineController : Controller
    {
        MSIT11404Entities1 db = new MSIT11404Entities1();
        IRepository<Airlines> airline = new Repository<Airlines>();

        public ActionResult GetAllAirline(int page = 1, int perpage = 8)
        {
            var list = db.Members.ToList();

            //轉到另外一個Action
            return View(list.ToList().ToPagedList(page, perpage));
        }

        // GET: Member/Default
        [HttpGet]
        public ActionResult AirlineInsert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AirlineInsert(Airlines _airline)
        {
            airline.Create(_airline);
            return RedirectToAction("GetAllAirline", "Airline", new { Area = "Admin" });
        }

        [HttpGet]
        public ActionResult AirlineEdit(int id = 0)
        {
            //將資料傳給View
            return View(airline.GetByID(id));
        }

        [HttpPost]
        public ActionResult AirlineEdit(Airlines _airline)
        {
            airline.Update(_airline);
            return RedirectToAction("GetAllAirline", "Airline", new { Area = "Admin" });
        }

        public ActionResult AirlineDelete(int id = 0)
        {
            airline.Delete(airline.GetByID(id));
            //轉到另外一個Action
            return RedirectToAction("GetAllAirline", "Airline", new { Area = "Admin" });
        }

    }
}
