using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MSIT11404project1.Models
{
    public enum Airline_Name
    {
        長榮航空,
        中華航空,
        中國國際航空,
        新加坡航空,
        聯合航空,
        加拿大航空,
        日本航空,
        國泰航空,
        德國漢莎航空,
        大韓航空,
        海南航空,
        中國東方航空,
        中國南方航空,
        美國航空,
        荷蘭皇家航空,
        阿聯酋航空,
        菲律賓航空

    }
    [MetadataType(typeof(AirlinesMeta))]
    public partial class Airlines
    {
        public class AirlinesMeta
        {
            public int AirlineID { get; set; }


            [DisplayName("航空公司")]
            public string AirlineCName { get; set; }

            [DisplayName("航空公司英文名稱")]
            public string AirlineName { get; set; }

            public string CallSign { get; set; }

            [DisplayName("航空公司國名")]
            public string Country { get; set; }

            public Nullable<bool> IsSelected { get; set; }

        }
    }
}