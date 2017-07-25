using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using MSIT11404project1.Models;
using MSIT11404project1.Areas.Blog.BlogViewModel;
using System.Data.Entity;


namespace MSIT11404project1.Areas.Blog.Controllers
{
    public class MainController : Controller
    {
        MSIT11404Entities db = new MSIT11404Entities();
        public ActionResult BlogHome(int page = 1,int numby=20)
        {
            int TotalDoc = db.Doc.Count();
            if (page * numby>TotalDoc+numby)
            {
                page = 1;
                numby = 20;
            }
            page -= 1;
            //先寫假的，記得登入
            //Response.Cookies["MemberID"].Value = "2";
            //先寫假的，記得登入
            ViewBag.Place = db.PlaceForBlog.ToList();
            ViewBag.Mood = db.Mood.ToList();
            ViewBag.Page = page;
            ViewBag.TotalPage = (TotalDoc / numby) + 1;
            ViewBag.NumBy = numby;
            ViewBag.TruePage = page + 1;

            var tmp =db.Doc.Where(p=>p.IsShow==true).OrderByDescending(p => p.CreateTime).Skip(page*numby).Take(numby).ToList();
            List<BlogHomeVM> Ls = new List<BlogHomeVM>();
            foreach (var item in tmp)
            {
                BlogHomeVM aBlog = new BlogHomeVM()
                {
                    Docid = item.DocID,
                    Title = item.Title,
                    UserName = item.Members.UserName,
                    CreateTime = Convert.ToDateTime(item.CreateTime).ToString(),
                    MoodName = item.Mood.MoodName,
                    PlaceName = item.PlaceForBlog.PlaceName,
                    Good = item.Good,
                    ModifyTime = "",
                    IsNew = 2,
                    TotalMSG = 0,
                    imgNew="",
                    Mid=item.Members.MemberID
                };
                //統計留言總數
                var msgSum = from p in db.Msg
                           where p.DocID == item.DocID
                           select p;
                aBlog.TotalMSG= msgSum.Count();

                //IsNew 是狀態，
                //0 至頂
                //1 新文章
                //2 普通文章
                //有修改時間就給，沒有就保持""的值;
                //1天內新增的文章都是1
                if (item.ModifyTime != null)
                {
                    aBlog.ModifyTime = item.ModifyTime;
                    DateTime dt = Convert.ToDateTime(item.ModifyTime);
                    if (DateTime.Now<dt.AddDays(1))
                    {
                        aBlog.IsNew = 1;
                        aBlog.imgNew = @"<span class='blink'><b>NEW<b></span>";
                    }
                }
                else
                {
                    DateTime dt = Convert.ToDateTime(item.CreateTime);
                    if (DateTime.Now<dt.AddDays(1))
                    {
                        aBlog.IsNew = 1;
                        aBlog.imgNew = @"<span class='blink'><b>NEW<b></span>";
                    }
                }
                Ls.Add(aBlog);
            }
            
            return View(Ls);
        }

