using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MSIT11404project1.Models
{

    [MetadataType(typeof(OrderDetailsPartial))]
    public partial class OrderDetails
    {
        public class OrderDetailsPartial
        {

[DisplayName("訂單單號")]
            public int OrderID { get; set; }
            [DisplayName("產品名稱")]
            public int ProductID { get; set; }
            [DisplayName("購買數量")]
            public int Quantity { get; set; }
            [DisplayName("小計")]
            [DisplayFormat(DataFormatString = "{0:c0}")]
            public decimal UnitPrice { get; set; }


        }
    }
}