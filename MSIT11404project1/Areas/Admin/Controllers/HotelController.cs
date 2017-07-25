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
    public class HotelController : Controller
    {
        IRepository<Hotels> hotel = new Repository<Hotels>();
        // GET: Admin/Hotel
        [HttpGet]
        public ActionResult Index(int page = 1, string sortby = "", string allsearch = "", string namesearch = "nothing", string addrsearch = "nothing", string citysearch = "nothing")
        {
            List<Hotels> backlist = new List<Hotels>();
            IQueryable<Hotels> list;

            if (allsearch == "" && namesearch == "nothing" && addrsearch == "nothing" && citysearch == "nothing")
            {
                list = hotel.GetAll();
            }

            else if (allsearch != "")
            {
                list = hotel.GetAll().Where(h => h.Name.Contains(allsearch) || h.Address.Contains(allsearch) || h.Cities.CityName.Contains(allsearch));
            }
            else
            {
                list = hotel.GetAll().Where(h => h.Name.Contains(namesearch))
                    .Union(hotel.GetAll().Where(h => h.Address.Contains(addrsearch)))
                    .Union(hotel.GetAll().Where(h => h.Cities.CityName.Contains(citysearch)));
            }

            ViewBag.SortByID = string.IsNullOrEmpty(sortby) ? "PlaceID desc" : "";
            ViewBag.SortByName = (sortby == "Name") ? "Name desc" : "Name";
            ViewBag.SortByAddress = (sortby == "Address") ? "Address desc" : "Address";
            ViewBag.SortByCity = (sortby == "City") ? "City desc" : "City";

            switch (sortby)
            {
                case "PlaceID desc":
                    backlist = list.OrderByDescending(h=>h.HotelID).ToList();
                    break;
                case "Name desc":
                    backlist = list.OrderByDescending(h => h.Name).ToList();
                    break;
                case "Address desc":
                    backlist = list.OrderByDescending(h => h.Address).ToList();
                    break;
                case "City desc":
                    backlist = list.OrderByDescending(h => h.Cities.CityName).ToList();
                    break;
                case "Name":
                    backlist = list.OrderBy(h => h.Name).ToList();
                    break;
                case "Address":
                    backlist = list.OrderBy(h => h.Address).ToList();
                    break;
                case "City":
                    backlist = list.OrderBy(h => h.Cities.CityName).ToList();
                    break;
                default:
                    backlist = list.ToList();
                    break;
            }

            return View(backlist.ToPagedList(page, 15));
        }


        [HttpGet]
        public ActionResult Edit(int id, int page = 1, string sortby = "", string allsearch = "", string namesearch = "nothing", string addrsearch = "nothing", string citysearch = "nothing")
        {
            return View(hotel.GetByID(id));
        }

        [HttpPost]
        public ActionResult Edit(Hotels h, HttpPostedFileBase byteimg, int page = 1, string sortby = "", string allsearch = "", string namesearch = "nothing", string addrsearch = "nothing", string citysearch = "nothing")
        {
            if (byteimg != null)
            {
                h.Image = new byte[byteimg.ContentLength];
                byteimg.InputStream.Read(h.Image, 0, byteimg.ContentLength);
            }
            hotel.Update(h);
            return RedirectToAction("Index", "Hotel", new { Area = "Admin", page = page, sortby = sortby, allsearch = allsearch, namesearch = namesearch, addrsearch = addrsearch, citysearch = citysearch });
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Hotels h, HttpPostedFileBase byteimg)
        {
            if (byteimg != null)
            {
                h.Image = new byte[byteimg.ContentLength];
                byteimg.InputStream.Read(h.Image, 0, byteimg.ContentLength);
            }
            hotel.Create(h);
            return RedirectToAction("Index", "Hotel", new { Area = "Admin" });
        }

    }
}