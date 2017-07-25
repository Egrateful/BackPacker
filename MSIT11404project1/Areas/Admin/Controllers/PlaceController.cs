using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using MSIT11404project1.Models;
namespace MSIT11404project1.Areas.Admin.Controllers
{
    public class PlaceController : Controller
    {
        MSIT11404Entities db = new MSIT11404Entities();
        IRepository<Places> place = new Repository<Places>();

        // GET: Admin/Place
        [HttpGet]
        public ActionResult Index(int page = 1, int perpage = 15, string sortby = "",string namesearch="",string citysearch="",string addrsearch ="",string allsearch="")
        {
            var backList =new List<Places>();
            var list = db.Places.ToList();
            string allstr = string.IsNullOrEmpty(allsearch) ? "" : allsearch;
            string citystr = string.IsNullOrEmpty(citysearch) ? "nothing" : citysearch;
            string addrstr = string.IsNullOrEmpty(addrsearch) ? "nothing" : addrsearch;
            string namestr = string.IsNullOrEmpty(namesearch) ? "nothing" : namesearch;

            if (allstr == "" && citystr == "nothing" && addrstr == "nothing" && namestr == "nothing")
            {
                backList = list;
            }
            else if (allstr != "")
            {
                backList = list.Select(p => p).Where(p => p.Address.Contains(allstr) || p.Cities.CityName.Contains(allstr) || p.Name.Contains(allsearch)).ToList();
            }
            else
            {
                backList = list.Select(p => p).Where(p => p.Address.Contains(addrstr)).ToList()
                    .Union(list.Select(p => p).Where(p => p.Cities.CityName.Contains(citystr))).ToList()
                    .Union(list.Select(p => p).Where(p => p.Name.Contains(namestr))).ToList();
            }


            ViewBag.SearchByName = namestr;
            ViewBag.SearchByAddr = addrstr;
            ViewBag.SearchByCity = citystr;
            ViewBag.SearchByAll = allstr;


            ViewBag.SortByID = string.IsNullOrEmpty(sortby) ? "PlaceID desc" : "";
            ViewBag.SortByName = (sortby == "Name") ? "Name desc" : "Name";
            ViewBag.SortByAddress = (sortby == "Address") ? "Address desc" : "Address";
            ViewBag.SortByCity = (sortby == "City") ? "City desc" : "City";
           

            switch (sortby)
            {
                case "PlaceID desc":
                    backList = backList.OrderByDescending(p => p.PlaceID).ToList();
                    break;
                case "Name desc":
                    backList = backList.OrderByDescending(p => p.Name).ToList();
                    break;
                case "Name":
                    backList = backList.OrderBy(p => p.Name).ToList();
                    break;
                case "Address desc":
                    backList = backList.OrderByDescending(p => p.Address).ToList();
                    break;
                case "Address":
                    backList = backList.OrderBy(p => p.Address).ToList();
                    break;
                case "City desc":
                    backList = backList.OrderByDescending(p => p.Cities.CityName).ToList();
                    break;
                case "City":
                    backList = backList.OrderBy(p => p.Cities.CityName).ToList();
                    break;
                default:
                    backList = backList.OrderBy(p => p.PlaceID).ToList();
                    break;
            }


            return View(backList.ToList().ToPagedList(page, perpage));
        }


        //[HttpPost]
        //public ActionResult Index(FormCollection form)
        //{
        //    var list = db.Places.ToList();
        //    string allstr = form["allsearch"].ToString();

        //    string namestr = form["namesearch"].ToString() == "" ? "nothing" : form["namesearch"].ToString();
        //    string addrstr = form["addrsearch"].ToString() == "" ? "nothing" : form["addrsearch"].ToString();
        //    string citystr = form["citysearch"].ToString() == "" ? "nothing" : form["citysearch"].ToString();
        //    List<Places> allList = new List<Places>();
        //    if (allstr == "" && namestr == "nothing" && addrstr == "nothing" && citystr == "nothing")
        //        allList = db.Places.ToList();
        //    else if (allstr != "")
        //        allList = (list.Select(p => p).Where(p => p.Address.Contains(allstr) || p.City.Contains(allstr) || p.Name.Contains(allstr))).ToList();
        //    else
        //    {
        //        allList = list.Select(p => p).Where(p => p.Address.Contains(addrstr)).ToList()
        //            .Union(list.Select(p => p).Where(p => p.City.Contains(citystr))).ToList()
        //            .Union(list.Select(p => p).Where(p => p.Name.Contains(namestr))).ToList();
        //    }
        //    return View("Index",allList.ToPagedList(1, 20));
        //}



        public ActionResult Delete(int id = 0, int page = 1,string sortby="", string namesearch = "", string citysearch = "", string addrsearch = "", string allsearch = "")
        {
            place.Delete(place.GetByID(id));
            return RedirectToAction("Index", "Place", new { Area = "Admin",sortby=sortby ,page = page,allsearch=allsearch,namesearch=namesearch,citysearch=citysearch,addrsearch=addrsearch });
        }

        [HttpGet]
        public ActionResult Edit(int id, int page = 1,string sortby="", string namesearch = "", string citysearch = "", string addrsearch = "", string allsearch = "")
        {
            ViewBag.EditSavedPage = page;
            ViewBag.EditSaveAllStr = allsearch;
            ViewBag.EditSaveNameStr = namesearch;
            ViewBag.EditSaveAddrStr = addrsearch;
            ViewBag.EditSaveCityStr = citysearch;
            ViewBag.EditSaveSortBy = sortby;
            return View(place.GetByID(id));
        }

        [HttpPost]
        public ActionResult Edit(Places p, HttpPostedFileBase byteimg)
        {

            if (byteimg != null)
            {
                p.Image = new byte[byteimg.ContentLength];
                byteimg.InputStream.Read(p.Image, 0, byteimg.ContentLength);
            }
            place.Update(p);
            return RedirectToAction("Index", "Place", new { Area = "Admin",sortby=Request.Form["sortby"] , page = Request.Form["thispage"], allsearch = Request.Form["allsearch"], namesearch = Request.Form["namesearch"], addrsearch = Request.Form["addrsearch"], citysearch = Request.Form["citysearch"] });
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Places p, HttpPostedFileBase byteimg)
        {
            if (byteimg != null)
            {
                p.Image = new byte[byteimg.ContentLength];
                byteimg.InputStream.Read(p.Image, 0, byteimg.ContentLength);
            }
            place.Create(p);
            return RedirectToAction("Index", "Place", new { Area = "Admin" });
        }


        public ActionResult GetCities() {
            List<string> CityNameList = new List<string>();
            CityNameList = db.Cities.ToList().Select(c => c.CityName).ToList();
            return Json(CityNameList,JsonRequestBehavior.AllowGet);
        }
    }
}