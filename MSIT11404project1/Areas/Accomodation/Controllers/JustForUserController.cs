using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSIT11404project1.Models;
using MSIT11404project1.Areas.ViewModel;
using MSIT11404project1.Areas.Accomodation.Models;
using MSIT11404project1.Areas.Accomodation.ViewModel;
using System.IO;
namespace MSIT11404project1.Areas.Accomodation.Controllers
{
    public class JustForUserController : Controller
    {
        IRepository<HouseMain> houses = new Repository<HouseMain>();
        IRepository<ImageForRoom> images = new Repository<ImageForRoom>();
        IRepository<HouseQuality> quality = new Repository<HouseQuality>();
        // GET: Accomodation/JustForUser
        public ActionResult Index()
        {
            InsertHouse veiws = new InsertHouse();
            veiws.EntitiesHouses = houses.GetAll().Where(n=>n.IsActive==true&&n.IsFreeze!=true);
            veiws.EntityImages = images.GetAll();
            return View(veiws);
        }

        //房間搜尋

        public ActionResult searchRoom() {
            string places = Convert.ToString(Request.Form["place"]);
            int peo = Convert.ToInt32(Request.Form["people"]);
            DateTime fdate = Convert.ToDateTime(Request.Form["fdate"]);
            DateTime edate = Convert.ToDateTime(Request.Form["edate"]);
            

            HouseMainRepository houses = new HouseMainRepository();

            return View(houses.SearchResaults2(places, peo, fdate, edate));
        }



        public ActionResult houseDetails(int? id)
        {
            housedetailsview views = new housedetailsview(Convert.ToInt32(id));
            

            return View(views);
        }

        

        //fullcalendar's class 
        public class bookings{
            public string title { get; set; }
            public string start { get; set; }
            public string end { get; set; }
            
        }


        //將日曆事件與訂單同步 屬於房東的
        public ActionResult bookingtest() {
            //可以點進去 行事曆的功能!代表他已經有房東資格
            int memid = Convert.ToInt32(Request.Cookies["MemberID"].Value);
            BookingRepository books = new BookingRepository();
            List<Booking> lists = new List<Booking>();
            List<bookings> alistforc = new List<bookings>();
            HouseMainRepository houses = new HouseMainRepository();
            List<int> hkeys = new List<int>();
            if (houses.FindhouseKey(memid,out hkeys))//先假設MemberID=1
            {
                if (books.FindbookbyHouseKey(hkeys, out lists))
                {
                    var list2 = lists.Where(n => n.IsConfirm == true && n.IsRejected != true);
                    foreach (var items in list2)
                    {
                        bookings one = new bookings();
                        one.title = items.HouseMain.HouseName + "/" + items.BedDtails.SpaceIntro;
                        one.start = String.Format("{0:yyyy-MM-dd}", items.FromDate);
                        one.end = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(items.EndDate).AddDays(1));
                        alistforc.Add(one);
                    }

                }
                else {
                    return Json(null,JsonRequestBehavior.AllowGet);

                }
                //var jsonobj =JsonConvert.SerializeObject(strlist);

                return Json(alistforc, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(null,JsonRequestBehavior.AllowGet);
            }
        }

        //public class visiblerage{
        //public string start { get; set; }
        //public string end { get; set; }
        //}

        //這是有關預定的
        public ActionResult calforbooks() {
            int hkey = Convert.ToInt32(Request.QueryString["hkey"]);
            int bedspace = Convert.ToInt32(Request.QueryString["bspace"]);
            List<bookings> one = new List<bookings>();
            BookingRepository boo = new BookingRepository();
            List<Booking> books = boo.FindbyhkeyBedspace(hkey, bedspace);
            if (books.Count != 0)
            {
                var listsforcon = books.Where(n => n.IsConfirm == true);
                if (listsforcon.Count() > 0)
                {
                    foreach (var i in listsforcon)
                    {
                        bookings two = new bookings();


                        two.title = "已預定";
                        two.start = String.Format("{0:yyyy-MM-dd}", i.FromDate);

                        
                        two.end = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(i.EndDate).AddDays(1));
                        one.Add(two);
                    }

                    return Json(one, JsonRequestBehavior.AllowGet);
                }
                else {
                    return null;
                }
            }
            return null;
            
        }


