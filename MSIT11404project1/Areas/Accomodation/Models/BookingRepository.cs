using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSIT11404project1.Models;
namespace MSIT11404project1.Areas.Accomodation.Models
{
    public class BookingRepository:Repository<Booking>,IRepository<Booking> 
    {
        //以房源編號為主   看看房間有沒有訂單 這是為了房東管理
        public bool FindbookbyHouseKey(List<int> hkeys, out List<Booking> hasbooklist) {

            List<List<Booking>> lists = new List<List<Booking>>();
            hasbooklist = new List<Booking>();
            foreach (var keys in hkeys)
            {
                var list = this.GetAll().Where(n => n.HouseKeyID == keys).ToList();
                if (list.Count != 0)
                {

                    lists.Add(list);

                }
            }
            if (lists.Count == 0)
            {
                return false;

            }
            else {

            foreach (var listsamehkey in lists)
            {
                foreach (var items in listsamehkey)
                {
                    hasbooklist.Add(items);
                }

            }

               return true;

            }

            
           
            

        }
        public List<Booking> FindbyhkeyBedspace(int hkeys,int bedspace) {
            var lists = this.GetAll().Where(n => n.HouseKeyID == hkeys && n.BedInSpaceID == bedspace).ToList();
            return lists;
        }

        //用MemberID洗訂房紀錄

        public List<Booking> FindBookByCustomerID(int Memid) {
            var lists = this.GetAll().AsEnumerable().Where(n => n.CustomerID == Memid).ToList();
            List<Booking> reservation = new List<Booking>();
            return lists; 


        }
        
    }
}