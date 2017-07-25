using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MSIT11404project1.Models;
using System.IO;

namespace MSIT11404project1.Areas.Job.Controllers
{
    public class JobListsController : Controller
    {
        private MSIT11404Entities db = new MSIT11404Entities();

        // GET: Job/JobLists
        public ActionResult Index()
        {
            return View(db.JobList.ToList());
        }

        // GET: Job/JobLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobList jobList = db.JobList.Find(id);
            if (jobList == null)
            {
                return HttpNotFound();
            }
            return View(jobList);
        }

        // GET: Job/JobLists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Job/JobLists/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobId,CompanyId,JobTitle,JobDescription,Contacts,Phone,Email,Address,City,Region,PostalCode,Country,EducationalBackground,Department,CompanyName")] JobList jobList,HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                if (Photo!=null)
                {
                 byte[] cover = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    Photo.InputStream.CopyTo(ms);
                    cover = ms.GetBuffer();
                }
                jobList.Photo = cover;
                }
            

                db.JobList.Add(jobList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobList);
        }

        // GET: Job/JobLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobList jobList = db.JobList.Find(id);
            if (jobList == null)
            {
                return HttpNotFound();
            }
            return View(jobList);
        }

        // POST: Job/JobLists/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobId,CompanyId,JobTitle,JobDescription,Contacts,Phone,Email,Address,City,Region,PostalCode,Country,EducationalBackground,Department,CompanyName")] JobList jobList,HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                if (Photo != null)
                {
                    byte[] cover = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Photo.InputStream.CopyTo(ms);
                        cover = ms.GetBuffer();
                    }
                    jobList.Photo = cover;
                }
                else
                {
                    IRepository<JobList> jtemp = new Repository<JobList>();
                    jobList.Photo = jtemp.GetByID(jobList.JobId).Photo;
                }
                db.Entry(jobList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobList);
        }

        // GET: Job/JobLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobList jobList = db.JobList.Find(id);
            if (jobList == null)
            {
                return HttpNotFound();
            }
            return View(jobList);
        }

        // POST: Job/JobLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobList jobList = db.JobList.Find(id);
            db.JobList.Remove(jobList);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
