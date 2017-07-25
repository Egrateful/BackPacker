using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSIT11404project1.Models;
namespace MSIT11404project1.Areas.Accomodation.Models
{
    public class CalendarRepository:Repository<Calendar>,IRepository<Calendar>
    {
        //從memberID 找housekeys 找
        //把housekey傳進來 找到此房源的 Calender
        public List<Calendar> FindCalendarByhkey(List<int> hkeys) {


            List<List<Calendar>> lists = new List<List<Calendar>>();
            List<Calendar> lists1 = new List<Calendar>();

            foreach (var keys in hkeys) {
                var list = this.GetAll().Where(n => n.HouseKeyID == keys).ToList();
                if (list.Count != 0) { 
                lists.Add(list);
                }

            }
            if (lists.Count != 0) { 
            foreach (var listsamehkey in lists) {
                foreach (var items in listsamehkey) {
                    lists1.Add(items);
                }

            }
            }
            return lists1;

        }
        //用housekey 和 Bedinspace 找calendars
        public List<Calendar> FindCByHkeyBedspace(int hkey ,int bedinspace)
        {
            var lists = this.GetAll().Where(n => n.HouseKeyID == hkey && n.BedInSpaceID == bedinspace).ToList();

            return lists;
        }

        public bool IsbedinCal(int bedinspace) {
            if (this.GetAll().Where(n => n.BedInSpaceID == bedinspace).ToList().Count() > 0)
            {
                return true;
            }
            else {
                return false;
            }

        }
        
    }
}