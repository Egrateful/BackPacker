using MSIT11404project1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MSIT11404project1.Areas.Blog.BlogViewModel
{
    public class BlogHomeVM
    {
        MSIT11404Entities db = new MSIT11404Entities();
        Random r = new Random();

        public BlogHomeVM() {

            this.AllMood = db.Mood.ToList();
            this.AllPlace = db.PlaceForBlog.ToList();
            ///做一個新的Class List 讓他有時間這個欄位可以比較
            List<DocTver> DocTverLs = new List<DocTver>();
            DateTime Weekago = DateTime.Now.AddDays(-7);
            DateTime Monthago = DateTime.Now.AddMonths(-1);
            var qDoc = db.Doc.Where(p => p.IsShow == true).ToList();
            foreach (var item in qDoc)
            {
                DocTver doctver = new DocTver();
                doctver.theTDoc = item;
                doctver.DocCT = Convert.ToDateTime(item.CreateTime);
                if (item.ModifyTime != null)
                {
                    doctver.DocMT = Convert.ToDateTime(item.ModifyTime);
                }
                DocTverLs.Add(doctver);
            }
            List<DocTver> LsWeekago = DocTverLs.Where(p => p.DocCT > Weekago || p.DocMT > Weekago).ToList();
            List<DocTver> LsMonthago = DocTverLs.Where(p => p.DocCT > Monthago || p.DocMT > Monthago).ToList();

            //精選文章的兩種隨機產生法
            switch (r.Next(0, 2))
            {
                case 0: //7天內最多留言 找5筆
                    this.BestDoc = LsMonthago.OrderByDescending(p => p.theTDoc.Msg.Count).Take(5).ToList();
                    break;
                case 1://7天內最多瀏覽 找5筆
                    this.BestDoc = LsMonthago.OrderByDescending(p => p.theTDoc.Good).Take(5).ToList();
                    break;
            }


            //找到各種分類的總數;
            int TotalMood = db.Mood.Count();
            int TotalPlace = db.PlaceForBlog.Count();

            //決定四個搜尋條件的ID且不重複
            int M1, M2, P1, P2;
            M1 = r.Next(1, TotalMood + 1);
            M2 = r.Next(1, TotalMood + 1);
            P1 = r.Next(1, TotalPlace+1);
            P2 = r.Next(1, TotalPlace+1);

            bool MoodC = true;
            bool PlaceC = true;

            while (MoodC)
            {
                if (M2==M1)
                {
                    M2 = r.Next(1, TotalMood + 1);
                }
                else
                {
                    MoodC = false;
                }
            };
            while (PlaceC)
            {
                if (P2 == P1)
                {
                    P2 = r.Next(1, TotalPlace);
                }
                else
                {
                    PlaceC = false;
                }
            };

            //開始用上面的四個條件來建立搜尋，並鎖定一個月內的文章;
            this.G1Doc = LsMonthago
                .Where(p => p.theTDoc.MoodID == M1)
                .OrderByDescending(p => p.theTDoc.Good)
                .Take(5).ToList();
            this.G2Doc = LsMonthago
                .Where(p => p.theTDoc.MoodID == M2)
                .OrderByDescending(p => p.theTDoc.Msg.Count)
                .Take(5).ToList();
            this.G3Doc = LsMonthago
                .Where(p => p.theTDoc.PlaceID == P1) ///原本是== P1
                .OrderByDescending(p => p.theTDoc.Good)
                .Take(5).ToList();
            this.G4Doc = LsMonthago
                .Where(p => p.theTDoc.PlaceID == P2) ///原本是== P2
                .OrderByDescending(p => p.theTDoc.Msg.Count)
                .Take(5).ToList();
            // 找尋活躍用戶(1個月內) 的人氣王跟發文王



        }
        public List<DocTver> BestDoc { get; set; }
        public List<DocTver> G1Doc { get; set; }
        public List<DocTver> G2Doc { get; set; }
        public List<DocTver> G3Doc { get; set; }
        public List<DocTver> G4Doc { get; set; }


        //等待處理中... ... ... ...
        public List<DocTver> HotDocUpter { get; set; }
        public List<DocTver> HotPicUpter { get; set; }
        //等待處理中... ... ... ...
        public List<PlaceForBlog>AllPlace{ get; set; }
        public List<Mood> AllMood { get; set; }

        public int Docid { get; set; }
        public string Title { get; set; }
        public string PlaceName { get; set; }
        public string MoodName { get; set; }
        public string UserName { get; set; }
        public string CreateTime { get; set; }
        public string ModifyTime { get; set; }
        public int Good { get; set; }
        public int TotalMSG { get; set; }
        public int IsNew { get; set; }

        [AllowHtml]
        public string imgNew { get; set; }

        public int? Mid { get; set; }
    }
    public class DocTver
    {
        public Doc theTDoc { get; set; }
        public DateTime DocCT { get; set; }
        public DateTime DocMT { get; set; }
    }

    public class BlogOneVM
    {
        MSIT11404Entities db = new MSIT11404Entities();
        public BlogOneVM(int num) {
            this.doc =db.Doc.Find(num);
            this.OwnerName = db.Doc.Find(num).Members.UserName;
            this.TheOwnerNewDoc = db.Doc.Where(p => p.Nid == this.doc.Nid).OrderByDescending(p=>p.CreateTime).Take(5).ToList();
            this.TheOwnerHotDoc = db.Doc.Where(p => p.Nid == this.doc.Nid).OrderByDescending(p=>p.Good).Take(5).ToList();
            Random rr= new Random();
            switch (rr.Next(0,4))
            {
                case 0:
                    this.TheTagNewDoc = db.Doc.Where(p => p.MoodID == this.doc.MoodID).OrderByDescending(p => p.CreateTime).Take(8).ToList();
                    this.TheTagHotDoc = db.Doc.Where(p => p.MoodID == this.doc.MoodID).OrderByDescending(p => p.Good).Take(8).ToList();
                    break;
                case 1:
                    this.TheTagNewDoc = db.Doc.Where(p => p.PlaceID == this.doc.PlaceID).OrderByDescending(p => p.CreateTime).Take(8).ToList();
                    this.TheTagHotDoc = db.Doc.Where(p => p.PlaceID == this.doc.PlaceID).OrderByDescending(p => p.Good).Take(8).ToList();
                    break;
                case 2:
                    this.TheTagNewDoc = db.Doc.Where(p => p.MoodID == this.doc.MoodID).OrderByDescending(p => p.CreateTime).Take(8).ToList();
                    this.TheTagHotDoc = db.Doc.Where(p => p.PlaceID == this.doc.PlaceID).OrderByDescending(p => p.Good).Take(8).ToList();
                    break;
                case 3:
                    this.TheTagNewDoc = db.Doc.Where(p => p.PlaceID == this.doc.PlaceID).OrderByDescending(p => p.CreateTime).Take(8).ToList();
                    this.TheTagHotDoc = db.Doc.Where(p => p.MoodID == this.doc.MoodID).OrderByDescending(p => p.Good).Take(8).ToList();
                    break;
            }
            List<Doc> ls = new List<Doc>() { };
            ls.AddRange(this.TheTagHotDoc);
            ls.AddRange(this.TheTagNewDoc);
            var ls2=ls.Distinct().Take(8).ToList();
            this.AllTagDoc = ls2;

            this.TotalOwnerDoc = db.Doc.Where(p => p.Nid == this.doc.Nid).Count();
            this.TotalMSG = db.Msg.Where(p => p.DocID == this.doc.Nid).Count();
            this.TotalHotPoint = db.Doc.Where(p => p.Nid == this.doc.Nid).Select(p => p.Good).Sum();

            foreach (var item in this.TheOwnerNewDoc)
            {
                item.CreateTime = Convert.ToDateTime(item.CreateTime).ToString();
            }
            foreach (var item in this.TheOwnerHotDoc)
            {
                item.CreateTime = Convert.ToDateTime(item.CreateTime).ToString();
            }
            foreach (var item in this.TheOwnerHotDoc)
            {
                item.CreateTime = Convert.ToDateTime(item.CreateTime).ToString();
            }
            //foreach (var item in this.TheOwnerNewDoc)
            //{
            //    item.CreateTime = Convert.ToDateTime(item.CreateTime).ToString();
            //}

        }
        public Doc doc { get; set; }
        public string OwnerName { get; set; }
        public List<Doc> TheOwnerNewDoc { get; set; }
        public List<Doc> TheOwnerHotDoc { get; set; }
        public List<Doc> TheTagNewDoc { get; set; }
        public List<Doc> TheTagHotDoc { get; set; }

        public List<Doc> AllTagDoc { get; set; }
        
        public int TotalOwnerDoc { get; set; }
        public int TotalMSG { get; set; }
        public int TotalHotPoint { get; set; }
    }

    public class MsgVM
    {
        public string Name { get; set; }
        public string Msg { get; set; }
        public string CTime { get; set; }

        public int Nid { get; set; }
    }

    public class MyBlogVM
    {
        MSIT11404Entities db = new MSIT11404Entities();
        public MyBlogVM(int id,string getby) {
            this.User = db.Members.Where(p => p.MemberID == id).First();
            this.CountMyDoc = db.Doc.Where(p => p.Nid == id).Count();
            this.CountMyPic = db.Pic.Where(p => (p.Doc).Nid == id).Count();//這樣寫是正確的嗎？
            this.CountMyMsg = db.Msg.Where(p => p.Nid == id).Count();

            var q_forp = db.Doc.FirstOrDefault(p=>p.Nid==id);
            int np = 0;
            if (q_forp != null)
            {
                np = db.Doc.Where(p => p.Nid == id).Select(p => p.Good).Sum();
            }
            else
            {
                np = 0;
            }
            this.CountMyHotPoint = np;
            var x = db.Doc.Where(p => p.Nid == id).ToList();
             foreach (var item in x)
            {
                item.CreateTime=item.CreateTime.Substring(0, item.CreateTime.Length - 4);
            }
            this.MyDocList = x;
            this.MyPicList= db.Pic.Where(p => (p.Doc).Nid==id).ToList(); //是不是要在DB多寫一個照片是誰存的欄位呢?
            var y= db.Msg.Where(p => p.Nid == id).ToList();
            foreach (var item in y)
            {
                item.CreateTime = item.CreateTime.Substring(0, item.CreateTime.Length - 4);
            }
            this.MyMsgList = y;


            this.getby = getby;
        }

        public Members User { get; set; }
        public List<Doc> MyDocList { get; set; }
        public List<Pic> MyPicList { get; set; }
        public List<Msg> MyMsgList { get; set; }
        public int CountMyDoc { get; set; }
        public int CountMyPic { get; set; }
        public int CountMyMsg { get; set; }
        public int? CountMyHotPoint { get; set; }
        public string getby { get; set; }
    }

    public class EditPicVM
    {
        MSIT11404Entities db = new MSIT11404Entities();

        public EditPicVM(int id =1)
        {
           var Picx5=db.Pic.Where(p => p.DocID == id).ToList();
            List<Pic_DTMode> Pcs = new List<Pic_DTMode>();
            foreach (var item in Picx5)
            {
                DateTime theCT=Convert.ToDateTime(item.TakeTime);
                Pic_DTMode P = new Pic_DTMode()
                {
                    apic = item,
                    yyyy = theCT.Year,
                    MM = theCT.Month,
                    dd= theCT.Day,
                    HH= theCT.Hour,
                    mm=theCT.Minute,
                    ss=theCT.Second,
                    fff=theCT.Millisecond
                };
                Pcs.Add(P);
            }
            this.pic = Pcs;
        }
        public List<Pic_DTMode> pic { get; set; }
    }

    public class Pic_DTMode
    {
        public Pic apic { get; set; }
        public int yyyy { get; set; }
        public int MM { get; set; }
        public int dd { get; set; }
        public int HH { get; set; }
        public int mm { get; set; }
        public int ss { get; set; }
        public int fff { get; set; }
    }


  

}