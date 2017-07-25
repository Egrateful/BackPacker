using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSIT11404project1.Models;

namespace MSIT11404project1.Areas.Transaction.Controllers
{
    public class OrderListController : Controller
    {
        MSIT11404Entities db = new MSIT11404Entities();
     
     
        // GET: Transaction/OrderList
        public ActionResult List(int Mid=0 )
        {

            if(Request.Cookies["MemberID"] != null) { 
            Mid = Convert.ToInt32(Request.Cookies["MemberID"].Value);
            if(db.Orders.Where(p => p.MemberID == Mid).Count() > 0) { 
           
            ViewBag.All = db.Orders.FirstOrDefault(p => p.MemberID == Mid).OrderDetails.Sum(p=>p.UnitPrice);


            return View( db.Orders.Where(p=>p.MemberID== Mid).ToList());}
            return RedirectToAction("Index", "Products", new { area = "Transaction" });
}

            return RedirectToAction("Login", "Member", new { area = "Member" });
        }


      
    }
}