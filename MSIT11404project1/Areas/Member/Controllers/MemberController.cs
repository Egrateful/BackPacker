using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSIT11404project1.Areas.Member.ViewModel;
using MSIT11404project1.Models;
using CaptchaMvc.HtmlHelpers;
using System.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace MSIT11404project1.Areas.Member.Controllers
{
    public class MemberController : Controller
    {

        MSIT11404Entities db = new MSIT11404Entities();
        IRepository<Models.Members> member = new Repository<Models.Members>();

        [HttpGet]
        public ActionResult MemberJoin()
        {
            return View();
        }
                
        [HttpPost]
        public ActionResult MemberJoin(Models.Members _members)
        {
            member.Create(_members);
            return RedirectToAction("MemberJoinDone", "Member", new { Area = "Member" });
        }

        public ActionResult MemberJoinDone()
        {
            ViewBag.message = string.Format("歡迎成為 BackPacker 會員 !!!");
            return View();
        }


        [HttpGet]
        public ActionResult MemberEdit(int id = 0)
        {
            //將資料傳給View
            return View(member.GetByID(id));
        }

        [HttpPost]
        public ActionResult MemberEdit(Models.Members _member)
        {
            member.Update(_member);
            return RedirectToAction("GetAllMember", "Member", new { Area = "Admin" });
        }

        //public ActionResult MemberEdit(Members _member, HttpPostedFileBase byteimg)
        //{
        //    if (byteimg != null)
        //    {
        //        _member.UserPhoto = new byte[byteimg.ContentLength];
        //        byteimg.InputStream.Read(_member.UserPhoto, 0, byteimg.ContentLength);
        //    }
        //    member.Update(_member);
        //    return RedirectToAction("GetAllMember", "Member", new { Area = "Admin" });
        //}

        public ActionResult MemberDelete(int id = 0)
        {
            member.Delete(member.GetByID(id));
            //轉到另外一個Action
            return RedirectToAction("GetAllMember", "Member", new { Area = "Member" });
        }

        public ActionResult CaptchaImage(string prefix, bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            //generate new question 
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            var captcha = string.Format("{0} + {1} = ?", a, b);

            //store answer 
            Session["Captcha" + prefix] = a + b;

            //image stream 
            FileContentResult img = null;

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise 
                if (noisy)
                {
                    int i, r, x, y;
                    //var pen = new Pen(Color.Yellow);
                    var pen = new Pen(Color.DarkBlue);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);
                   
                        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                    }
                }

                //add question 
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);

                //render as Jpeg 
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }

            return img;

        }

         public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {  
                IRepository<LoginRecord> logr = new Repository<LoginRecord>();
                Models.Members loginUser = db.Members.FirstOrDefault(u => u.Email == login.Email && u.Password == login.Password);
                if (loginUser != null)
                {
                    if (Session["Captcha"] != null && Session["Captcha"].ToString() == login.Captcha)
                    {
                        Response.Cookies["MemberID"].Value = loginUser.MemberID.ToString();
                        Response.Cookies["MemberID"].Expires = DateTime.Now.AddDays(7);
                        LoginRecord one = new LoginRecord();
                        one.MemberID = loginUser.MemberID;
                        one.LoginTime = DateTime.Now;
                        one.LogoutTime = DateTime.Now.AddDays(7);
                        logr.Create(one);
                        if (Request.Form["admin"] != null) {
                            IRepository<Members> mem = new Repository<Members>();
                            Members one11 =mem.GetByID(loginUser.MemberID);
                            if (one11.IsAdminitrator)
                            {

                                Response.Cookies["IsAdmin"].Value = "Yes";
                                TempData["wel"]= "歡迎" + one11.UserName + "管理員";
                                return RedirectToAction("Index", "Home", new { area = "Admin" });
                                
                            }
                            else {
                                ViewBag.answer = "您不是管理員";
                                return View();
                            }
                        }
                        //if (login.RememberMe)
                        //{
                        //    Response.Cookies["name"].Expires = DateTime.Now.AddDays(7);
                        //}

                        

                        return RedirectToAction("BlogHome", "Main", new { Area = "Blog" });
                    }

                    ModelState.AddModelError("Captcha", "驗證碼錯誤，請再輸入一次!");
                }
               
            }
            return View();

        }

        public ActionResult LoginOK()
        {
            Models.Members loginUser = new Models.Members();
            if (Request.Cookies["MemberID"] != null)
            {
                int userID = Convert.ToInt32(Request.Cookies["MemberID"].Value);
                //"歡迎您的到來， 正在登入中。。。"

                var x = from p in db.Members where p.MemberID == userID select p;
                Models.Members y = x.First();

                ViewBag.message = string.Format("Hello {0} ! 歡迎回到 BackPacker....", y.UserName);
                return View();
            }
            else {
                return RedirectToAction("Login", "Member", new { area = "Member" });
            }
            
        }

        public ActionResult Logout()
        {
            if (Request.Cookies["CheckR"] != null) {
                Response.Cookies["CheckR"].Expires = DateTime.Now.AddSeconds(-1);
            }
            if (Request.Cookies["LandCheck"] != null) {
                Response.Cookies["LandCheck"].Expires = DateTime.Now.AddSeconds(-1);
            }

            if (Request.Cookies["IsAdmin"] != null) {
                Response.Cookies["IsAdmin"].Expires = DateTime.Now.AddSeconds(-1);
            }
            int memberid =Convert.ToInt32( Request.Cookies["MemberID"].Value);
            IRepository<LoginRecord> logR = new Repository<LoginRecord>();

            LoginRecord entitylog = logR.GetAll().Where(n => n.MemberID == memberid).OrderByDescending(n => n.LoginTime).First();
            entitylog.LogoutTime = DateTime.Now;
            logR.Update(entitylog);

            Response.Cookies["MemberID"].Expires = DateTime.Now.AddSeconds(-1);
            Session.Abandon();
            return RedirectToAction("Login","Member",new {area="Member" });
        }

        

    }
}