        //當使用者點了日期會判斷是不是在開放日期內
        public ActionResult findateisEvent() {
            int hkey = Convert.ToInt32(Request.QueryString["hkey"]);
            int bedspace = Convert.ToInt32(Request.QueryString["bspace"]);
            DateTime dates = Convert.ToDateTime(Request.QueryString["date"]);
            CalendarRepository crepo = new CalendarRepository();
            List<Calendar> listcal = crepo.FindCByHkeyBedspace(hkey, bedspace);
            string results = "";
            if (listcal.Count > 0) { 
            if (listcal.Where(n => n.EndDate < dates || n.FromDate > dates).ToList().Count> 0)
            {
                results = "1";

            }
            else {
                results = "0";
            }
            }
            return Content(results);
        }


        //這是有關開放日期的
        public ActionResult frontcal() {
            //知道傳進來的housekey 和 bedinspaceid
            int hkey = Convert.ToInt32(Request.QueryString["hkey"]);
            int bedspace = Convert.ToInt32(Request.QueryString["bspace"]);
            List<bookings> Vis = new List<bookings>();
            CalendarRepository cals = new CalendarRepository();
           
            List<Calendar> calss = cals.FindCByHkeyBedspace(hkey, bedspace);
            
            if (calss.Count != 0) { 
            foreach (var i in calss) {
                bookings one = new bookings();

                one.title = "開放日期";
                one.start = String.Format("{0:yyyy-MM-dd}", i.FromDate);
                one.end = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(i.EndDate).AddDays(1));
                Vis.Add(one);

            }}

            

            if (Vis.Count != 0)
            {

                return Json(Vis, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(null);
            }

        }


        



        public ActionResult stars()
        {  

            string paths = Server.MapPath("~/img/stars.png");
            byte[] pics = System.IO.File.ReadAllBytes(paths);
            return File(pics, "image/png");
        }



        [HttpPost]
        public ActionResult addcomments(int id,housedetailsview one) {
                    
                
                one.addacomment.MemberID = Convert.ToInt32(Request.Cookies["MemberID"].Value);
                one.addacomment.CreateDate = DateTime.Now;
                one.addacomment.HouseKey = id;
              
                quality.Create(one.addacomment);
                return RedirectToAction("houseDetails", "JustForUser", new { area = "Accomodation", id = id });
            
           
        }


        //查看訂房紀錄
        public ActionResult CheckReservation() {
            if (Request.Cookies["MemberID"] != null)
            {
                int Memid = Convert.ToInt32(Request.Cookies["MemberID"].Value);
                CheckReView viewmodel = new CheckReView(Memid);
                Response.Cookies["CheckR"].Value = String.Format("{0:yyyy-MM-dd H:mm:ss}", DateTime.Now);
                Response.Cookies["CheckR"].Expires = DateTime.Now.AddDays(7);
                return View(viewmodel);
            }
            else {
                return RedirectToAction("Index", "JustForUser", new { area = "Accomodation" });
            }
        }

        //查看檢舉進度
        public ActionResult aboutbadcomment()
        {
            if (Request.Cookies["MemberID"] != null)
            {
                int memberid = Convert.ToInt32(Request.Cookies["MemberID"].Value);
                HouseQualityRepository hrpo = new HouseQualityRepository();
                List<HouseQuality> hslist = hrpo.GetAll().Where(n => n.MemberID == memberid&&n.Score==-1).ToList();

                return View(hslist);
            }
            else {
                return RedirectToAction("Index", "JustForUser", new { area = "Accomodation" });
            }
        }


    }
}