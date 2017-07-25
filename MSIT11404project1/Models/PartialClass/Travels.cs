using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MSIT11404project1.Models
{
    [MetadataType(typeof(TravelMetaData))]
    public partial class Travels
    {
        public partial class TravelMetaData
        {
            [DisplayName("行程編號")]
            public int TravelID { get; set; }
            [DisplayName("行程名稱")]
            public string Name { get; set; }

            [DisplayName("開始時間")]
            //[DataType(DataType.Date)]
            //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-mm-dd}")]
            public System.DateTime StartDate { get; set; }

            [DisplayName("結束時間")]
            //[DataType(DataType.Date)]
            //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-mm-dd}")]
            public System.DateTime EndDate { get; set; }

            [DisplayName("經過時間")]
            public int DuringDay { get; set; }

            [DisplayName("行程簡介")]
            public string Description { get; set; }

            [AllowHtml]
            public string Detail { get; set; }

            [DisplayName("會員編號")]
            public int MemberID { get; set; }



            public virtual Members Members { get; set; }

        }

    }

}