using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSIT11404project1.Models;
using MSIT11404project1.Areas.Accomodation.Models;
namespace MSIT11404project1.Areas.Accomodation.ViewModel
{
    //和admin不一樣的InsertHouse 這個是面對使用者 以使用者ID為主
    public class basicviewmodel
    {
        IRepository<ImageForRoom> imgs = new Repository<ImageForRoom>();
        IRepository<Members> datas = new Repository<Members>();
        HouseMainRepository houserepo = new HouseMainRepository();
        BedDetailsRepository beds = new BedDetailsRepository();
        public basicviewmodel() { }
        public basicviewmodel(int hkey) {
            //透過 thefirst 就可以知道是哪一個房間要編輯

            List<ImageForRoom> imglis = new List<ImageForRoom>();

            List<BedDtails> bedlists = new List<BedDtails>();
            if (hkey != 0) {
                this.housesentity = houserepo.GetByID(hkey);
                bedlists = beds.GetAll().AsEnumerable().Where(n => n.HouseKey ==hkey).ToList();
            if (bedlists.Count != 0) {
                this.bedss = bedlists;
            }
                imglis = imgs.GetAll().Where(n => n.HouseKey == hkey).ToList();
                if (imglis.Count != 0) {
                this.imglist = imglis;
            }
            }
            
           
            


            
            

        }

        public List<HttpPostedFileBase> pics { get; set; }
        public List<BedDtails> bedss { get; set; }
        public HouseMain housesentity { get; set; }

        

        public Members memsentity { get; set; }
        public List<ImageForRoom> imglist { get; set; }
    }
}