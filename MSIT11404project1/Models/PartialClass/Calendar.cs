using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace MSIT11404project1.Models
{
    [MetadataType(typeof(calendar))]
    public partial class Calendar
    {
        public class calendar
        {
            public int HouseKeyID { get; set; }
            public Nullable<int> BedInSpaceID { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-mm-dd}")]
            public Nullable<System.DateTime> FromDate { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-mm-dd}")]
            public Nullable<System.DateTime> EndDate { get; set; }

        }
        
    }
}