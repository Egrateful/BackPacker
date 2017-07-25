using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSIT11404project1.Areas.Accomodation.Models;
using MSIT11404project1.Areas.ViewModel;
using MSIT11404project1.Models;
using PagedList.Mvc;
using PagedList;
using MSIT11404project1.Areas.Admin.ViewModel;
using MSIT11404project1.Areas.Accomodation.ViewModel;

namespace MSIT11404project1.Areas.Admin.Controllers
{
    public class AboutHouseController : Controller
    {

        IRepository<HouseMain> housemains = new Repository<HouseMain>();
        //IRepository<BedDtails> beds = new Repository<BedDtails>();
        BedDetailsRepository beds = new BedDetailsRepository();
        IRepository<ImageForRoom> imageforroom = new Repository<ImageForRoom>();
        // GET: Admin/AboutHouse
        public ActionResult Index(int page=1)
        {

            int currentpage = page < 1 ? 1 : page;
            var allrooms = housemains.GetAll().OrderBy(n => n.MemberID);

            var pagelist = allrooms.ToPagedList(currentpage, 10);
            
            return View(pagelist);
        }

        [HttpGet]
        public ActionResult Index2(int page = 1) {

            HouseSearchView view = new HouseSearchView();
            HouseMainRepository housrepo = new HouseMainRepository();
            int currentpage = page < 1 ? 1 : page;

            view.houseEntities=housrepo.GetAll().OrderBy(n=>n.MemberID).ToPagedList(currentpage,5);
            return View(view);

        }


        [HttpPost]
        public ActionResult Index2(HouseSearchView one) {
            HouseMainRepository hrepo = new HouseMainRepository();
            var query = hrepo.GetAll().AsQueryable();
            if (!string.IsNullOrWhiteSpace(Convert.ToString(one.housesearchm.MemberID))&&one.housesearchm.MemberID!=0) {
                query = query.Where(n => n.MemberID == one.housesearchm.MemberID);
            }
            if (!string.IsNullOrWhiteSpace(one.housesearchm.Country)) {
                query = query.Where(n => n.Country.Contains(one.housesearchm.Country));

            }
            if (!string.IsNullOrWhiteSpace(one.housesearchm.States)) {
                query = query.Where(n => n.State.Contains(one.housesearchm.States));

            }
            if (one.housesearchm.housetype!=0) {
                query = query.Where(n => n.HouseSourceID == one.housesearchm.housetype);
            }
            if (one.housesearchm.housesoutce != null)
            {
                if (one.housesearchm.housesoutce.Count != 0)
                {
                    List<IQueryable<HouseMain>> houseslistss = new List<IQueryable<HouseMain>>();

                    for (int i = 0; i < one.housesearchm.housesoutce.Count; i++)
                    {
                        int sourcetype=Convert.ToInt32(one.housesearchm.housesoutce[i]);
                        var aaa = query.Where(n => n.HouseSourceID == sourcetype);

                        houseslistss.Add(aaa);
                    }
                    query = houseslistss[0];
                    if (houseslistss.Count > 1) {
                        for (int i = 1; i < houseslistss.Count; i++) {
                            query = query.Concat(houseslistss[i]);
                        }
                    }
                    
                        }
            }
            if ((one.housesearchm.FromDate == null && one.housesearchm.EndDate != null)||(one.housesearchm.FromDate!=null&&one.housesearchm.EndDate==null)) {
                if (one.housesearchm.FromDate == null && one.housesearchm.EndDate != null)
                {
                    query = query.Where(n => n.CreateDate < one.housesearchm.EndDate);
                }
                else if (one.housesearchm.FromDate != null && one.housesearchm.EndDate != null)
                {
                    query = query.Where(n => n.CreateDate > one.housesearchm.FromDate && n.CreateDate < one.housesearchm.EndDate);

                }
                else {
                    query = query.Where(n => n.CreateDate > one.housesearchm.FromDate);
                }

            }
            if (one.housesearchm.IsActive!=0) {
                if (one.housesearchm.IsActive == 1)
                {
                    query = query.Where(n => n.IsActive == false);
                }
                else {
                    query = query.Where(n => n.IsActive == true);
                }
            }

            if (one.housesearchm.Isfreeze!=0) {
                if (one.housesearchm.Isfreeze == 1)
                {
                    query = query.Where(n => n.IsFreeze == false);
                }
                else {
                    query = query.Where(n => n.IsFreeze == true);
                }
            }
            

            HouseSearchView resaults = new HouseSearchView();
            resaults.PageIndex = one.PageIndex<1?1:one.PageIndex;
            resaults.houseEntities = query.OrderBy(n=>n.MemberID).ToPagedList(resaults.PageIndex, 5);
            return View(resaults);
        }



