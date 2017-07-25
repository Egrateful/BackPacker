using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSIT11404project1.Models;
namespace MSIT11404project1.Areas.Travel.Controllers
{
    public class TravelController : Controller
    {
        IRepository<Travels> travel = new Repository<Travels>();
        IRepository<Members> member = new Repository<Members>();
        IRepository<Message> message = new Repository<Message>();
        // GET: Travel/Travel
        public ActionResult Index()
        {
            return View(travel.GetAll());
        }


        //需要一個memberid
        [HttpGet]
        public ActionResult Create(int id )
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Travels t)
        {
            if (t.Detail == null)
                t.Detail = "new";
            t.MemberID = Convert.ToInt32(Request.Cookies["MemberID"].Value);
            t.CreateDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            t.LastModifiedDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            travel.Create(t);
            return RedirectToAction("Edit", "Travel", new { Area = "Travel", id = t.TravelID });
        }

        [HttpGet]
        public ActionResult UserTravels(int id)
        {
            return View(travel.GetAll().Where(t => t.MemberID == id));
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Travels t = travel.GetByID(id);
            var mid = t.MemberID;
            for (int i = 0; i < t.Message.Count; i++)
            {
                Message m = message.GetByID(t.Message.ToList()[0].MemberID);
                message.Delete(m);
            }
            travel.Delete(t);
            return RedirectToAction("UserTravels","Travel",new {Area="Travel",id= mid });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Travels t = travel.GetByID(id);
            return View(t);
        }
        [HttpPost]
        public ActionResult Edit(Travels t)
        {
            t.LastModifiedDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            travel.Update(t);
            return RedirectToAction("Index", "Travel", new { Area = "Travel" });
        }

        [HttpGet]
        public ActionResult TravelDetail(int id)
        {
            Travels t = travel.GetByID(id);
            t.ViewCount++;
            travel.Update(t);
            return View(t);
        }
        //[HttpGet]
        //public ActionResult TravelDetail(int id)
        //{
        //    Travels t = travel.GetByID(id);
        //    return View(t);
        //}

        //[HttpGet]
        //public ActionResult CreateDetail(int id=1)
        //{
        //    Travels t = travel.GetByID(id);
        //    return View(t);
        //}
        //[HttpPost]
        //public ActionResult CreateDetail(Travels t)
        //{
        //    travel.Update(t);
        //    return RedirectToAction("Index","Travel",new {Area="Travel" });
        //}
    }

}