using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MSIT11404project1.Models;
namespace MSIT11404project1.Controllers
{
    public class BasicWebapiController : ApiController
    {
        public void Putbeds(BedType bbs) {
            IRepository<BedType> beds = new Repository<BedType>();

            beds.Update( beds.GetByID(bbs.BedTypeID));

        }
        [Route(template:"api/houseSpace/put",Name ="Puths")]
        public void Puths(HouseSpace one) {
            IRepository<HouseSpace> hs = new Repository<HouseSpace>();
            hs.Update(hs.GetByID(one.HouseSpaceID));

        }


        [Route(template:"api/houseSource/put",Name ="PutHouse")]
        public void Puthsource(HouseSourceType source) {
            IRepository<HouseSourceType> hs = new Repository<HouseSourceType>();
            hs.Update(hs.GetByID(source.HouseSourceID));
        }
    }
}
