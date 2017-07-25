using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MSIT11404project1.Models;

namespace MSIT11404project1.Areas.FlightSearch.ViewModel
{
    public class SearchFlightViewModel
    {
   
        [Required(ErrorMessage = "必要輸入出發地")]
        public string FromAirportCity { get; set; }

        [Required(ErrorMessage = "必要輸入目的地")]
        public string ToAirportCity { get; set; }

        [Required(ErrorMessage = "必要輸入出發日期")]
        public System.DateTime DepartureDate { get; set; }

        public Nullable<System.DateTime> ReturnDate { get; set; }

        public int? passengerNumber { get; set; }

    }
}