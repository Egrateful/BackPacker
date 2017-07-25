using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MSIT11404project1.Models
{

    [MetadataType(typeof(CartsProductsPartial))]
    public partial class CartsProducts
    {
        public class CartsProductsPartial
        {
            [DisplayName("商品名稱")]
            public Nullable<int> ProductID { get; set; }
                        
            [DisplayName("購買量")]
            public Nullable<int> Quantity { get; set; }
            [DisplayFormat(DataFormatString = "{0:c0}")]
            [DisplayName("小計")]
            public Nullable<decimal> Subtotal { get; set; }
         
        }
    }
}