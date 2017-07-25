using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MSIT11404project1.Models
{
    [MetadataType(typeof(ProductsPartial))]
    public partial class Products
    {

        public  class ProductsPartial
        {

        [Required(ErrorMessage = "未選擇產品類別")]
        [DisplayName("產品類別")]
            public int CategoryID { get; set; }
            [Required(ErrorMessage = "沒填寫產品名稱")]
            [DisplayName("產品名稱")]
            public string ProductName { get; set; }
            [DisplayName("拍賣人")]
            public Nullable<int> MemberID { get; set; }
            [Required(ErrorMessage ="未填寫產品規格")]
            [DataType(DataType.MultilineText)]
            [DisplayName("產品規格")]
            public string ProductContent { get; set; }
         
            [DisplayName("產品圖片")]
            public string ProductImage { get; set; }
            [Required(ErrorMessage ="未填寫數量")]
            [RegularExpression("^(0|[1-9][0-9]*)$", ErrorMessage ="請輸入正確的數量(不可小於0)")]
            [DisplayName("可購買數量")]
            public int Quantity { get; set; }
            [Required(ErrorMessage = "未填寫價位")]
            [RegularExpression("^(0|[1-9][0-9]*)$", ErrorMessage = "請輸入正確的價位(不可小於0)")]
            [DisplayFormat(DataFormatString = "{0:c0}")]
            [DisplayName("單價")]
            public decimal UnitPrice { get; set; }
            [DataType(DataType.DateTime)]
            [DisplayFormat(DataFormatString ="{0:yyyy/MM/dd}")]
            [DisplayName("建立日期")]
            public Nullable<System.DateTime> Date { get; set; }


        }
    }
}