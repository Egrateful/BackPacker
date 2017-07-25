using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MSIT11404project1.Models
{
    [MetadataType(typeof(FlightsMeta))]
    public partial class Flights
    {
        public class FlightsMeta
        {
            [DisplayName("航班編號")]
            public int FlightID { get; set; }

            [DisplayName("航空公司編號")]
            public int AirlineID { get; set; }

            [DisplayName("機票編號")]
            public int TicketInfoID { get; set; }

            [DisplayName("起飛機場代號")]
            public string FromAirport { get; set; }

            [DisplayName("起飛機場")]
            public string FromAirportName { get; set; }

            [DisplayName("出發城市")]
            public string FromAirportCity { get; set; }

            [DisplayName("出發國家")]
            public string FromAirportCountry { get; set; }

            [DisplayName("目的機場代號")]
            public string ToAirport { get; set; }

            [DisplayName("目的地機場")]
            public string ToAirportName { get; set; }

            [DisplayName("目的地城市")]
            public string ToAirportCity { get; set; }

            [DisplayName("目的國家")]
            public string ToAirportCountry { get; set; }

            [DisplayName("出發日期")]
            [DataType(DataType.DateTime)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            public System.DateTime DepartureDate { get; set; }

            [DisplayName("回程日期")]
            [DataType(DataType.DateTime)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            public Nullable<System.DateTime> ReturnDate { get; set; }

            [DisplayName("行程編號")]
            public int ScheduleID { get; set; }
        }
    }
}