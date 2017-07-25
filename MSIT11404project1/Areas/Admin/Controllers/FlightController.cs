using MSIT11404project1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using MSIT11404project1.Areas.FlightSearch.ViewModel;

namespace MSIT11404project1.Areas.Admin.Controllers
{
    public class FlightController : Controller
    {
        MSIT11404Entities db = new MSIT11404Entities();
        IRepository<Flights> flight = new Repository<Flights>();
        IRepository<Airlines> airline = new Repository<Airlines>();
        IRepository<TicketInfo> ticket = new Repository<TicketInfo>();
        IRepository<ScheduleDetails> schedule = new Repository<ScheduleDetails>();

        [HttpGet]
        public ActionResult GetAllFlights(int page = 1, int perpage = 10)
        {
            var list = db.Flights.ToList();

            //轉到另外一個Action
            return View(list.ToList().ToPagedList(page, perpage));
        }

        public ActionResult AppendFlight()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AppendFlight(Flights _flight)
        {
            flight.Create(_flight);
            return RedirectToAction("GetAllFlight", "Flight", new { Area = "Admin" });
        }

        [HttpGet]
        public ActionResult FlightEdit(int id = 0)
        {
            //將資料傳給View
            return View(flight.GetByID(id));
        }

        [HttpPost]
        public ActionResult FlightEdit(Flights _flight, Airlines _airline, TicketInfo _ticket, ScheduleDetails _schedule)
        {
            flight.Update(_flight);
            //airline.Update(_airline);
            //ticket.Update(_ticket);
            //schedule.Update(_schedule);

            return RedirectToAction("GetAllFlight", "Flight", new { Area = "Admin" });
        }

        public ActionResult FlightDelete(int id = 0)
        {
            flight.Delete(flight.GetByID(id));
            //轉到另外一個Action
            return RedirectToAction("GetAllFlight", "Flight", new { Area = "Admin" });
        }

        // GET: FlightSearch/Flight
        public ActionResult SkyScannerWidget()
        {
            return View();
        }
    }
}