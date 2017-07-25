using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MSIT11404project1.Models;

namespace MSIT11404project1.Areas.Blog.BlogViewModel
{
    public class PMVM
    {
        public PMVM(int page = 0,int get=0)
        {
            this.getby = get;
            
            MSIT11404Entities db = new MSIT11404Entities();
           
            switch (get)
            {
                case 0:
                    this.Doc = db.Doc.OrderByDescending(p => p.DocID).Skip(page*20).Take(20).ToList();
                    this.Page = (db.Doc.Count() / 20) + 1;
                    break;
                case 1:
                    this.Pic = db.Pic.OrderByDescending(p => p.PhotoID).Skip(page * 20).Take(20).ToList();
                    this.Page = (db.Pic.Count() / 20) + 1;
                    break;
                case 2:
                    this.Msg = db.Msg.OrderByDescending(p => p.MsgID).Skip(page * 20).Take(20).ToList();
                    this.Page = (db.Msg.Count() / 20) + 1;

                    break;
                case 3:
                    this.PlaceForBlog = db.PlaceForBlog.ToList();
                    this.Page = (db.PlaceForBlog.Count() / 20) + 1;
                    break;
                case 4:
                    this.Mood = db.Mood.ToList();
                    this.Page = (db.Mood.Count() / 20) + 1;
                    break;

                case 10:
                   int totaldoc = db.Doc.Count();
                    this.TotalDoc = totaldoc;

                    var moodidq = from s in db.Doc
                                  group s by s.MoodID into g
                                  select new { keyname = g.Key, total = g.Count() };
                    List<GroupDoc> _docLS = new List<GroupDoc>();
                    foreach (var item in moodidq)
                    {
                        GroupDoc _doc = new GroupDoc();
                        _doc.Keyname = item.keyname;
                        _doc.Total = item.total;
                        _doc.Total100 =(Convert.ToDouble(item.total) / Convert.ToDouble(totaldoc)).ToString("P2");
                        _doc.Thename = db.Mood.Where(p => p.MoodID == item.keyname).Select(p => p.MoodName).First(); 
                        _docLS.Add(_doc);
                    }
                    this.Docbymood = _docLS;


                    var placeidq = from s in db.Doc
                                  group s by s.PlaceID into g
                                  select new { keyname = g.Key, total = g.Count() };
                    List<GroupDoc> _docLS2 = new List<GroupDoc>();
                    foreach (var item in placeidq)
                    {
                        GroupDoc _doc = new GroupDoc();
                        _doc.Keyname = item.keyname;
                        _doc.Total = item.total;
                        double tt = (item.total) / totaldoc;
                        _doc.Total100 = (Convert.ToDouble(item.total) / Convert.ToDouble(totaldoc)).ToString("P2");
                        _doc.Thename = db.PlaceForBlog.Where(p => p.PlaceID == item.keyname).Select(p => p.PlaceName).First();
                        _docLS2.Add(_doc);
                    }
                    this.Docbyplace = _docLS2;
                    break;


            }
        }
        public List<GroupDoc> Docbymood { get; set; }
        public List<GroupDoc> Docbyplace { get; set; }

        public List<Mood> Mood { get; set; }
        public List<PlaceForBlog> PlaceForBlog { get; set; }
        public List<Doc> Doc { get; set; }
        public List<Pic> Pic { get; set; }
        public List<Msg>Msg { get; set; }
        public  int getby { get; set; }
        public int Page { get; set;}

        public int TotalDoc { get; set; }

    }

    public class tosave
    {
        public int IDn { get; set; }
        public string saveby { get; set; }
        public string TheContent { get; set; }
    }

    public class Itshow
    {
        public int IDu { get; set; }
        public bool IsShow { get; set; }
        public string TableName { get; set; }
    }


    public class ChangeShow
    {
        public int IDu { get; set; }
        public string ChangeBy { get; set; }
    }


    public class GroupDoc
    {
        public int Keyname { get; set; }
        public string Thename { get; set; }
        public int Total { get; set; }
        public string Total100 { get; set; }
}

    public class addTag
    {
        public string TagTable { get; set; }
        public string TagNew { get; set; }

        public int getby { get; set; }
        public int  page { get; set; }



    }







}