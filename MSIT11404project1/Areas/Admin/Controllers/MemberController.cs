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
    public class MemberController : Controller
    {
        MSIT11404Entities db = new MSIT11404Entities();
        IRepository<Members> member = new Repository<Members>();

        public ActionResult GetAllMember(int page = 1, int perpage = 8)
        {
            var list = db.Members.ToList();

            //轉到另外一個Action
            return View(list.ToList().ToPagedList(page, perpage));
        }

        // GET: Member/Default
        [HttpGet]
        public ActionResult MemberInsert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MemberInsert(Members _members)
        {
            member.Create(_members);
            return RedirectToAction("GetAllMember", "Member", new { Area = "Admin" });
        }

        [HttpGet]
        public ActionResult MemberEdit(int id=0)
        {
            //將資料傳給View
            return View(member.GetByID(id));
        }

        [HttpPost]
        public ActionResult MemberEdit(Members _member)
        {
            member.Update(_member);
            return RedirectToAction("GetAllMember", "Member", new { Area = "Admin"});
        }

        public ActionResult MemberDelete(int id = 0)
        {
            member.Delete(member.GetByID(id));
            //轉到另外一個Action
            return RedirectToAction("GetAllMember", "Member", new { Area = "Admin" });
        }
        
    }

}