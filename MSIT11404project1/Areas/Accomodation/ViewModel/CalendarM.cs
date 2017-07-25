using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MSIT11404project1.Models;
using MSIT11404project1.Areas.Accomodation.Models;
namespace MSIT11404project1.Areas.Accomodation.ViewModel
{
    public class CalendarM
    {
        MSIT11404Entities db = new MSIT11404Entities();
        IRepository<HouseMain> housemainsss = new Repository<HouseMain>();
        public CalendarM() { }
        public CalendarM(int memberID)
        {
            HouseMainRepository hrepo = new HouseMainRepository();
            CalendarRepository Crepo = new CalendarRepository();
            BedDetailsRepository Bedsrepo = new BedDetailsRepository();
            List<BedDtails> beds = new List<BedDtails>();

            this.MemberID = Convert.ToInt32(memberID);
            
            List<List<Calendar>> listsc = new List<List<Calendar>>();

            List<List<Booking>> booklist = new List<List<Booking>>();

            List<List<BedDtails>> beddetails = new List<List<BedDtails>>();

            List<HouseMain> listforhouseMain = new List<HouseMain>();
           

            HouseMain housemain = new HouseMain();
            List<int> housekeyss = new List<int>();

            if (hrepo.FindhouseKey(this.MemberID, out housekeyss)) {
                

                this.houseKeys = housekeyss;
                for (int i = 0; i < housekeyss.Count; i++)
                {
                    listforhouseMain.Add(housemainsss.GetByID(housekeyss[i]));

                    var listforbook = db.Booking.AsEnumerable().Where(n => n.HouseKeyID == housekeyss[i]).ToList();
                    booklist.Add(listforbook);

                    var listforbeds = db.BedDtails.AsEnumerable().Where(n => n.HouseKey ==housekeyss[i]).ToList();
                    beddetails.Add(listforbeds);
                    var listforcalendar = db.Calendar.AsEnumerable().Where(n => n.HouseKeyID == housekeyss[i]).Select(n=>n).ToList();
                    listsc.Add(listforcalendar);

                }

                this.calendaranother = Crepo.FindCalendarByhkey(housekeyss);//Memberid=>housekeys=>Calenders

                if (Bedsrepo.FindBedsbyHouseKey(housekeyss, out beds)) {
                    this.bedsdetails = beds;
                }


                this.beddetailforC = beddetails.AsQueryable();
                this.calendars = listsc.AsQueryable();
                this.housemains = listforhouseMain.AsQueryable();
                this.bookings = booklist.AsQueryable();
           }
          

            
        }
        
        public IEnumerable<Calendar> calendaranother { get; set; }
        public IEnumerable<BedDtails> bedsdetails { get; set; }

        public IQueryable<List<Calendar>> calendars { get; set; } 
        public IQueryable<List<BedDtails>> beddetailforC { get; set; }
        public IEnumerable<int> houseKeys { get; set; }

        public IQueryable<HouseMain> housemains { get; set; }

        public IQueryable<List<Booking>> bookings { get; set; }

        public int MemberID { get; set; }
        public int HouseKey { get; set; }

        

        

        [DisplayName("空間名稱")]
        public string SpaceIntro { get; set; }

        
      
    
      



        
        
       
        public int BedInSpaceID { get; set; }


    }
}