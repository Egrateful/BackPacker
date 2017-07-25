using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MSIT11404project1.Models
{
    [MetadataType(typeof(TicketInfoMeta))]
    public partial class TicketInfo
    {
        public class TicketInfoMeta
        {
            [DisplayName("機票編號")]
            public int TicketInfoID { get; set; }

            [DisplayName("航班編號")]
            public int FlightID { get; set; }

            [DisplayName("機票票價")]
            [DisplayFormat(DataFormatString = "{0:c0}")]
            public decimal TicketPrice { get; set; }

            [DisplayName("機票種類")]
            public byte TicketType { get; set; }

            [DisplayName("轉機次數")]
            public byte Stops { get; set; }

        }
    }
}