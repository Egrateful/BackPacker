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
    public class RestaurantController : Controller
    {
        IRepository<Restaurants> rest = new Repository<Restaurants>();
        // GET: Admin/Restaurant
        [HttpGet]
        public ActionResult Index(int page = 1, string sortby = "", string allsearch = "", string namesearch = "nothing", string addrsearch = "nothing", string citysearch = "nothing")
        {
            List<Restaurants> backlist = new List<Restaurants>();
            IQueryable<Restaurants> list;

            if (allsearch == "" && namesearch == "nothing" && addrsearch == "nothing" && citysearch == "nothing")
            {
                list = rest.GetAll();
            }

            else if (allsearch != "")
            {
                list = rest.GetAll().Where(r => r.Name.Contains(allsearch) || r.Address.Contains(allsearch) || r.Cities.CityName.Contains(allsearch));
            }
            else
            {
                list = rest.GetAll().Where(r => r.Name.Contains(namesearch))
                    .Union(rest.GetAll().Where(r => r.Address.Contains(addrsearch)))
                    .Union(rest.GetAll().Where(r => r.Cities.CityName.Contains(citysearch)));
            }

            ViewBag.SortByID = string.IsNullOrEmpty(sortby) ? "PlaceID desc" : "";
            ViewBag.SortByName = (sortby == "Name") ? "Name desc" : "Name";
            ViewBag.SortByAddress = (sortby == "Address") ? "Address desc" : "Address";
            ViewBag.SortByCity = (sortby == "City") ? "City desc" : "City";

            switch (sortby)
            {
                case "PlaceID desc":
                    backlist = list.OrderByDescending(r => r.RestaurantID).ToList();
                    break;
                case "Name desc":
                    backlist = list.OrderByDescending(r => r.Name).ToList();
                    break;
                case "Address desc":
                    backlist = list.OrderByDescending(r => r.Address).ToList();
                    break;
                case "City desc":
                    backlist = list.OrderByDescending(r => r.Cities.CityName).ToList();
                    break;
                case "Name":
                    backlist = list.OrderBy(r => r.Name).ToList();
                    break;
                case "Address":
                    backlist = list.OrderBy(r => r.Address).ToList();
                    break;
                case "City":
                    backlist = list.OrderBy(r => r.Cities.CityName).ToList();
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
            return View(rest.GetByID(id));
        }

        [HttpPost]
        public ActionResult Edit(Restaurants r, HttpPostedFileBase byteimg, int page = 1, string sortby = "", string allsearch = "", string namesearch = "nothing", string addrsearch = "nothing", string citysearch = "nothing")
        {
            if (byteimg != null)
            {
                r.Image = new byte[byteimg.ContentLength];
                byteimg.InputStream.Read(r.Image, 0, byteimg.ContentLength);
            }
            //r.Cities.CityID = r.CityID;
            rest.Update(r);
            return RedirectToAction("Index", "Restaurant", new { Area = "Admin", page = page, sortby = sortby, allsearch = allsearch, namesearch = namesearch, addrsearch = addrsearch, citysearch = citysearch });
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Restaurants r, HttpPostedFileBase byteimg)
        {

            if (byteimg != null)
            {
                r.Image = new byte[byteimg.ContentLength];
                byteimg.InputStream.Read(r.Image, 0, byteimg.ContentLength);
            }
            rest.Create(r);
            return RedirectToAction("Index", "Restaurant", new { Area = "Admin" });
        }
    }
}