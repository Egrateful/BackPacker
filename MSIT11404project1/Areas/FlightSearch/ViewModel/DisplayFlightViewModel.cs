using MSIT11404project1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace MSIT11404project1.Areas.FlightSearch.ViewModel
{
     public class DisplayFlightViewModel
    {


        public IEnumerable<Models.Airlines> Airlines { get; set; }
        public IEnumerable<Models.Flights> Flights { get; set; }
        public IEnumerable<Models.TicketInfo> Tickets { get; set; }
        public IEnumerable<Models.ScheduleDetails> Schedules { get; set; }

        //[DisplayName("航班編號")]
        //public int FlightID { get; set; }

        //[DisplayName("航空公司")]
        //public string AirlineCName { get; set; }

        //public bool IsSelected { get; set; }
        //public int TicketInfoID { get; set; }

        //public string FromAirport { get; set; }

        //public string FromAirportName { get; set; }

        //[DisplayName("出發地")]
        //public string FromAirportCity { get; set; }
        //public string FromAirportCountry { get; set; }
        //public string ToAirport { get; set; }
        //public string ToAirportName { get; set; }

        //[DisplayName("目的地")]
        //public string ToAirportCity { get; set; }
        //public string ToAirportCountry { get; set; }


        //[DisplayName("出發日期")]
        //public System.DateTime DepartureDate { get; set; }

        //[DisplayName("回程日期")]
        //public System.DateTime ReturnDate { get; set; }

        //[DisplayFormat(DataFormatString = "{0:c0}")]
        //public decimal TicketPrice { get; set; }

    }
}