using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MSIT11404project1.Models
{
    [MetadataType(typeof(RestaurantsMetaData))]
    public partial class Restaurants
    {
        public class RestaurantsMetaData
        {
            [DisplayName("餐廳編號")]
            public int RestaurantID { get; set; }

            [DisplayName("餐廳地址")]
            public string Address { get; set; }

            [DisplayName("抵達方式")]
            public string ArrivalWay { get; set; }

            [DisplayName("平均價位")]
            public Nullable<decimal> AvgPrice { get; set; }

            //[DisplayName("所在城市")]
            //public string City { get; set; }

            [DisplayName("餐廳描述")]
            public string Description { get; set; }

            [DisplayName("餐廳圖片")]
            public byte[] Image { get; set; }

            [DisplayName("餐廳圖片網址")]
            public string ImageWebSite { get; set; }

            [DisplayName("餐廳名稱")]
            public string Name { get; set; }

            [DisplayName("營業時間")]
            public string OpenTime { get; set; }

            [DisplayName("連絡電話")]
            public string Phone { get; set; }

            [DisplayName("參考網站")]
            public string WebSite { get; set; }

            [JsonIgnore]
            public virtual Cities Cities { get; set; }
        }
    }
}