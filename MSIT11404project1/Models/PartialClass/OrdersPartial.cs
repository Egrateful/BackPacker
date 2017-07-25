using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MSIT11404project1.Models
{

    [MetadataType(typeof(OrdersPartial))]
    public partial class Orders
    {

  public class OrdersPartial
        {
            [DisplayName("訂單編號")]
            public int OrderID { get; set; }
            [DisplayName("購買人")]
            public int MemberID { get; set; }
            [DisplayName("收件人")]
            public string Purchaser { get; set; }
            [DisplayName("電話")]
            public string PurchaserPhon { get; set; }
            [DisplayName("地址")]
            public string PurchaserAddress { get; set; }
            [DisplayName("購買日期")]
            [DataType(DataType.Date)]
            public Nullable<System.DateTime> OrderDate { get; set; }

        }
    }
}