        //頁數與比數篩選
        [HttpPost]
        ActionResult BlogPage(int numby=20,int page=0)
        {
            int g=page - 1;
            return RedirectToAction("BlogHome", new { numby=numby, page= g });
        }
        [HttpPost] //多重篩選搜尋
        public ActionResult BlogHome(List<string> AllPlace, List<string> AllMood,string SSTime,string SWord)
        {
            List<string> LsPlace = new List<string>();
            List<string> LsMood = new List<string>();
            for (int i = 0; i <AllPlace.Count ; i++)
            {
                if (AllPlace[i]!="0")
                {
                    LsPlace.Add(AllPlace[i]);
                }
            }
            for (int i = 0; i < AllMood.Count; i++)
            {
                if (AllMood[i] != "0")
                {
                    LsMood.Add(AllMood[i]);
                }
            }
            DateTime theDT = DateTime.Now;
            switch (SSTime)
            {
                case"一日內":
                    theDT = DateTime.Now.AddDays(-1);
                    break;
                case "一週內":
                    theDT = DateTime.Now.AddDays(-7);
                    break;
                case "一個月內":
                    theDT = DateTime.Now.AddMonths(-1);
                    break;
                case "三個月內":
                    theDT = DateTime.Now.AddMonths(-3);
                    break;
                case "半年內":
                    theDT = DateTime.Now.AddMonths(-6);
                    break;
                case "一年內":
                    theDT = DateTime.Now.AddYears(-1);
                    break;
                case "0":
                    theDT = DateTime.Now;
                    break;
            }


            List<DocTver> LsT = new List<DocTver>();
            if (SWord != "0") ///1
            {
                if (LsPlace.Count != 0 && LsMood.Count != 0) ///1-1
                {
                    var Q = db.Doc.Where(p => p.Title.Contains(SWord)).Where(p => LsPlace.Contains(p.PlaceForBlog.PlaceName)).Where(p => LsMood.Contains(p.Mood.MoodName)).ToList();
                    if (Q != null)
                    {
                        foreach (var item in Q) 
                        {
                            DocTver D = new DocTver();
                            D.theTDoc = item;
                            D.DocCT = Convert.ToDateTime(item.CreateTime);
                            D.DocMT = DateTime.Now;
                            LsT.Add(D);
                        }
                    }
                }
                else if (LsPlace.Count != 0 && LsMood.Count == 0)///1-2
                {
                    var Q = db.Doc.Where(p => p.Title.Contains(SWord)).Where(p => LsPlace.Contains(p.PlaceForBlog.PlaceName)).ToList();
                    if (Q != null)
                    {
                        foreach (var item in Q)
                        {
                            DocTver D = new DocTver();
                            D.theTDoc = item;
                            D.DocCT = Convert.ToDateTime(item.CreateTime);
                            D.DocMT = DateTime.Now;
                            LsT.Add(D);
                        }
                    }
                }
                else if (LsPlace.Count == 0 && LsMood.Count != 0)///1-3
                {
                    var Q = db.Doc.Where(p => p.Title.Contains(SWord)).Where(p => LsMood.Contains(p.Mood.MoodName)).ToList();
                    if (Q != null)
                    {
                        foreach (var item in Q)
                        {
                            DocTver D = new DocTver();
                            D.theTDoc = item;
                            D.DocCT = Convert.ToDateTime(item.CreateTime); 
                            D.DocMT = DateTime.Now;
                            LsT.Add(D);
                        }
                    }
                }
                else  ///1-4
                {
                    var Q = db.Doc.Where(p => p.Title.Contains(SWord)).ToList();
                    if (Q != null)
                    {
                        foreach (var item in Q)
                        {
                            DocTver D = new DocTver();
                            D.theTDoc = item;
                            D.DocCT = Convert.ToDateTime(item.CreateTime); 
                            D.DocMT = DateTime.Now;
                            LsT.Add(D);
                        }
                    }
                }
            }
            else ////2.
            {
                if (LsPlace.Count != 0 && LsMood.Count != 0) ///2-1
                {
                    var Q = db.Doc.Where(p => LsPlace.Contains(p.PlaceForBlog.PlaceName)).Where(p => LsMood.Contains(p.Mood.MoodName)).ToList();
                    if (Q != null)
                    {
                        foreach (var item in Q)
                        {
                            DocTver D = new DocTver();
                            D.theTDoc = item;
                            D.DocCT = Convert.ToDateTime(item.CreateTime);
                            D.DocMT = DateTime.Now;
                            LsT.Add(D);
                        }
                    }
                }
                else if (LsPlace.Count != 0 && LsMood.Count == 0)///2-2
                {
                    var Q = db.Doc.Where(p => LsPlace.Contains(p.PlaceForBlog.PlaceName)).ToList();
                    if (Q != null)
                    {
                        foreach (var item in Q)
                        {
                            DocTver D = new DocTver();
                            D.theTDoc = item;
                            D.DocCT = Convert.ToDateTime(item.CreateTime);
                            D.DocMT = DateTime.Now;
                            LsT.Add(D);
                        }
                    }
                }
                else if (LsPlace.Count == 0 && LsMood.Count != 0)///2-3
                {
                    var Q = db.Doc.Where(p => LsMood.Contains(p.Mood.MoodName)).ToList();
                    if (Q != null)
                    {
                        foreach (var item in Q)
                        {
                            DocTver D = new DocTver();
                            D.theTDoc = item;
                            D.DocCT = Convert.ToDateTime(item.CreateTime);
                            D.DocMT = DateTime.Now;
                            LsT.Add(D);
                        }
                    }
                }
                else  ///2-4
                {
                    var Q = db.Doc.ToList();
                    if (Q != null)
                    {
                        foreach (var item in Q)
                        {
                            DocTver D = new DocTver();
                            D.theTDoc = item;
                            D.DocCT = Convert.ToDateTime(item.CreateTime);
                            D.DocMT = DateTime.Now;
                            LsT.Add(D);
                        }
                    }
                }
            }

            List<DocTver> OKLs = new List<DocTver>();
            if (SSTime != "0")
            {
                foreach (var item in LsT)
                {
                    if (item.DocCT > theDT)
                    {
                        OKLs.Add(item);
                    }
                }
            }
            else
            {
                foreach (var item in LsT)
                {
                    OKLs.Add(item);
                }
            }


            if (OKLs.Count !=0 )
            {
                return View("SchDoc", OKLs);
            }
            else
            {
                //沒有結果，自動跳轉回去首頁
                return View("NoData");
             }
        }


