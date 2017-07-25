using MSIT11404project1.Areas.Transaction.ViewModel;
using MSIT11404project1.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MSIT11404project1.Areas.Transaction.Controllers
{
    public class CategorysController : Controller
    {
        MSIT11404Entities db = new MSIT11404Entities();
        // GET: Transaction/Categorys
        public ActionResult CategorysIndex(int id = 0)
        {



            return View(db.Categorys.ToList());
        }

        
        
        //==產品列表
        public ActionResult Products(int? page, string sortBy, int id = 0)
        {
            var prodicts = db.Products.AsQueryable();



            ViewBag.SortDate = string.IsNullOrEmpty(sortBy) ? "Date desc" : "";
            ViewBag.SortUnitPrice = sortBy == "UnitPrice" ? "UnitPrice desc" : "UnitPrice";

            switch (sortBy)
            {
                case "Date desc":
                    prodicts = prodicts.OrderByDescending(p => p.Date);
                    break;


                case "UnitPrice desc":
                    prodicts = prodicts.OrderByDescending(p => p.UnitPrice);
                    break;

                case "UnitPrice":
                    prodicts = prodicts.OrderBy(p => p.UnitPrice);
                    break;


                default:
                    prodicts = prodicts.OrderBy(p => p.Date);
                    break;
            }

            return View(prodicts.ToList().ToPagedList(page ?? 1, 18));
        }

        //================刪除===========
        public ActionResult Delete(int id = 0)
        {
            if (db.Products.Where(p => p.CategoryID == id).Count() == 0)
            {
                Categorys delete = db.Categorys.Find(id);
                db.Categorys.Remove(delete);
                db.SaveChanges();
                return RedirectToAction("Products", "Categorys", new { area = "Transaction" });
            }
            return RedirectToAction("Products", "Categorys", new { area = "Transaction" });
        }




        public ActionResult PDelete(int id = 0)
        {
            if (db.OrderDetails.Where(p => p.ProductID == id).Count() == 0)
            {
                Products delete = db.Products.Find(id);
                db.Products.Remove(delete);
                db.SaveChanges();
                return RedirectToAction("Products", "Categorys", new { area = "Transaction" });
            }
            return RedirectToAction("Products", "Categorys", new { area = "Transaction" });
        }
        //================修改產品================

        public ActionResult Categorys(int id = 0)
        {

            ViewBag.item = db.Products.Where(p => p.CategoryID == id).Count();
            return PartialView(db.Categorys.ToList());
        }
        [HttpGet]
        public ActionResult Update(int id = 0)
        {
            return View(db.Categorys.Find(id));

        }
        [HttpPost]
        public ActionResult Update(Categorys _cat)
        {


            db.Entry(_cat).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Products", "Categorys", new { area = "Transaction" });

        }



        [HttpGet]
        public ActionResult PUpdate(int id = 0)
        {
           
                ViewBag.Categorys = db.Categorys.ToList();

                Products _product = db.Products.Find(id);

                return View(_product);
          
        }
        [HttpPost]
        public ActionResult PUpdate(Products _product, HttpPostedFileBase ProductImage)
        {

            if (ModelState.IsValid)
            {
                Products p = new Products();
                if (p.ProductImage != _product.ProductImage)
                {
                    var imgSize = ProductImage.ContentLength;//檔案大小
                    byte[] imgbyte = new byte[imgSize];//轉byte
                    ProductImage.InputStream.Read(imgbyte, 0, imgSize);//InputStream讀取
                    _product.ProductImage = ProductImage.FileName;
                    _product.ByteImge = imgbyte;
                    //_product.Date = DateTime.Now;

                    db.Entry(_product).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Products", "Categorys", new { area = "Transaction" });
                }
            }
            ViewBag.Categorys = db.Categorys.ToList();
            db.Entry(_product).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Products", "Categorys", new { area = "Transaction" });
        }
        //==新增類別======
        [HttpGet]
        public ActionResult Create() {

            return View(); 
        }
        [HttpPost]

        public ActionResult Create(Categorys _cat)
        {
            db.Categorys.Add(_cat);
            db.SaveChanges();
            return RedirectToAction("Products", "Categorys", new { area = "Transaction" });
        }

        //======產品詳細資料======


        [HttpGet]
        public ActionResult ProductsDetailed(int id = 0)
        {
            ViewBag.Categories = db.Categorys.ToList();
            Products _product = db.Products.Find(id);
            return View(_product);
        }


    }
}