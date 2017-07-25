using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSIT11404project1.Models;
using MSIT11404project1.Areas.Transaction.ViewModel;

namespace MSIT11404project1.Areas.Transaction.Controllers
{
    public class OrderController : Controller
    {
        MSIT11404Entities db = new MSIT11404Entities();
        // GET: Transaction/Order
        [HttpGet]
        public ActionResult OrderIndex( int id = 0)
        {
            ViewBag.length = db.CartsProducts.Where(p => p.MemberID == id).Count();
            OrderAdd OA = new OrderAdd();
            var q = db.CartsProducts.Sum(p => p.Subtotal);
            ViewBag.all = string.Format("{0:c0}", q);
            OA.CartsProducts = db.CartsProducts.Where(p=>p.MemberID==id);
            
            return View(OA);
        }
        [HttpPost]
           public ActionResult OrderIndex(OrderAdd OA, int id = 0)
        {


            if (this.ModelState.IsValid)
            {
                Orders _order = new Orders();
                _order.Purchaser = OA.Purchaser;
                _order.PurchaserPhon = OA.PurchaserPhon;
                _order.PurchaserAddress = OA.PurchaserAddress;
                _order.MemberID = Convert.ToInt32(Request.Cookies["MemberID"].Value);
                _order.OrderDate = DateTime.Now;
                _order.Total = db.CartsProducts.Sum(s => s.Subtotal);
                _order.Service = false;
                db.Orders.Add(_order);





                foreach (var cc in db.CartsProducts.Where(p => p.MemberID == id).ToList())
                {

                    OrderDetails _orderdetails = new OrderDetails();
                    _orderdetails.OrderID = _order.OrderID;
                    _orderdetails.ProductID = cc.ProductID;
                   _orderdetails.Quantity = cc.Quantity;
                    _orderdetails.UnitPrice = cc.Subtotal;
                    Products _products = db.Products.Find(cc.ProductID);
                    _products.ProductID = _products.ProductID;
                    _products.CategoryID = _products.CategoryID;
                    _products.MemberID = _products.MemberID;
                    _products.ProductName = _products.ProductName;
                    _products.ProductImage = _products.ProductImage;
                    _products.ByteImge = _products.ByteImge;
                    _products.ProductContent = _products.ProductContent;
                    _products.UnitPrice = Convert.ToInt32(string.Format("{0:f0}", _products.UnitPrice));
                    _products.Date = _products.Date;


                    _products.Quantity = _products.Quantity - cc.Quantity;

                    db.OrderDetails.Add(_orderdetails);
                    db.Entry(_products).State = System.Data.Entity.EntityState.Modified;

                    db.CartsProducts.Remove(cc);
                }




                db.SaveChanges();

                return RedirectToAction("Index", "Products", new { area = "Transaction" });
            }

            OA.CartsProducts = db.CartsProducts.Where(p => p.MemberID == id);

            return View(OA);

        }
        public ActionResult OrderDetailed(int id = 0)
        {
            ViewBag.Categories = db.Categorys.ToList();
            Products _product = db.Products.Find(id);

            return View(_product);
        }

     
    }
}