        public ActionResult TurnToHtml(string tmp)
        {
            ViewBag.tmp = tmp;
            return PartialView();
        }
        public ActionResult BlogOne(int id =1)
        {
            string key = "Readed" + id;
            if (Request.Cookies[key] == null)
            {
                var q = db.Doc.Find(id);
                if (q!=null)
                {
                    q.Good = q.Good + 1;
                    db.Entry(q).State = EntityState.Modified;
                    db.SaveChanges();
                    Response.Cookies[key].Value = "see";
                    Response.Cookies[key].Expires = DateTime.Now.AddDays(1);
                }
            }
            
            ///處理照片>>>>>>>>>>>>>>>>>>>>>>>>>
            ViewBag.Pics = db.Pic.Where(p => p.DocID == id);
            ///處理照片=========================

            ///文章部分與其他相關搜尋>>>>>>>>>>>>>>>>>>>>>>>>>
            BlogOneVM blogonevm = new BlogOneVM(id);
            if (blogonevm.doc.ModifyTime!=null)
            {
                blogonevm.doc.ModifyTime = Convert.ToDateTime(blogonevm.doc.ModifyTime).ToString();

            }
            ///文章部分與其他相關搜尋=========================

            /////處理這篇文章的留言>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            var Msgs = db.Msg.Where(p => p.DocID == id&&p.IsShow==true).OrderByDescending(p => p.CreateTime).ToList();

       
                List<MsgVM> MSGLs = new List<MsgVM>();
                foreach (var item in Msgs)
                {
                    MsgVM tmp = new MsgVM();
                    tmp.Name = item.Members.UserName;
                    tmp.Msg = item.Msg1;
                    string MsgDate = Convert.ToDateTime(item.CreateTime).ToString();
                    tmp.CTime = $"於 {MsgDate} 發表";
                    tmp.Nid = Convert.ToInt32(item.Nid);
                    MSGLs.Add(tmp);
                }
                ViewBag.MSG = MSGLs;
            
            /////處理這篇文章的留言=======================================


            return View(blogonevm);
        }
        [HttpPost]//發出留言的時候走這條
        public ActionResult BlogOne(Msg tmp,int DocID)
        {
            //tmp.Nid = 3; //到時候要判斷使用者的登入 ?????????????????????????
            tmp.Nid=Convert.ToInt32(Request.Cookies["MemberID"].Value);
            tmp.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            tmp.IsShow = true;
            tmp.Msg1 = Request.Form["Msg1"];
            db.Msg.Add(tmp);
            db.SaveChanges();
            return RedirectToAction("BlogOne", new { id = tmp.DocID });
        }
        public ActionResult IsNew(int id = 2)
        {
            if (id==1)
            {
                string paths = Server.MapPath(@"~\Areas\Blog\PicForBlog\new.gif");
                byte[] pics = System.IO.File.ReadAllBytes(paths);
                return File(pics, "image/gif");
            }
            else
            {
                return File(" ", "text/html");
            }
        }

        //傳文章ID近來，自動找到封面圖回傳
        public ActionResult GetImage(int id = 1)
        {
            Pic thepic = db.Pic.Where(p => p.IsCover == true).Where(p => p.DocID == id).First();
            byte[] img = thepic.Photo;
            return File(img, "image/jpeg");
        }

