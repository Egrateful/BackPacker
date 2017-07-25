using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSIT11404project1.Models;
using MSIT11404project1.Areas.Accomodation.Models;
namespace MSIT11404project1.Areas.Accomodation.ViewModel
{
    
    //Image HouseMain Beddetails comments
    public class housedetailsview
    {
        
        HouseMainRepository housemain = new HouseMainRepository();
        IRepository <ImageForRoom> imageforroom = new Repository<ImageForRoom>();
        BedDetailsRepository beddetails = new BedDetailsRepository();//for 床型
        HouseQualityRepository housequality = new HouseQualityRepository();
        IRepository<Members> mems = new Repository<Members>();
        
        //一個housekey 的照片list
        //以housekey為主的各個房間的介紹和床型
        //這個房源的相關評論,而且可以新增評論
        public housedetailsview() { }
        public housedetailsview(int housekey)
        {
            
            this.imagelist = imageforroom.GetAll().AsEnumerable().Where(n => n.HouseKey == housekey).ToList();
            this.housemainentity = housemain.GetByID(housekey);

            this.housecriticles = housequality.GetAll().AsEnumerable().Where(n => n.HouseKey == housekey).ToList();

            this.bedsintros = beddetails.GetAll().AsEnumerable().Where(n => n.HouseKey == housekey).OrderBy(n=>n.BedInSpaceID).ToList();

            this.mem = mems.GetByID(Convert.ToInt32(this.housemainentity.MemberID));

            
        }
        public List<BedDtails> bedsintros { get; set; }


        public List<HouseQuality> housecriticles { get; set; }


        public HouseMain housemainentity { get; set; }
        public List<ImageForRoom> imagelist { get; set; }
        public Members mem { get; set; }

        public HouseQuality addacomment { get; set; }

        public Booking bookdetails { get; set; }
    }
}