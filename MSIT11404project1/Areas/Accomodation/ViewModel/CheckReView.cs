using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSIT11404project1.Areas.Accomodation.Models;
using MSIT11404project1.Models;
namespace MSIT11404project1.Areas.Accomodation.ViewModel
{
    public class CheckReView
    {
        //MemberID傳進來 查看有什麼訂房紀錄
        public CheckReView() { }
        public CheckReView(int MemberID) {
            BookingRepository brepo = new BookingRepository();
            if (brepo.FindBookByCustomerID(MemberID).Count != 0)
            {
                this.BookingForM = brepo.FindBookByCustomerID(MemberID).Where(n => n.EndDate >= DateTime.Now).ToList();
            }
        }
        public List<Booking> BookingForM { get; set; }
    }
}