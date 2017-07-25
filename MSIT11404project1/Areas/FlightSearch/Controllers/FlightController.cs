using MSIT11404project1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using MSIT11404project1.Areas.FlightSearch.ViewModel;

namespace MSIT11404project1.Areas.FlightSearch.Controllers
{
    public class FlightController : Controller
    {
        MSIT11404Entities db = new MSIT11404Entities();
        
        IRepository<Models.Flights> flights = new Repository<Models.Flights>();
        IRepository<Models.Airlines> airlines = new Repository<Models.Airlines>();
        IRepository<Models.TicketInfo> tickets = new Repository<Models.TicketInfo>();
        IRepository<Models.ScheduleDetails> schedule = new Repository<Models.ScheduleDetails>();

        public ActionResult SearchFlight()
        {
              return View();
        }

        [HttpPost]
        public ActionResult SearchFlight(SearchFlightViewModel _sf, int page=1)
        {
            //轉到另外一個Action
            return RedirectToAction("FlightList", "Flight", new { Area = "FlightSearch", FromAirportCity = _sf.FromAirportCity, ToAirportCity = _sf.ToAirportCity, DepartureDate = _sf.DepartureDate, ReturnDate = _sf.ReturnDate });
            //return RedirectToAction("SearchResult", "Flight", new { Area = "FlightSearch", FromAirportCity = _sf.FromAirportCity, ToAirportCity = _sf.ToAirportCity, DepartureDate = _sf.DepartureDate, ReturnDate = _sf.ReturnDate });
        }

        [HttpGet]
        public ActionResult FlightList(string FromAirportCity, string ToAirportCity, DateTime DepartureDate, DateTime ReturnDate,int page = 1)
        {
            List<Flights> sf = new List<Flights>();
            //sf = db.Flights.Where(f => f.FromAirportCity == FromAirportCity && f.ToAirportCity == ToAirportCity && f.DepartureDate == DepartureDate && f.ReturnDate == ReturnDate).ToList();
            sf = db.Flights.Where(f => f.ToAirportCity == ToAirportCity && f.DepartureDate == DepartureDate && f.ReturnDate == ReturnDate && (f.FromAirportCity == "New York" || f.FromAirportCity == "Newark")).ToList();

            int perpage = 6;
            var list = sf.ToList().ToPagedList(page, perpage);
            if (sf.Count == 0)
            {
                string nodata = string.Format("很抱歉，沒有任何航班符合您的篩選條件。請更改您的過濾設置查看結果。");
                return View(nodata);
            }
            else
            {
                 return View(list);
            }
           
            
        }

        public ActionResult SearchFlight2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchFlight2(SearchFlightViewModel _sf, int page = 1)
        {
            //轉到另外一個Action
            return RedirectToAction("SearchResult", "Flight", new { Area = "FlightSearch", FromAirportCity = _sf.FromAirportCity, ToAirportCity = _sf.ToAirportCity, DepartureDate = _sf.DepartureDate, ReturnDate = _sf.ReturnDate });
        }
        [HttpGet]
        public ActionResult SearchResult(string FromAirportCity, string ToAirportCity, DateTime DepartureDate, DateTime ReturnDate, int page = 1)
        {
            List<Flights> sf = new List<Flights>();
            //sf = db.Flights.Where(f => f.FromAirportCity == FromAirportCity && f.ToAirportCity == ToAirportCity && f.DepartureDate == DepartureDate && f.ReturnDate == ReturnDate).ToList();
            sf = db.Flights.Where(f => f.ToAirportCity == ToAirportCity && f.DepartureDate == DepartureDate && f.ReturnDate == ReturnDate).ToList();

            int perpage = 6;
            var list = sf.ToList().ToPagedList(page, perpage);
            if (sf.Count == 0)
            {
                string nodata = string.Format("很抱歉，沒有任何航班符合您的篩選條件。請更改您的過濾設置查看結果。");
                return View(nodata);
            }
            else
            {
                return View(list);
            }

        }

        public ActionResult OriginByWidget()
        {
            return View();
        }

        //public ActionResult Airlines()
        //{
        //    //ViewBag.Message = "Hello";
        //    return PartialView(db.Airlines.ToList());
        //}

        
        [HttpPost]
        public ActionResult Browse1(DisplayFlightViewModel vm)
        {
            return View(vm);
        }

       

        // GET: FlightSearch/Flight
        //public ActionResult SkyScannerWidget()
        //{
        //    return View();
        //}
    }
}