        //傳照片ID近來，回傳照片
        public ActionResult GetImage2(int id = 1)
        {
            Pic thepic = db.Pic.Where(p => p.PhotoID==id).First();
            byte[] img = thepic.Photo;
            return File(img, "image/jpeg");
        }


        [HttpGet]
        public ActionResult PoDoc()
        {
                ViewBag.Place = db.PlaceForBlog.ToList();
                ViewBag.Mood = db.Mood.ToList();
                return View();
        }

        [HttpPost]
        public ActionResult PoDoc(Doc doc,Pic pic, HttpPostedFileBase myPhoto, HttpPostedFileBase myPhoto2, HttpPostedFileBase myPhoto3, HttpPostedFileBase myPhoto4, HttpPostedFileBase myPhoto5)
        {
            //===== 處理文章 ============================>>>>>
            //doc.Nid= 1; //要抓到使用者是誰?????????????????????????
            doc.Nid = Convert.ToInt32(Request.Cookies["MemberID"].Value);

            string thetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            doc.CreateTime = thetime;
            doc.IsShow = true;
            db.Doc.Add(doc);
            int newdocid = doc.DocID;
        

            //封面
            //byte[] cover = null;
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    myPhoto.InputStream.CopyTo(ms);
            //    cover = ms.GetBuffer();
            //}
            //pic.Photo = cover;

            var photosize = myPhoto.ContentLength;
            byte[] photobyte = new byte[photosize];
            myPhoto.InputStream.Read(photobyte, 0, photosize);
            pic.Photo = photobyte;

            pic.PhotoName = myPhoto.FileName;
            pic.DocID = newdocid;
            pic.IsShow = true;
            pic.IsCover = true;
            db.Pic.Add(pic);

            db.SaveChanges();

            //剩下的四張
            List<HttpPostedFileBase> Pics = new List<HttpPostedFileBase>();
            if (myPhoto2 != null)
            {
                Pics.Add(myPhoto2);
            }
            if (myPhoto3 != null)
            {
                Pics.Add(myPhoto3);
            }
            if (myPhoto4 != null)
            {
                Pics.Add(myPhoto4);
            }
            if (myPhoto5 != null)
            {
                Pics.Add(myPhoto5);
            }

            foreach (var pp in Pics)
            {
                Pic mypic = new Pic();
                byte[] PicByte = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    pp.InputStream.CopyTo(ms);
                    PicByte = ms.GetBuffer();
                }
                mypic.PhotoName = pp.FileName;//找照片名稱的設定
                mypic.DocID = newdocid;
                mypic.Photo =PicByte;
                mypic.IsCover = false;
                mypic.IsShow = true;
                db.Pic.Add(mypic);
                db.SaveChanges();
            }
            //===== 處理圖片 ==================================
            return RedirectToAction("BlogHome");
        }
        ////////////////////////使用者專區>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        public ActionResult MyBlog(int id = 1,string getby = "MyFunc")
        {
            if (Request.Cookies["MemberID"]!=null)
            {
                id = Convert.ToInt32(Request.Cookies["MemberID"].Value);
                MyBlogVM myblogvm = new MyBlogVM(id, getby);
                return View(myblogvm);
            }
            else
            {
                return RedirectToAction("Login", "Member", new { area = "Member" });
            }
        }

        //上傳大頭照
        [HttpPost]
        public ActionResult MyBlog(HttpPostedFileBase UserPhoto, int Uid = 1)
        {
            var q= db.Members.Where(p => p.MemberID == Uid).FirstOrDefault();
            byte[] cover = null;
            using (MemoryStream ms = new MemoryStream())
            {
                UserPhoto.InputStream.CopyTo(ms);
                cover = ms.GetBuffer();
            }
            q.UserPhoto = cover;
            db.Entry(q).State = EntityState.Modified;
            db.SaveChanges();
            //return RedirectToAction("MyBlog",new {id=Uid});
            return RedirectToAction("MyBlog");

        }

