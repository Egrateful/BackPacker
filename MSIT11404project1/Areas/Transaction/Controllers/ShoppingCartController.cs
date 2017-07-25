using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSIT11404project1.Models;

namespace MSIT11404project1.Areas.Transaction.Controllers
{
    
    public class ShoppingCartController : Controller
    {
        MSIT11404Entities db = new MSIT11404Entities();
        // GET: Transaction/ShoppingCart
        public ActionResult CartIndex( int id=0)
        {
            if (db.CartsProducts.Count() != 0)
            {
                var q = db.CartsProducts.Sum(p => p.Subtotal);

                CartsProducts _cartsProducts = new CartsProducts();
                id = Convert.ToInt32(Request.Cookies["MemberID"].Value);
                if (Request.Cookies["MemberID"] != null)
                {

                    var list = db.CartsProducts.Where(cp => cp.MemberID == id).ToList();
                    ViewBag.all = string.Format("{0:c0}", q);
                    return View(list);
                }


                return RedirectToAction("Login", "Member", new { area = "Member" });
            }
            return RedirectToAction("Index", "Products", new { area = "Transaction" });
        }

        //修改數量
        public ActionResult CartIDetailes(int id =0)
        {
            ViewBag.Categories = db.Categorys.ToList();
            Products _product = db.Products.Find(id);

            return View(_product);
                }
        [HttpPost]
   
             public ActionResult CartIDetailes(CartsProducts _cartsproducts,int Quantity=1, int id = 0)
        { 
            Products _product = db.Products.Find(id);
            _cartsproducts.Quantity = Quantity;
            _cartsproducts.Subtotal = _product.UnitPrice* Quantity;


        db.Entry(_cartsproducts).State=System.Data.Entity.EntityState.Modified;

                db.SaveChanges();
            return RedirectToAction("CartIndex", "ShoppingCart", new { area = "Transaction" });
        }
        

        //================刪除===========
        public ActionResult Delete(int id = 0)
        {
            CartsProducts delete = db.CartsProducts.FirstOrDefault(p=>p.ProductID==id);
            db.CartsProducts.Remove(delete);
            db.SaveChanges();
            return RedirectToAction("CartIndex", "ShoppingCart", new { area = "Transaction" });
        }



    }
}



