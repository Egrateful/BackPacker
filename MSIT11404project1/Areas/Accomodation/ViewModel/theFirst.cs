using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSIT11404project1.Models;
using MSIT11404project1.Areas.Accomodation.Models;
namespace MSIT11404project1.Areas.Accomodation.ViewModel
{
    public class theFirst
    {
        public theFirst() { }
        public theFirst(int MemberID) {
            IRepository<Members> memrpo = new Repository<Members>();
            HouseMainRepository hrepo = new HouseMainRepository();
            List<HouseMain> houss = new List<HouseMain>();
            this.Mems = memrpo.GetByID(MemberID);

            this.housesEnties = hrepo.GetAll().AsEnumerable().Where(n => n.MemberID == MemberID).ToList();




        }
        public Members Mems { get; set; }
        public List<HouseMain> housesEnties { get; set; }
    }
}