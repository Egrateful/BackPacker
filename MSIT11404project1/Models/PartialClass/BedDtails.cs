using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace MSIT11404project1.Models
{
    [MetadataType(typeof(beddetails))]
    public partial class BedDtails
    {
        public class beddetails {

            [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString ="{0:c0}")]
            public Nullable<decimal> RoomPrice { get; set; }
        }
    }
}