using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

using MSIT11404project1.Models;
namespace MSIT11404project1.Areas.ViewModel
{
    
    public class InsertHouse

    {
        MSIT11404Entities db = new MSIT11404Entities();
        
        IRepository<HouseMain> houses = new Repository<HouseMain>();
        IRepository<ImageForRoom> images = new Repository<ImageForRoom>();

        public InsertHouse() { }


        public InsertHouse(int? houseKey)
        {
            if (houseKey != null) { 
            int Housekkey =   Convert.ToInt32(houseKey);
                this.EntityHouseMain = houses.GetByID(Housekkey);
                var listforimage = images.GetAll().Where(n => n.HouseKey == Housekkey);
                this.EntityImages = listforimage;
                

            }
        }
        public HouseMain EntityHouseMain { get; set; }
     
        


        [DisplayName("照片集")]

        
        public IEnumerable<HttpPostedFileBase> Images { get; set; }//不太確定這是幹嘛的
        public IQueryable<ImageForRoom> EntityImages { get; set; }

        public IQueryable<HouseMain> EntitiesHouses { get; set; }

        
       

        
    }
}