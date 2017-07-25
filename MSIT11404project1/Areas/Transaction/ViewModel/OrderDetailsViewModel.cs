using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSIT11404project1.Areas.Transaction.ViewModel
{
    public class OrderDetailsViewModel
    {
        //Products資料
        public string ProductName { get; set; }
        public int Quantity{get;set;}
        public int UnitPrice { get; set; }

        //自訂資料
        public int Amount { get; set; }//各產品小計
        public int Total { get; set; }//總金額
    }
}