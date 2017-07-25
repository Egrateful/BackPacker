using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSIT11404project1.Models;

namespace MSIT11404project1.Areas.Accomodation.ViewModel
{
    public class AdminBedView
    {
        public AdminBedView() {
            IRepository<BedType> bhrpo = new Repository<BedType>();
            this.bedentities = bhrpo.GetAll().AsEnumerable();
            IRepository<HouseSourceType> housesoue = new Repository<HouseSourceType>();
            this.housesSource = housesoue.GetAll().AsEnumerable();
            IRepository<HouseSpace> housespace = new Repository<HouseSpace>();
            this.houseSpace = housespace.GetAll().AsEnumerable();
        }
        public IEnumerable<BedType> bedentities { get; set; }
        public IEnumerable<HouseSourceType> housesSource { get; set; }
        public IEnumerable<HouseSpace> houseSpace { get; set; } 
        public BedType bedentity { get; set; }
        public HouseSpace houseSpaceentity { get; set; }
        public HouseSourceType houseSourcesE { get; set; }
    }
}