        ///傳使用者ID
        public ActionResult GetHeadPic(int id = 0)
        {
            byte[] img = db.Members.Where(p => p.MemberID == id).Select(p=>p.UserPhoto).FirstOrDefault();
            if (img != null)
            {
                return File(img, "image/jpeg");
            }
            else
            {
                string paths = Server.MapPath(@"~\Areas\Blog\PicForBlog\DFHeadPic.png");
                byte[] pics = System.IO.File.ReadAllBytes(paths);
                return File(pics, "image/png");
            }
        }
        public ActionResult EditDoc(int id = 1)
        {

            ViewBag.Place = db.PlaceForBlog.ToList();
            ViewBag.Mood = db.Mood.ToList();
            return View(db.Doc.Where(p => p.DocID == id).First());
        }

        [HttpPost]
        public ActionResult EditDoc(Doc newdoc)
        {
            Doc doc=db.Doc.Find(newdoc.DocID);
            doc.Title = newdoc.Title;
            doc.Article=newdoc.Article;
            doc.MoodID = newdoc.MoodID;
            doc.PlaceForBlog = newdoc.PlaceForBlog;
            doc.ModifyTime= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            db.Entry(doc).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("MyBlog",new { id = doc.Members.MemberID });
        }

        public ActionResult DelDoc(int id = 1)
        {
            Doc TheDoc=db.Doc.Find(id);
            db.Entry(TheDoc).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("MyBlog", new { id = TheDoc.Members.MemberID });
        }

        //傳文章ID進來
        public ActionResult EditPic(int id = 1)
        {

            var q = db.Pic.Where(p => p.DocID == id);
    
            
                ViewBag.MemberID = db.Doc.Where(p => p.DocID == id).Select(p => p.Nid).FirstOrDefault();
                ViewBag.DocID = db.Doc.Where(p => p.DocID == id).Select(p => p.DocID).First();

                return View(db.Pic.Where(p => p.DocID == id));
        }
        [HttpPost]
        public ActionResult EditPic()
        {
            int CoverNum = Convert.ToInt32(Request.Form["WhoIsCover"]);
            string[] PhotoIDAr = Request.Form.GetValues("item.PhotoID");
            string[] PhotoNameAr= Request.Form.GetValues("item.PhotoName");
            string[] DescriptionAr = Request.Form.GetValues("Description");
            if (PhotoIDAr!=null)
            {
                for (int i = 0; i < PhotoIDAr.Count(); i++)
                {
                    int PicNum = Convert.ToInt32(PhotoIDAr[i]);
                    Pic mdPic = db.Pic.Find(PicNum);
                    mdPic.PhotoName = PhotoNameAr[i];
                    mdPic.Description = DescriptionAr[i];
                    mdPic.IsCover = false;
                    if (PicNum == CoverNum)
                    {
                        mdPic.IsCover = true;
                    }
                    db.Entry(mdPic).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("MyBlog", new { id = Convert.ToInt32(Request.Form["ThisMemberID"]) });
            }
            else
            {
                return RedirectToAction("MyBlog");
            }
        }

        [HttpPost] //快速修改單張照片
        public ActionResult EditAPic(int PicID, int Uid,string PhotoName , string Description, HttpPostedFileBase UserPhoto)
        {
            Pic APic = db.Pic.Find(PicID);
           
            byte[] cover = null;
            if (UserPhoto !=null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    UserPhoto.InputStream.CopyTo(ms);
                    cover = ms.GetBuffer();
                }
                APic.Photo = cover;
            }
            APic.PhotoName = PhotoName;
            APic.Description = Description;
            db.Entry(APic).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("MyBlog", new { id = Uid });
        }

        public ActionResult Up5Pic(int id =1)
        {
            ViewBag.DocID = id;
            return View();
        }


