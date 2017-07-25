using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSIT11404project1.Areas.Accomodation.Models;
using MSIT11404project1.Models;
using MSIT11404project1.Areas.Accomodation.ViewModel;
using System.Web.UI;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace MSIT11404project1.Areas.Accomodation.Controllers
{


    public class LandLordController : Controller
    {

        // GET: Accomodation/LandLord
        public ActionResult ManageCalendar()
        {
            if (Request.Cookies["MemberID"] != null) {
                int userid = Convert.ToInt32(Request.Cookies["MemberID"].Value);

                MSIT11404project1.Areas.Accomodation.ViewModel.ManageCalendar view = new ViewModel.ManageCalendar(userid);
                Response.Cookies["LandCheck"].Value = String.Format("{0:yyyy-MM-dd H:mm:ss}", DateTime.Now);
                Response.Cookies["LandCheck"].Expires = DateTime.Now.AddDays(7);
                return View(view);
            }

            return RedirectToAction("Index", "JustForUser", new { area = "Accomodation" });



        }
        public ActionResult Index() {


            return View();

        }




        public ActionResult BasicInfoFirst(int? id) {
            if (id == null) {
                int hkey=Convert.ToInt32(Request.Cookies["hkey"].Value);
                basicviewmodel viewmodel = new basicviewmodel(hkey);
                return View(viewmodel);
            }//從別的步驟點過來呼叫
            //int memid = Convert.ToInt32(Request.Cookies["MemberID"].Value);測試先寫死
            else if (id == 0)
            {
                basicviewmodel viewholenew = new basicviewmodel();
                
                Response.Cookies["hkey"].Value = "0";
                return View(viewholenew);
            }

            else
            {
                if (Request.QueryString["notactive"] != null)
                {
                    Response.Cookies["IsActive"].Value = "yes";
                }
                basicviewmodel viewmodel = new basicviewmodel(Convert.ToInt32(id));
                Response.Cookies["hkey"].Value = id.ToString(); 
                return View(viewmodel);
            }



            
        }


        public ActionResult GetFirst(basicviewmodel entity)
        {
            
            HouseMainRepository repo = new HouseMainRepository();
            int memid = Convert.ToInt32(Request.Cookies["MemberID"].Value);
            //在主畫面的時候就可以判斷 繼續編輯 或 建立一個新房源
            if (Request.Cookies["hkey"].Value == "0") {
                    
                    entity.housesentity.CreateDate = DateTime.Now;
                    entity.housesentity.IsActive = false;
                    entity.housesentity.MemberID = memid;//記得換掉

                    repo.Create(entity.housesentity);

                    int housekey = entity.housesentity.HouseKey;

                    Response.Cookies["hkey"].Value = housekey.ToString();
                    

                    return RedirectToAction("BasicInfoTwo", "LandLord", new { area = "Accomodation" });
               

            }
            else
            {
                    
                    HouseMain one = repo.GetByID(Convert.ToInt32(Request.Cookies["hkey"].Value));
                    one.HouseSourceID = entity.housesentity.HouseSourceID;
                    one.HouseSpaceTypeID = entity.housesentity.HouseSpaceTypeID;
                    one.Country = entity.housesentity.Country;
                    one.State = entity.housesentity.State;
                    one.Adress = entity.housesentity.Adress;
                    
                    repo.Update((repo.GetByID(one.HouseKey)));

                    return Redirect(Request.UrlReferrer.ToString());
                
            }



           



        }

         
        public ActionResult BasicInfoTwo()
        {
            int hkey = Convert.ToInt32(Request.Cookies["hkey"].Value);
            basicviewmodel viewmodel = new basicviewmodel(hkey);
            return View(viewmodel);
        }

        public ActionResult GetSecond(basicviewmodel entity) {
            
            HouseMainRepository repo = new HouseMainRepository();

            HouseMain one = repo.GetByID(Convert.ToInt32( Request.Cookies["hkey"].Value));
            one.HouseName = entity.housesentity.HouseName;

            one.IsOwnerHome = entity.housesentity.IsOwnerHome;

            one.BathroomNum = entity.housesentity.BathroomNum;

            one.PeopleAllowed = entity.housesentity.PeopleAllowed;

            one.BedroomNum = entity.housesentity.BedroomNum;

            repo.Update(repo.GetByID(one.HouseKey));

            if (Request.Cookies["StepCount2"]!=null)
            {
                
                return Redirect(Request.UrlReferrer.ToString());
            }
            else {
                Response.Cookies["StepCount2"].Value = "1";
                return RedirectToAction("BasicInfoThree","LandLord",new {area="Accomodation"});
            }

        }

        public ActionResult BasicInfoThree() {
            int hkey =Convert.ToInt32( Request.Cookies["hkey"].Value);
            //int memid = Convert.ToInt32(Request.Cookies["MemberID"].Value);測試先寫死
            basicviewmodel view = new basicviewmodel(hkey);
            return View(view);
        }


        public ActionResult GetThird(basicviewmodel entity) {
            BedDetailsRepository brpo = new BedDetailsRepository();
            HouseMainRepository hpro = new HouseMainRepository();
            HouseMain one = hpro.GetByID(Convert.ToInt32(Request.Cookies["hkey"].Value));
            //填入memberid找到編輯到一半 的 房源編號
            
            List<int> prices = new List<int>();
            foreach (var items in entity.bedss) {
                BedDtails oo = new BedDtails();
                oo = brpo.GetByID(items.BedInSpaceID);
                oo.BedCount = items.BedCount;
                oo.BedTypeID = items.BedTypeID;
                oo.HouseKey = items.HouseKey;
                oo.Ispublic = items.Ispublic;
                oo.SpaceIntro = items.SpaceIntro;
                int a;
                if (items.RoomPrice != null)
                {
                    a= Convert.ToInt32(items.RoomPrice);
                    
                    oo.RoomPrice = items.RoomPrice;
                }
                else {
                    a = Convert.ToInt32(oo.RoomPrice);
                }
                prices.Add(a);
                
                brpo.Update(oo);
            }
            one.Price=Convert.ToDecimal(prices.Average());
            hpro.Update(one);
            if (Request.Cookies["StepCount3"]==null)
            {
                Response.Cookies["StepCount3"].Value = "1";
                return RedirectToAction("BasicInfoFour", "LandLord", new { area = "Accomodation" });

            }
            else {
                return Redirect(Request.UrlReferrer.ToString());//如果有按過下一步
            }


        }

        public ActionResult BasicInfoFour() {
            //int memid = Convert.ToInt32(Request.Cookies["MemberID"].Value);測試先寫死
            int hkey = Convert.ToInt32(Request.Cookies["hkey"].Value);
            basicviewmodel view = new basicviewmodel(hkey);
            return View(view);

        }
        

        //新增照片的時候
        public ActionResult GetForth(basicviewmodel postpic) {
            IRepository<ImageForRoom> imgs = new Repository<ImageForRoom>();
            IEnumerable<HttpPostedFileBase> listpics = postpic.pics;
            List<ImageForRoom> lispic = new List<ImageForRoom>();
            int hkey = Convert.ToInt32(Request.Cookies["hkey"].Value);
            foreach (var items in listpics) {
                
                ImageForRoom pics = new ImageForRoom();
                int con = items.ContentLength;
                byte[] placement = new byte[con];
                items.InputStream.Read(placement, 0, con);
                pics.ImageByte = placement;
                pics.HouseKey = hkey;
                imgs.Create(pics);
            }

            

            return RedirectToAction("BasicInfoFour","LandLord",new { area="Accomodation"});
        }




        //成為房東點下去=>一開始的首頁
        //在step 4會呼叫一次返回首頁
        public ActionResult theveryfirst(int? id) {
            if (id ==0 ) {
                Response.Cookies["StepCount2"].Expires = DateTime.Now.AddSeconds(-1);
                Response.Cookies["StepCount3"].Expires = DateTime.Now.AddSeconds(-1);
                Response.Cookies["hkey"].Expires = DateTime.Now.AddSeconds(-1);
                Response.Cookies["IsActive"].Expires = DateTime.Now.AddSeconds(-1);
            }

            int memid = Convert.ToInt32(Request.Cookies["MemberID"].Value);
            theFirst view = new theFirst(memid);

            return View(view);
        }



       
       
        //沒有發布過的房間(包括編輯到一半,全新建立)
        public ActionResult laststep() {
            HouseMainRepository hrepo = new HouseMainRepository();

            int keys = Convert.ToInt32(Request.Cookies["hkey"].Value);
            HouseMain hhs =hrepo.GetByID(keys);
            hhs.IsActive = true;

            Response.Cookies["StepCount2"].Expires = DateTime.Now.AddSeconds(-1);
            Response.Cookies["StepCount3"].Expires = DateTime.Now.AddSeconds(-1);
            Response.Cookies["hkey"].Expires = DateTime.Now.AddSeconds(-1);

            hrepo.Update(hhs);
            return RedirectToAction("theveryfirst","LandLord",new {area="Accomodation"});
        }


        //如果是有發布過的房間
        public ActionResult ChangeState() {
            HouseMainRepository hrepo = new HouseMainRepository();
            int Keys = Convert.ToInt32(Request.Cookies["hkey"].Value);
            HouseMain hs = hrepo.GetByID(Keys);
            hs.IsActive = false;
            hrepo.Update(hs);


            Response.Cookies["StepCount2"].Expires = DateTime.Now.AddSeconds(-1);
            Response.Cookies["StepCount3"].Expires = DateTime.Now.AddSeconds(-1);
            Response.Cookies["hkey"].Expires = DateTime.Now.AddSeconds(-1);
            Response.Cookies["IsActive"].Expires = DateTime.Now.AddSeconds(-1);

            return RedirectToAction("theveryfirst", "LandLord", new { area = "Accomodation" });
        }








        public void inputbeds() {
            HouseMainRepository hous = new HouseMainRepository();
            var hs = hous.GetAll().AsEnumerable().ToList();
            BedDetailsRepository beds = new BedDetailsRepository();
            var bedss = beds.GetAll().AsEnumerable().ToList();
            foreach (var items in hs) {
                if (bedss.Where(n => n.HouseKey == items.HouseKey).ToList().Count != 0) { 
                int counts =bedss.Where(n => n.HouseKey == items.HouseKey).ToList().Count;
                items.BedAvailableNum = counts;
                    hous.Update(items);
                }
            }

        }


        //public ActionResult inputprice() {
        //    HouseMainRepository hhs = new HouseMainRepository();
        //    var prices = hhs.GetAll().AsEnumerable().ToList();
        //    BedDetailsRepository brpo = new BedDetailsRepository();
        //    var bbs = brpo.GetAll().AsEnumerable().GroupBy(n=>n.HouseKey).Select(n=>new {n.Key,n }).ToList();
        //    foreach (var iis in bbs) {
        //        var listspp = iis.n.Select(m => m.RoomPrice);
        //        HouseMain hsmain = prices.Where(b => b.HouseKey == iis.Key).First();
        //        hsmain.Price = listspp.Average();

        //        hhs.Update(hsmain);
        //    }


        //    return View();
        //}








        //public ActionResult inputhouses() {
        //HouseMainRepository hs = new HouseMainRepository();
        //    int countt = 0;
        //    IRepository<Hotels> hotelss = new Repository<Hotels>();
        //    BedDetailsRepository bpo = new BedDetailsRepository();
        //    var hoss = hotelss.GetAll().AsEnumerable().Where(n => n.Address.Contains("澎湖")||n.Address.Contains("金門")).ToList();
        //    Random rr = new Random();

        //    for (int i = 0; i <= 8; i++) {
        //        HouseMain hss = new HouseMain();
        //        hss.Adress = Convert.ToString(hoss[i].Address);
        //        hss.BedAvailableNum = rr.Next(1, 3);
        //        int a = Convert.ToInt32( hss.BedAvailableNum);

        //        hss.PeopleAllowed = rr.Next(1, 6);
        //        hss.MemberID = 89+countt;
        //        hss.IsOwnerHome = Convert.ToBoolean( rr.Next(0, 2));
        //        hss.BathroomNum = rr.Next(1, 3);
        //        hss.IsActive = Convert.ToBoolean(rr.Next(0, 2));
        //        hss.HouseSpaceTypeID = rr.Next(1, 4);
        //        hss.HouseSourceID = rr.Next(1, 7);
        //        hss.CreateDate = DateTime.Now;
        //        if (hoss[i].Address.Contains("澎湖"))
        //        {
        //            hss.Country = "台灣";
        //            hss.State = "澎湖";
        //        }
        //        else {
        //            hss.Country = "台灣";
        //            hss.State = "金門";

        //        }

        //        hss.BedroomNum = hss.BedAvailableNum;
        //        hs.Create(hss);



        //        for (int ss = 1; ss <= a; ss++)
        //        {
        //            BedDtails one = new BedDtails();
        //            one.HouseKey = hss.HouseKey;
        //            one.Ispublic = Convert.ToBoolean(rr.Next(0, 2));
        //            one.RoomPrice = rr.Next(800, 1600);
        //            one.BedTypeID = rr.Next(1, 6);
        //            one.BedCount = rr.Next(1, 3);
        //            one.SpaceIntro = ss.ToString();
        //            bpo.Create(one);

        //        }

        //        List<byte[]> piccc = new List<byte[]>();
        //        for (int tt = 1; tt <= 20; tt++)
        //        {
        //            string path = Server.MapPath("~/imgg/img" + tt+ ".jpg");
        //            Byte[] bys = System.IO.File.ReadAllBytes(path);
        //            piccc.Add(bys);

        //        }

        //        IRepository<ImageForRoom> ipo = new Repository<ImageForRoom>();

        //        for (int ses = 1; ses <= 3; ses++) {
        //            ImageForRoom teo = new ImageForRoom();
        //            teo.ImageByte = piccc[rr.Next(0, 18)];
        //            teo.HouseKey = hss.HouseKey;
        //            ipo.Create(teo);
        //        }
        //        countt++;


        //    }

        //    return View();
        //}









        //public void inputhousename()
        //{
        //    HouseMainRepository hp = new HouseMainRepository();
        //    var lis = hp.GetAll().AsEnumerable().ToList();
        //    foreach (var hs in lis)
        //    {
        //        var hh = hp.GetByID(hs.HouseKey);
        //        hh.HouseName = hh.Members.UserName + "'s Home";
        //        hp.Update(hh);

        //    }
        //}



        //public ActionResult inputcomm()
        //{
        //    HouseMainRepository hrepo = new HouseMainRepository();
        //    HouseQualityRepository hsq = new HouseQualityRepository();
        //    var hhs = hrepo.GetAll().AsEnumerable().ToList();
        //    foreach (var items in hhs)
        //    {
        //        int all = hsq.GetAll().AsEnumerable().Where(n => n.HouseKey == items.HouseKey && n.Score == -1).ToList().Count;
        //        items.BadComment = all;
        //        hrepo.Update(items);

        //    }

        //    return View();
        //}











        //public ActionResult inputfile2()
        //{
        //    HouseMainRepository db = new HouseMainRepository();
        //    BedDetailsRepository bb = new BedDetailsRepository();
        //    Random one = new Random();
        //    var houses = db.GetAll().ToList();

        //    foreach (var items in houses)
        //    {
        //        int bedsnum = Convert.ToInt32(items.BedroomNum);
        //        for (int i = 1; i <= bedsnum; i++)
        //        {
        //            BedDtails beds = new BedDtails();
        //            beds.BedTypeID = one.Next(1, 6);
        //            beds.BedCount = one.Next(1, 7);
        //            beds.HouseKey = items.HouseKey;
        //            beds.RoomPrice = Convert.ToDecimal(one.Next(900, 10000));
        //            beds.Ispublic = Convert.ToBoolean(one.Next(0, 2));
        //            bb.Create(beds);
        //        }

        //    }
        //    return View();
        //}


        //public ActionResult inputblog() {
        //    IRepository<Pic> pic = new Repository<Pic>();
        //    string strurl1 = Server.MapPath("~/blog5-1.json");
        //    string readtext = System.IO.File.ReadAllText(strurl1);
        //    JArray array = JArray.Parse(readtext);

        //    foreach (JObject a in array.Children()) {

        //        Pic one = new Pic();

        //        one.DocID = Convert.ToInt32(a["DocID"]);
        //        one.PhotoName = Convert.ToString(a["PhotoName"]);
        //        byte[] img = Encoding.ASCII.GetBytes(Convert.ToString(a["Photo"]));
        //        one.Photo = img;
        //        one.Description = Convert.ToString(a["Description"]);
        //        one.TakeTime = Convert.ToString(a["TakeTime"]);
        //        one.IsShow = Convert.ToBoolean(a["IsShow"]);
        //        one.IsCover = Convert.ToBoolean(a["IsCover"]);
        //        pic.Create(one);
        //        //foreach (JProperty properties in a.Children())
        //        //{
        //        //    if (properties.Name == "PhotoID") {
        //        //        one.PhotoID = Convert.ToInt32( properties.Value);
        //        //    }


        //        //}

        //    }

        //    return View();
        //}


        //        public void inputMember()
        //        {
        //            List<string> names = new List<string>() {
        //        "Elinor"
        //,
        //"Elinore"
        //,
        //"Elisa"
        //,
        //"Elisabet"
        //,
        //"Elisabeth"
        //,
        //"Elisabetta"
        //,
        //"Elise"
        //,
        //"Elisha"
        //,
        //"Elissa"
        //,
        //"Elita"
        //,
        //"Eliza"
        //,
        //"Elizabet"
        //,
        //"Elizabeth"
        //,
        //"Elka"
        //,
        //"Elke"
        //,
        //"Ella"
        //,
        //"Elladine"
        //,
        //"Elle"
        //,
        //"Ellen"
        //,
        //"Ellene"
        //,
        //"Ellette"
        //,
        //"Elli"
        //,
        //"Ellie"
        //,
        //"Ellissa"
        //,
        //"Elly"
        //,
        //"Ellyn"};

        //            HouseMainRepository hrepo = new HouseMainRepository();
        //            var hlists = hrepo.GetAll().AsEnumerable().ToList();
        //            IRepository<Members> mem = new Repository<Members>();
        //            int count = 0;
        //            Random rr = new Random();
        //            List<byte[]> piccc = new List<byte[]>();
        //            for (int i = 1; i <= 22; i++) {
        //                string path = Server.MapPath("~/piccc/img" + i + ".jpg");
        //               Byte[] bys = System.IO.File.ReadAllBytes(path);
        //                piccc.Add(bys);

        //            }

        //            for (int i = 0; i <= 20; i++)
        //            {

        //                    Members onemem = new Members();

        //                    onemem.Email = names[i] + "@gmail.com";
        //                    onemem.UserName = names[i];
        //                    onemem.Password = rr.Next(1000, 5000).ToString();
        //                    onemem.UserPhoto = piccc[i];
        //                    mem.Create(onemem);



        //            }
        //        }




        //putpro ---------------------------購物商城的資料
        //public ActionResult inputpro() {
        //    IRepository<Products> pro = new Repository<Products>();

        //    string strurl1 = Server.MapPath("~/pro.json");
        //    string readtext = System.IO.File.ReadAllText(strurl1);
        //    JObject obj = JObject.Parse(readtext);
        //    var list = obj["Info"].ToList();

        //    string t = "";

        //    foreach (var data in list) {
        //        Products one = new Products();
        //        one.ProductID = Convert.ToInt32(data["ProductID"]);
        //        one.CategoryID = Convert.ToInt32(data["CategoryID"]);
        //        one.MemberID = Convert.ToInt32(data["MemberID"]);
        //        one.ProductName = Convert.ToString(data["ProductName"]);
        //        one.ProductImage = Convert.ToString(data["ProductImage"]);
        //        byte[] tobyte = Encoding.ASCII.GetBytes(Convert.ToString(data["ByteImge"]));

        //        one.ByteImge = tobyte;

        //        one.ProductContent = Convert.ToString(data["ProductContent"]);
        //        one.Quantity = Convert.ToInt32(data["Quantity"]);
        //        one.UnitPrice = Convert.ToDecimal(data["UnitPrice"]);
        //        one.Date = Convert.ToDateTime(data["Date"]);

        //        pro.Create(one);
        //    }

        //    //JArray aary = JsonConvert.DeserializeObject<JArray>(readtext);
        //    //List<JToken> obj = new List<JToken>();

        //    //foreach (var datas in aary) {
        //    //    obj.Add(datas);    

        //    //}

        //    //foreach (JToken items in obj)
        //    //{
        //    //    Products one = new Products();
        //    //    one.ProductID = Convert.ToInt32(items["ProductID"]);
        //    //    one.CategoryID = Convert.ToInt32(items["CategoryID"]);
        //    //    one.MemberID = Convert.ToInt32(items["MemberID"]);
        //    //    one.ProductName = Convert.ToString(items["ProductName"]);
        //    //    one.ProductImage = Convert.ToString(items["ProductImage"]);
        //    //    byte[] tobyte = Encoding.ASCII.GetBytes(Convert.ToString(items["ByteImge"]));

        //    //    one.ByteImge = tobyte;

        //    //    one.ProductContent = Convert.ToString(items["ProductContent"]);
        //    //    one.Quantity = Convert.ToInt32(items["Quantity"]);
        //    //    one.UnitPrice = Convert.ToDecimal(items["UnitPrice"]);
        //    //    one.Date = Convert.ToDateTime(items["Date"]);

        //    //    pro.Create(one);


        //    //}



        //    return View();
        //}







        //public ActionResult inputfiles()

        //{
        //    IRepository<Hotels> db = new Repository<Hotels>();
        //    var listss = db.GetAll().Where(n => n.City.Contains("台北市")).Take(15).ToList();
        //    string strurl2 = Server.MapPath("~/Scripts/tao.json");
        //    string strurl1 = Server.MapPath("~/Scripts/hsinchu.json");
        //    string strurl = Server.MapPath("~/Scripts/address.json");
        //    string ad = System.IO.File.ReadAllText(strurl);
        //    string ad2 = System.IO.File.ReadAllText(strurl1);
        //    string ad3 = System.IO.File.ReadAllText(strurl2);
        //    JArray arry2 = JsonConvert.DeserializeObject<JArray>(ad2);
        //    JArray arry3 = JsonConvert.DeserializeObject<JArray>(ad3);
        //    JArray arry = JsonConvert.DeserializeObject<JArray>(ad);
        //    List<string> address = new List<string>();
        //    List<JObject> adds = new List<JObject>();

        //    List<string> adhsi = new List<string>();

        //    List<string> aasa = new List<string>();
        //    foreach (var aas in arry)
        //    {
        //        address.Add(aas["Add"].ToString());
        //    }
        //    foreach (JObject aas in arry2)
        //    {
        //        adds.Add(aas);
        //    }
        //    foreach (var aas in arry3)
        //    {
        //        adhsi.Add(aas["Add"].ToString());
        //    }
        //    HouseMainRepository houses = new HouseMainRepository();

        //    var listshouse = houses.GetAll().ToList();

        //    for (var i = 0; i < listshouse.Count; i++)
        //    {
        //        if (i <= 5)
        //        {

        //            listshouse[i].State = "新北市";
        //            listshouse[i].Adress = address[i].ToString();

        //        }
        //        else if (i > 5 && i <= 12)
        //        {
        //            listshouse[i].State = "新竹";
        //            listshouse[i].Adress = adds[i - 6].Property("F4").Value.ToString();
        //        }
        //        else if (i > 12 && i <= 18)
        //        {
        //            listshouse[i].State = "桃園";
        //            listshouse[i].Adress = adhsi[i - 13].ToString();

        //        }
        //        else
        //        {
        //            listshouse[i].State = "台北市";
        //            listshouse[i].Adress = listss[i - 19].Address.ToString();
        //        }

        //        houses.Update(houses.GetByID(listshouse[i].HouseKey));
        //    }


        //    return Content(address.ToString());

        //}

    }
}