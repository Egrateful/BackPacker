using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSIT11404project1.Models;
using MSIT11404project1.Areas.Accomodation.Models;
namespace MSIT11404project1.Areas.Accomodation.ViewModel
{
    //需要Calendar 和 BedDeatails
    public class ManageCalendar
    {
        IRepository<Members> mems = new Repository<Members>();
        BookingRepository bookrepo = new BookingRepository();
        CalendarRepository Crepo = new CalendarRepository();
        BedDetailsRepository bedrepo = new BedDetailsRepository();
        HouseMainRepository hrepo = new HouseMainRepository();
        public ManageCalendar() { }
        public ManageCalendar(int memberid) {

            List<int> hkeys = new List<int>();
            List<BedDtails> forbeds = new List<BedDtails>();
            List<HouseMain> houses = new List<HouseMain>();
            List<Booking> books = new List<Booking>();
            if (hrepo.FindhouseKey(memberid, out hkeys)) {
                foreach (var hh in hkeys) {

                   var haa = hrepo.GetAll().Where(n => n.HouseKey == hh).First();
                    houses.Add(haa);
                }
                this.memhouses = houses;
                if (bedrepo.FindBedsbyHouseKey(hkeys, out forbeds)) {
                    this.bedlists = forbeds;
                    if (Crepo.FindCalendarByhkey(hkeys).Count != 0) {

                        this.callist = Crepo.FindCalendarByhkey(hkeys);
                    }
                    if (bookrepo.FindbookbyHouseKey(hkeys, out books)) {
                        this.booklists = books.OrderBy(n=>n.HouseKeyID).OrderBy(n=>n.BedInSpaceID).OrderBy(n=>n.Createdate).Where(n=>n.EndDate>=DateTime.Now).ToList();
                        

                    }


                }
                
            }



        }
        public List<HouseMain> memhouses { get; set; }
        public List<BedDtails> bedlists { get; set; }
        public List<Calendar> callist { get; set; }
        public List<Booking> booklists{ get; set; }
        
    }
}