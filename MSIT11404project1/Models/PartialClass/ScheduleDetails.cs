using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MSIT11404project1.Models
{
    [MetadataType(typeof(ScheduleDetailsMeta))]
    public partial class ScheduleDetails
    {
        public class ScheduleDetailsMeta
        {
            public int ScheduleID { get; set; }

            public int TicketInfoID { get; set; }

            public string FlightNo { get; set; }

            [DisplayName("起飛時間")]
            [DataType(DataType.DateTime)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd HH:mm}")]
            public System.DateTime DepartureTime { get; set; }

            [DisplayName("到達時間")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd HH:mm}")]
            public System.DateTime ArrivalTime { get; set; }

        }
    }
}