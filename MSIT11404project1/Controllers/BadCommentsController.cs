using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MSIT11404project1.Models;
using MSIT11404project1.Areas.Accomodation.Models;
namespace MSIT11404project1.Controllers
{
    public class BadCommentsController : ApiController
    {
        
        IRepository<HouseQuality> abc = new Repository<HouseQuality>();
        public void PostBadComments(HouseQuality qus) {
            HouseQualityRepository hsq = new HouseQualityRepository();
            qus.CreateDate = DateTime.Now;
            hsq.Addbadcomment(qus);
            
        }
        public class checking {
            public bool ischeck { get; set;}
            public int commentid { get; set; }
        }


        public void PutbadComment(checking one) {
            HouseQualityRepository repo = new HouseQualityRepository();
            HouseQuality onew = repo.GetByID(one.commentid);
            onew.IsChecked = one.ischeck;
            repo.Update(onew);
        }
    }
}
