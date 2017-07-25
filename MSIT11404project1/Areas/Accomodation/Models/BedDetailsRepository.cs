using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSIT11404project1.Models;
namespace MSIT11404project1.Areas.Accomodation.Models
{
    public class BedDetailsRepository:Repository<BedDtails>,IRepository<BedDtails>
    {
        public bool FindBedsbyHouseKey(List<int> hkeys, out List<BedDtails> hasbooklist)
        {
            List<List<BedDtails>> lists = new List<List<BedDtails>>();
            hasbooklist = new List<BedDtails>();
            foreach (var keys in hkeys)
            {
                var list = this.GetAll().Where(n => n.HouseKey == keys).ToList();
                if (list != null)
                {

                    lists.Add(list);

                }
                else
                {
                    return false;
                }

            }

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
}