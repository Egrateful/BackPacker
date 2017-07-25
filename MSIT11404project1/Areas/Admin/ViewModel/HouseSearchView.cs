using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using MSIT11404project1.Models;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MSIT11404project1.Areas.Admin.Models;

namespace MSIT11404project1.Areas.Admin.ViewModel
{
    public class HouseSearchView
    {
        public HouseSearchView() {
            
            IRepository<HouseSourceType> sourcerepo = new Repository<HouseSourceType>();
            IRepository<HouseSpace> spacerepo = new Repository<HouseSpace>();
            this.housetypes = sourcerepo.GetAll().ToList();
            this.housespaces = spacerepo.GetAll().ToList();
            
        }

        public HouseSearchModel  housesearchm { get; set; }
        public IPagedList<HouseMain> houseEntities { get; set; }
        public List<HouseSourceType> housetypes { get; set; }
        public List<HouseSpace> housespaces { get; set; }
        public int PageIndex { get; set; }

    }
}