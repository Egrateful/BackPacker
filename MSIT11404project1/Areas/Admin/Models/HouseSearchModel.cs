using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSIT11404project1.Areas.Admin.Models
{
    public class HouseSearchModel
    {
        public int MemberID { get; set; }
        public List<int> housesoutce { get; set; }
        public int housetype { get; set; }
        public string Country { get; set; }
        public string States { get; set; }
        
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Isfreeze { get; set; }
        public int IsActive { get; set; }
    }
}