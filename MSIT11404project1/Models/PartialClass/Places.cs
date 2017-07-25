using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MSIT11404project1.Models
{
    [MetadataType(typeof(PlaceMetaData))]
    public partial class Places
    {
        public class PlaceMetaData {
            [DisplayName("景點編號")]
            public int PlaceID { get; set; }

            [DisplayName("景點地址")]
            public string Address { get; set; }

            [DisplayName("抵達方式")]
            public string ArrivalWay { get; set; }


            [DisplayName("景點描述")]
            public string Description { get; set; }

            [DisplayName("景點圖片")]
            public byte[] Image { get; set; }

            [DisplayName("景點圖片網址")]
            public string ImageWebSite { get; set; }

            [DisplayName("景點名稱")]
            public string Name { get; set; }

            [DisplayName("注意事項")]
            public string Notice { get; set; }

            [DisplayName("聯絡電話")]
            public string Phone { get; set; }

            [DisplayName("參考網站")]
            public string WebSite { get; set; }

            [JsonIgnore]
            public virtual Cities Cities { get; set; }
        }

    }
}