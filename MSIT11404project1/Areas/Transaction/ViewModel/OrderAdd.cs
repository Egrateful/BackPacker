using MSIT11404project1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MSIT11404project1.Areas.Transaction.ViewModel
{
    public class OrderAdd
    {
        [Required(ErrorMessage = "未選填寫購買人")]
        [DisplayName("購買人")]
        public string Purchaser { get; set; }
        [Required(ErrorMessage = "未選填寫電話")]
        [DisplayName("電話")]
        public string PurchaserPhon { get; set; }
        [Required(ErrorMessage = "未選填寫地址")]
        [DisplayName("地址")]
        public string PurchaserAddress { get; set; }
    
         public IEnumerable<CartsProducts> CartsProducts { get; set; }
        //public IEnumerable<Products> Products { get; set; }

    }
}