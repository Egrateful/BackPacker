using MSIT11404project1.Areas.Blog.BlogViewModel;
using MSIT11404project1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MSIT11404project1.Areas.Blog.Controllers
{
    public class PMController : Controller
    {
        MSIT11404Entities db = new MSIT11404Entities();

        // GET: Blog/PM
        public ActionResult Index(int get=0, int page = 0)
        {
            PMVM db = new PMVM(page, get);
            return View(db);
        }

        [HttpPost]
        public ActionResult saveN(tosave tmp)
        {
            switch (tmp.saveby)
            {
                case "Mood":
                    var moodq = db.Mood.Find(tmp.IDn);
                    moodq.MoodName = tmp.TheContent;
                    db.SaveChanges();
                    break;
                case "Place":
                    var placeq = db.PlaceForBlog.Find(tmp.IDn);
                    placeq.PlaceName = tmp.TheContent;
                    db.SaveChanges();
                    break;
            }
            return Content("完成");
        }

        [HttpPost]
        public ActionResult IsItShow(Itshow tmp)
        {
            switch (tmp.TableName)
            {
                case "Doc":
                    var QDoc = db.Doc.Find(tmp.IDu);
                    QDoc.IsShow = tmp.IsShow;
                    db.SaveChanges();
                    break;
                case "Pic":
                    var QPic = db.Pic.Find(tmp.IDu);
                    QPic.IsShow = tmp.IsShow;
                    db.SaveChanges();
                    break;
                case "Msg":
                    var QMsg = db.Msg.Find(tmp.IDu);
                    QMsg.IsShow = tmp.IsShow;
                    db.SaveChanges();
                    break;
            }
            return Content("完成");
        }

        [HttpPost]
        public ActionResult AddNewTag(addTag tmp)
        {
            switch (tmp.TagTable)
            {
                case "Mood":
                    Mood _mood = new Mood()
                    {
                        MoodName = tmp.TagNew
                    };
                    db.Mood.Add(_mood);
                    db.SaveChanges();
                    break;
                case "Place":
                    PlaceForBlog _Place = new PlaceForBlog()
                    {
                        PlaceName = tmp.TagNew
                    };
                    db.PlaceForBlog.Add(_Place);
                    db.SaveChanges();

                    break;
            }
            //return RedirectToAction("Index", "PM", new {get=tmp.getby,page=tmp.page });
            return Content("完成");
        }


        public ActionResult DelAData(int id,int delat)
        {
            switch (delat)
            {
                case 0:
                    var qdoc=db.Doc.Find(id);
                    var Dpic=db.Pic.Where(p => p.DocID == id).ToList();
                    for (int i = 0; i <Dpic.Count; i++)
                    {
                        var dp = db.Pic.Find(Dpic[i].PhotoID);
                        db.Entry(dp).State = System.Data.Entity.EntityState.Deleted;
                    }
                    var Dmsg = db.Msg.Where(p => p.DocID == id).ToList();
                    for (int i = 0; i < Dmsg.Count; i++)
                    {
                        var dm = db.Msg.Find(Dmsg[i].MsgID);
                        db.Entry(dm).State = System.Data.Entity.EntityState.Deleted;
                    }
                    db.Entry(qdoc).State = System.Data.Entity.EntityState.Deleted;
                    break;

                case 1:
                    var qpic = db.Pic.Find(id);
                    db.Entry(qpic).State = System.Data.Entity.EntityState.Deleted;
                    break;

                case 2:
                    var qmsg = db.Msg.Find(id);
                    db.Entry(qmsg).State = System.Data.Entity.EntityState.Deleted;
                    break;
                case 3:
                    var qplace = db.PlaceForBlog.Find(id);
                    db.Entry(qplace).State = System.Data.Entity.EntityState.Deleted;
                    break;
                case 4:
                    var qmood = db.Mood.Find(id);
                    db.Entry(qmood).State = System.Data.Entity.EntityState.Deleted;
                    break;
            }
            db.SaveChanges();
            return Content("完成");
           
        } 

    }
}