        [HttpPost]
        public ActionResult Up5Pic(int ThisDocID, string[] PicDe,string[] PicNa,HttpPostedFileBase myPhoto, HttpPostedFileBase myPhoto2, HttpPostedFileBase myPhoto3, HttpPostedFileBase myPhoto4, HttpPostedFileBase myPhoto5)
        {
            var xcover = db.Pic.Where(z => z.DocID == ThisDocID && z.IsCover == true).FirstOrDefault();
            if (xcover!=null)
            {
                db.Entry(xcover).State = EntityState.Deleted;
                db.SaveChanges();
            }

            Pic mypic = new Pic();
            var photosize = myPhoto.ContentLength;
            byte[] photobyte = new byte[photosize];
            myPhoto.InputStream.Read(photobyte, 0, photosize);
            mypic.Photo = photobyte;
            if (PicNa[0].Trim()=="")
            {
                mypic.PhotoName = myPhoto.FileName;
            }
            else
            {
                mypic.PhotoName = PicNa[0];
            }
            if (PicDe[0].Trim()!="")
            {
                mypic.Description = PicDe[0];
            }
            mypic.DocID = ThisDocID;
            mypic.IsShow = true;
            mypic.IsCover = true;
            db.Pic.Add(mypic);
            db.SaveChanges();


            List<HttpPostedFileBase> Pics = new List<HttpPostedFileBase>();
            if (myPhoto2 != null)
            {
                Pics.Add(myPhoto2);
            }
            if (myPhoto3 != null)
            {
                Pics.Add(myPhoto3);
            }
            if (myPhoto4 != null)
            {
                Pics.Add(myPhoto4);
            }
            if (myPhoto5 != null)
            {
                Pics.Add(myPhoto5);
            }
            int totalPic = Pics.Count;
            for (int i = 0; i < totalPic; i++)
            {
                //var objk = db.Pic.Where(p => p.IsCover == false && p.DocID == ThisDocID).FirstOrDefault();
                //if (objk != null)
                //{
                //    db.Entry(objk).State = EntityState.Deleted;
                //}
                Pic otherpic = new Pic();
                byte[] PicByte = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    Pics[i].InputStream.CopyTo(ms);
                    PicByte = ms.GetBuffer();
                }

                //var tosize = Pics[i].ContentLength;
                //byte[] tobyte = new byte[tosize];
                //myPhoto.InputStream.Read(tobyte, 0, tosize);
                //otherpic.Photo = tobyte;



                otherpic.PhotoName =(PicNa[i+1].Trim()!="")? PicNa[i + 1] : Pics[i].FileName;//找照片名稱的設定
                otherpic.Description = (PicDe[i + 1].Trim() != "") ? PicDe[i + 1] : "";
                otherpic.DocID = ThisDocID;
                otherpic.Photo = PicByte;
                otherpic.IsCover = false;
                otherpic.IsShow = true;
                db.Pic.Add(otherpic);
                db.SaveChanges();
            }
            return RedirectToAction("EditPic",new { id=ThisDocID});
        }


        public ActionResult DelPic(int id = 1)
        {
            Pic ThePic = db.Pic.Find(id);
            int ThisDocID = db.Pic.Where(p => p.PhotoID == id).Select(p => p.DocID).First();
            db.Entry(ThePic).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("EditPic", new { id = ThisDocID });
        }

        public ActionResult EditMsg(int id = 1)
        {
            var datas = db.Msg.Where(p => p.DocID == id);
            if (datas.Count()!=0)
            {
            foreach (var item in datas)
            {
                item.CreateTime = Convert.ToDateTime(item.CreateTime).ToString();
            }
            return View(datas);
            }
            return View("NoValue");
        }

        [HttpPost]
        public ActionResult EditMsg()
        {
            //string IsShowAr= Request.Form["IsTheMsgShow"];
            //var tmp = Request.Form["TheMsgID"];
            string[] MsgIDAr =Request.Form.GetValues("TheMsgID");
            string[] ISIS = Request.Form.GetValues("ISIS");
            if (MsgIDAr !=null )
            {
                for (int i = 0; i < MsgIDAr.Count(); i++)
                {
                    string MsgNum = MsgIDAr[i];
                    int num = Convert.ToInt32(MsgNum);
                    Msg mdMsg = db.Msg.Find(num);
                    if (ISIS[i] == "true")
                    {
                        mdMsg.IsShow = true;
                    }
                    else
                    {
                        mdMsg.IsShow = false;
                    }
                    db.Entry(mdMsg).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("MyBlog");
            }
            else
            {
                return RedirectToAction("MyBlog");

            }

        }

        public ActionResult DelMsg(int id = 1)
        {
            var q=db.Msg.Find(id);
            db.Entry(q).State = EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("MyBlog");
        }
        //////////////////////////使用者專區=======================================
    }
}