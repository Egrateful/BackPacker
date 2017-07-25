using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MSIT11404project1.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using MSIT11404project1.Areas.ViewModel;
namespace MSIT11404project1.Controllers
{
    public class tempimages{
        public string descripts { get; set; }
        public int keys { get; set; }
        
    }
   
    public class ImagesController : ApiController
    {
        IRepository<ImageForRoom> imgs = new Repository<ImageForRoom>();
        //public IQueryable <ImageForRoom> Getall() {
        //    return imgs.GetAll();


        //}
        


       
        public IEnumerable<ImageForRoom> GetImageByHouseKey(int id) {

            InsertHouse one = new InsertHouse(id);

            return one.EntityImages;
        }

        public void PutDatas(tempimages abc) {

            ImageForRoom aroom = new ImageForRoom();
            aroom = imgs.GetByID(abc.keys);
            aroom.Descript = abc.descripts;
            imgs.Update(aroom);
        }



        public void DeleteImageKeyArray(String id)
        {

            IEnumerable<string> datas = id.Split(',');
            foreach (string a in datas) {

                int ss = Convert.ToInt32(a);

                if (imgs.GetByID(ss) != null) {
                imgs.Delete(imgs.GetByID(ss)); }
            }


        }

      
    }
}
