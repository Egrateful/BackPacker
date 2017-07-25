using MSIT11404project1.Models;
using PagedList;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MSIT11404project1.Areas.Transaction.Controllers
{
    public class ProductsController : Controller
    {
        private MSIT11404Entities db = new MSIT11404Entities();
        // GET: Transaction/THome
        //=====MenuLink=====
        public ActionResult MenuLink(int id = 0)
        {


            if (Request.Cookies["MemberID"] != null)
            {
                id = Convert.ToInt32(Request.Cookies["MemberID"].Value);

                CartsProducts _carts = db.CartsProducts.Find(id);
                ViewBag.Sellcount = db.Products.Where(p => p.MemberID == id).Count();
                ViewBag.Buycount = db.CartsProducts.Where(c => c.MemberID == id).Count();
            }
            else
            {
                ViewBag.Sellcount = "0";

                if (db.CartsProducts.Where(c => c.MemberID == id).Count() == 0)
                {
                    ViewBag.Buycount = "0";
                }
                else
                { ViewBag.Buycount = db.CartsProducts.Where(c => c.MemberID == id).Count(); }
            }
            return View();
        }

        //=====ListLink=====
        public ActionResult ListLink(int id = 0)
        {
            ViewBag.m = Convert.ToInt32(Request.Form["product_ProductID"]);
            if (Request.Cookies["MemberID"] != null)
            {
                id = Convert.ToInt32(Request.Cookies["MemberID"].Value);

                CartsProducts _carts = db.CartsProducts.Find(id);
                ViewBag.Sellcount = db.Products.Where(p => p.MemberID == id).Count();
                ViewBag.Buycount = db.CartsProducts.Where(c => c.MemberID == id).Count();
                ViewBag.Order = db.Orders.Where(c => c.MemberID == id).Count();
            }
            else
            {
                ViewBag.Sellcount = "0";

                if (db.CartsProducts.Where(c => c.MemberID == id).Count() == 0)
                {
                    ViewBag.Buycount = "0";
                }
                else
                { ViewBag.Buycount = db.CartsProducts.Where(c => c.MemberID == id).Count(); }
            }
            return View();
        }




        //========首頁排版=================
        public ActionResult Index(int? page, string sortBy, int id = 0, int Mid = 0)
        {
            var prodicts = db.Products.AsQueryable();


            if (Request.Cookies["MemberID"] != null)
            {
                Mid = Convert.ToInt32(Request.Cookies["MemberID"].Value);
            }

            if (id != 0)
            {
                ViewBag.Products = db.Categorys.FirstOrDefault(c => c.CategoryID == id).CategoryName;
                prodicts = db.Products.Where(p => p.CategoryID == id && p.MemberID != Mid && p.Quantity != 0).AsQueryable();

            }
            else
            {
                ViewBag.Products = "全部商品";
                prodicts = db.Products.Where(p => p.MemberID != Mid && p.Quantity != 0).AsQueryable();

            }


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

        //======加入購物車======
        [HttpPost]
        public ActionResult Add(CartsProducts _cartsproducts, int id = 0, int Quantity = 1)
        {

            if (Request.Cookies["MemberID"] != null)
            {
                Products _product = db.Products.Find(id);


                _cartsproducts.MemberID = Convert.ToInt32(Request.Cookies["MemberID"].Value);



                //抓產品筆數 如果小於or等於0才會加入購物車
                int CountItems = db.CartsProducts.Where(p => p.ProductID == id).Count();

                if (CountItems <= 0)
                {
                    _cartsproducts.ProductID = _product.ProductID;
                
                    //   _cartsproducts.Quantity = Convert.ToInt32(Request.Form["Quantity"]);
                    _cartsproducts.Quantity = Quantity;
                    _cartsproducts.Subtotal = _product.UnitPrice * Quantity;


                    db.CartsProducts.Add(_cartsproducts);

                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Products", new { area = "Transaction" });
            }
            return RedirectToAction("Login", "Member", new { area = "Member" });
        }




        //=====類別DropDown=======
        public ActionResult Categorys(int id = 0)
        {

            ViewBag.item = db.Products.Where(p => p.CategoryID == id).Count();
            return PartialView(db.Categorys.ToList());
        }

        //===========讀取圖片==============
        public ActionResult GetImage(int id = 0)
        {

            Products product = db.Products.Find(id);
            byte[] img = product.ByteImge;

            return File(img, "image/jpeg");
        }
        //===========新增產品==============
        [HttpGet]
        public ActionResult Create()
        {

            if (Request.Cookies["MemberID"] != null)
            {
                ViewBag.Categorys = db.Categorys.ToList();
                return View();
            }
            return RedirectToAction("Login", "Member", new { area = "Member" });

        }
        [HttpPost]
        public ActionResult Create(Products _product, HttpPostedFileBase ProductImage)
        {

            if (this.ModelState.IsValid)
            {
                if (ProductImage != null)
                {
                    //檔案上傳 <input type="file"
                    //要設定form enctype="multipart/form-data"
                    //在action上用HttpPostedFileBase的型別來接收資料
                    //將檔案存放在server資料夾裡面
                    //要取得實際路徑
                    //Request.PhysicalApplicationPath
                    //string str = Request.PhysicalApplicationPath + @"ProductImages\" + ProductImage.FileName;
                    //Server.MapPath(".")
                    //ProductImage.SaveAs(str);

                    //將檔案轉成二進位寫進資料庫
                    var imgSize = ProductImage.ContentLength;//檔案大小
                    byte[] imgbyte = new byte[imgSize];//轉byte
                    ProductImage.InputStream.Read(imgbyte, 0, imgSize);//InputStream讀取
                    _product.ProductImage = ProductImage.FileName;
                    _product.ByteImge = imgbyte;

                    _product.Date = DateTime.Now;
                    db.Products.Add(_product);
                    db.SaveChanges();
                    return RedirectToAction("ProductsList", "Products", new { area = "Transaction" });
                }
            }
            ViewBag.Erro = "請選擇圖片";
            ViewBag.Categorys = db.Categorys.ToList();
            return View();

        }

        //==============修改============
        [HttpGet]
        public ActionResult Update(int id = 0)
        {
            if (Request.Cookies["MemberID"] != null)
            {
                ViewBag.Categorys = db.Categorys.ToList();

                Products _product = db.Products.Find(id);

                return View(_product);
            }

            return RedirectToAction("Login", "Member", new { area = "Member" });
        }
        [HttpPost]
        public ActionResult Update(Products _product, HttpPostedFileBase ProductImage)
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
                    return RedirectToAction("ProductsList", "Products", new { area = "Transaction" });
                }
            }
            ViewBag.Categorys = db.Categorys.ToList();
            db.Entry(_product).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ProductsList", "Products", new { area = "Transaction" });
        }

        //================刪除===========
        public ActionResult Delete(int id = 0)
        {
            if (db.OrderDetails.Where(p => p.ProductID == id).Count() == 0)
            {
                Products delete = db.Products.Find(id);
                db.Products.Remove(delete);
                db.SaveChanges();
                return RedirectToAction("ProductsList", "Products", new { area = "Transaction" });
            }
            return RedirectToAction("ProductsList", "Products", new { area = "Transaction" });
        }

        //======產品詳細資料======


        [HttpGet]
        public ActionResult ProductsDetailed(int id = 0)
        {
            ViewBag.Categories = db.Categorys.ToList();
            Products _product = db.Products.Find(id);
            return View(_product);
        }
        //public ActionResult ProductsDetailed(Products _product, HttpPostedFileBase ProductImage)
        //{

        //    ViewBag.Categories = db.Categorys.ToList();
        //    Products p = new Products();
        //    if (p.ProductImage != _product.ProductImage)
        //    {
        //        var imgSize = ProductImage.ContentLength;//檔案大小
        //        byte[] imgbyte = new byte[imgSize];//轉byte
        //        ProductImage.InputStream.Read(imgbyte, 0, imgSize);//InputStream讀取
        //        _product.ProductImage = ProductImage.FileName;
        //        _product.ByteImge = imgbyte;


        //        db.Entry(_product).State = System.Data.Entity.EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("ProductsList", "Products", new { area = "Transaction" });
        //    }

        //    db.Entry(_product).State = System.Data.Entity.EntityState.Modified;
        //    db.SaveChanges();
        //    return RedirectToAction("ProductsList", "Products", new { area = "Transaction" });
        //}

        //====拍賣清單=====
        public ActionResult ProductsList(int? page, string sortBy, int id = 0, int Mid = 0)
        {
            if (Request.Cookies["MemberID"] != null)
            {


 var prodictslist = db.Products.AsQueryable();

               
               Mid = Convert.ToInt32(Request.Cookies["MemberID"].Value);
                ViewBag.SortDate = string.IsNullOrEmpty(sortBy) ? "Date desc" : "";
               
                //過濾是某會員的資料
                prodictslist = db.Products.Where(p => p.MemberID == Mid).AsQueryable();
                switch (sortBy)
                {
                    case "Date desc":
                        prodictslist = prodictslist.OrderByDescending(p => p.Date);
                        break;

                    default:
                        prodictslist = prodictslist.OrderBy(p => p.Date);
                        break;
                }
                return View(prodictslist.ToList().ToPagedList(page ?? 1, 10));
            }
            return RedirectToAction("Login", "Member", new { area = "Member" });
        }


        //======產品清單詳細資料======


        [HttpGet]
        public ActionResult ProductsListDetailed(int id = 0)
        {
            ViewBag.Categories = db.Categorys.ToList();
            Products _product = db.Products.Find(id);

            return View(_product);
        }
        //public ActionResult ProductsListDetailed(Products _product, HttpPostedFileBase ProductImage)
        //{

        //    ViewBag.Categories = db.Categorys.ToList();
        //    Products p = new Products();
        //    if (p.ProductImage != _product.ProductImage)
        //    {
        //        var imgSize = ProductImage.ContentLength;//檔案大小
        //        byte[] imgbyte = new byte[imgSize];//轉byte
        //        ProductImage.InputStream.Read(imgbyte, 0, imgSize);//InputStream讀取
        //        _product.ProductImage = ProductImage.FileName;
        //        _product.ByteImge = imgbyte;


        //        db.Entry(_product).State = System.Data.Entity.EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("ProductsList", "Products", new { area = "Transaction" });
        //    }

        //    db.Entry(_product).State = System.Data.Entity.EntityState.Modified;
        //    db.SaveChanges();
        //    return RedirectToAction("ProductsList", "Products", new { area = "Transaction" });
        //}

        //======加入購物車==========







        //public ActionResult AddOrder( int id = 0,int Quantity=1)
        //{



        //    Products _product = db.Products.Find(id);
        //CartsProducts _cartsproducts = new CartsProducts();

        //    if (db.CartsProducts.Count().ToString() == null)
        //    {
        //        _cartsproducts.ProductID = _product.ProductID;
        //        _cartsproducts.Quantity = Convert.ToInt32(Request.Form["Quantity_" + id]);
        //        _cartsproducts.Subtotal = _product.UnitPrice * _cartsproducts.Quantity;

        //        db.CartsProducts.Add(_cartsproducts);
        //        db.SaveChanges();

        //    }

        //    else
        //    {
        //              if (Request.Cookies["name"] != null)
        //            {
        //                _cartsproducts.MemberID = Convert.ToInt32(Request.Cookies["name"].Value);
        //            }

        //            _cartsproducts.ProductID = _product.ProductID;
        //            _cartsproducts.Quantity = Convert.ToInt32(Request.Form["Quantity_" + id]);
        //            _cartsproducts.Subtotal = _product.UnitPrice * _cartsproducts.Quantity;

        //            db.CartsProducts.Add(_cartsproducts);
        //            db.SaveChanges();

        //            return RedirectToAction("Index", "Products", new { area = "Transaction" });

        //    }
        //    return RedirectToAction("Index", "Products", new { area = "Transaction" });
        //}

    }
}