        public ActionResult Delete(int id)
        {

            var listsbeds = beds.GetAll().Where(n => n.HouseKey == id).ToList();
            foreach (var bedss in listsbeds) {
                beds.Delete(bedss);
            }

            housemains.Delete(housemains.GetByID(id));
            
            return RedirectToAction("Index", "AboutHouse", "Admin");
        }//ID 是housekey




        

      

        [HttpGet]
        public ActionResult Edit2(int id) {

            InsertHouse viewmodel = new InsertHouse(id);
            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult Edit2(InsertHouse viewmodel, IEnumerable< HttpPostedFileBase> Images,int id) {
            HouseMainRepository hrepo = new HouseMainRepository();

            int pages = Convert.ToInt32(Request.QueryString["page"])<1?1:Convert.ToInt32(Request.QueryString["page"]);

            viewmodel.EntityHouseMain.HouseKey = id;
            HouseMain hhs =hrepo.GetByID(id);

            Convert.ToDateTime(viewmodel.EntityHouseMain.CreateDate);
            if (viewmodel.EntityHouseMain.CreateDate == null) {
                viewmodel.EntityHouseMain.CreateDate = hhs.CreateDate;
            }
            if (viewmodel.EntityHouseMain.Price == null) {
                viewmodel.EntityHouseMain.Price = hhs.Price;
            }

            hhs = viewmodel.EntityHouseMain;
            housemains.Update(hhs);
            
            if (Images.First()!=null) {
                foreach (var pic in Images) {
                    ImageForRoom imgs = new ImageForRoom();
                    imgs.HouseKey = viewmodel.EntityHouseMain.HouseKey;
                    Byte[] imgbytes = new byte[pic.ContentLength];
                    pic.InputStream.Read(imgbytes, 0, pic.ContentLength);
                    imgs.ImageByte = imgbytes;
                    imageforroom.Create(imgs);

                }
            }


            return RedirectToAction("Index2", "AboutHouse", new { area = "Admin" ,page=pages });
        }


        public ActionResult Insert()
        {
            return View(); 
        }


        [HttpPost]
        public ActionResult Insert(InsertHouse viewmodel,IEnumerable<HttpPostedFileBase> Images)
        {
            viewmodel.EntityHouseMain.CreateDate = DateTime.Now;

            housemains.Create(viewmodel.EntityHouseMain);
            if (Images.First()!=null) {
                
                
                foreach (HttpPostedFileBase item in Images) {
                    ImageForRoom room = new ImageForRoom();
                    room.HouseKey = viewmodel.EntityHouseMain.HouseKey;
                    byte[] bs = new byte[item.ContentLength];
                    item.InputStream.Read(bs, 0, item.ContentLength);
                    room.ImageByte = bs;
                    imageforroom.Create(room);
                }
                
                
            }
           
           
           
           return RedirectToAction("Index2", "AboutHouse", "Admin");
        }


        public ActionResult Critics(int id) {
            string lastpage = Request.UrlReferrer.ToString();
            HouseQualityRepository hrepo = new HouseQualityRepository();
            var houseq= hrepo.GetAll().Where(n => n.HouseKey == id&&n.Score==-1).ToList();

            Response.Cookies["lastpage"].Value = lastpage;
            return View(houseq);
        }

        public ActionResult backtosame() {


            return Redirect(Convert.ToString(Request.Cookies["lastpage"].Value));
        }


        public ActionResult Indexofbed() {
            AdminBedView view = new AdminBedView();
            return View(view);

        }




        [HttpPost]
        public ActionResult postBed(AdminBedView one) {
            IRepository<BedType> bed = new Repository<BedType>();

            bed.Create(one.bedentity);

            return RedirectToAction("Indexofbed", "AboutHouse", new { area = "Admin" });
        }
        public ActionResult postHS(AdminBedView one) {

            IRepository<HouseSpace> hs = new Repository<HouseSpace>();

            hs.Create(one.houseSpaceentity);

            return RedirectToAction("Indexofbed", "AboutHouse", new { area = "Admin" });
        }

        public ActionResult postHouseSource(AdminBedView one) {

            IRepository<HouseSourceType> hs = new Repository<HouseSourceType>();

            hs.Create(one.houseSourcesE);

            return RedirectToAction("Indexofbed", "AboutHouse", new { area = "Admin" });
        }

    }
}