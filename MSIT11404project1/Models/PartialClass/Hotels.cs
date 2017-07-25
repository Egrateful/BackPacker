using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MSIT11404project1.Models
{
    [MetadataType(typeof(HotelMetaData))]
    public partial class Hotels
    {
        public partial class HotelMetaData
        {
            [DisplayName("住宿編號")]
            public int HotelID { get; set; }

            [DisplayName("住宿地址")]
            public string Address { get; set; }

            [DisplayName("抵達方式")]
            public string ArrivalWay { get; set; }

            [DisplayName("平均價位")]
            public Nullable<decimal> AvgPrice { get; set; }

            

            [DisplayName("住宿描述")]
            public string Description { get; set; }

            [DisplayName("住宿圖片")]
            public byte[] Image { get; set; }

            [DisplayName("住宿圖片網址")]
            public string ImageWebSite { get; set; }

            [DisplayName("住宿名稱")]
            public string Name { get; set; }

            [DisplayName("連絡電話")]
            public string Phone { get; set; }

            [DisplayName("服務資訊")]
            public string ServiceInfo { get; set; }

            [DisplayName("參考網站")]
            public string WebSite { get; set; }

            [JsonIgnore]
            public virtual Cities Cities { get; set; }
        }
    }
}