using MSIT11404project1.Models;
using MSIT11404project1.Areas.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MSIT11404project1.Areas.Transaction.Controllers
{
    public class AccountController : Controller
    {
        private MSIT11404Entities db = new MSIT11404Entities();
        // GET: Account

        public ActionResult Loging()
        {
            //var MID = db.CartsProducts.Where(p => p.MemberID == id).Select(p => p.MemberID).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Loging (LogingViewModel loging)
        {
            if (ModelState.IsValid)
            {
                //ViewBag.messge = string.Format("Email:{0}", loging.EmailAddress);

                var logingUser = db.Members.FirstOrDefault(c => c.Email == loging.Email && c.Password == loging.Password);
                if (logingUser != null)
                {
                    Response.Cookies["name"].Value = logingUser.MemberID.ToString();
                    Session["MemberID"] = logingUser.UserName;
                  

                    if (loging.RememberMe)
                    {
                        Response.Cookies["name"].Expires = DateTime.Now.AddDays(7);
                        Session["MemberID"] = logingUser.UserName;
                    }
                    return RedirectToAction("Index", "Products", new { area = "Transaction" });
                }

                            
            }

            return View();
        }
        public ActionResult Logout()
        {
            Response.Cookies["name"].Expires = DateTime.Now.AddSeconds(-1);
            Session.RemoveAll();
            return RedirectToAction("Index", "Products", new { area = "Transaction" });

        }



        // GET: Transaction/Account

    }
}