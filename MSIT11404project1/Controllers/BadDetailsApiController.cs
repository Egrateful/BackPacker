using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MSIT11404project1.Areas.Accomodation.Models;
using MSIT11404project1.Models;
namespace MSIT11404project1.Controllers
{
    public class BadDetailsApiController : ApiController
    {
       HouseMainRepository hrop = new HouseMainRepository();
       BedDetailsRepository rpo = new BedDetailsRepository();
        public void Postbeds(BedDtails entity) {

            int hk = Convert.ToInt32(entity.HouseKey);

            HouseMain aa=hrop.GetByID(hk);
            if (aa.BedAvailableNum == null) {
                aa.BedAvailableNum = 0;
            }
            aa.BedAvailableNum += 1;
            hrop.Update(hrop.GetByID(hk));
            rpo.Create(entity);

        }
        public void Delete(int id) {
            int house = Convert.ToInt32(rpo.GetByID(id).HouseKey);
            hrop.GetByID(house).BedAvailableNum -= 1;
            hrop.Update(hrop.GetByID(house));
            rpo.Delete(rpo.GetByID(id));
        }
    }
}
