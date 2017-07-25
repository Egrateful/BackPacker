using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace MSIT11404project1.Models
{
    [MetadataType(typeof(bookings))]
    public partial class Booking
    {
        public class bookings {
            public int BookingID { get; set; }
            public int HouseKeyID { get; set; }
            public int BedInSpaceID { get; set; }

            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            public Nullable<System.DateTime> FromDate { get; set; }
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            public Nullable<System.DateTime> EndDate { get; set; }

            public Nullable<int> CustomerID { get; set; }
            public Nullable<bool> IsConfirm { get; set; }

            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString ="{0:yyyy-MM-dd}")]
            public System.DateTime Createdate { get; set; }

            public virtual BedDtails BedDtails { get; set; }
            public virtual HouseMain HouseMain { get; set; }
            public virtual Members Members { get; set; }

            public Nullable<bool> IsRejected { get; set; }


            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            public Nullable<System.DateTime> ReplyDate { get; set; }
        }
    }
}