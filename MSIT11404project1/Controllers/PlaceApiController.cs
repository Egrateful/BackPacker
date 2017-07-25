using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MSIT11404project1.Models;
namespace MSIT11404project1.Controllers
{
    public class PlaceApiController : ApiController
    {

        IRepository<Places> place = new Repository<Places>();
        public IQueryable<Places> GetAllPlaces()
        {
            return place.GetAll();
        }
        public IQueryable<Places> GetPlacesBySearch(string searchby, int page, string x, string y )
        {
            IQueryable<Places> list = place.GetAll();

            //有沒有輸入條件
            if (searchby != "all")
                list = list.Where(p => p.Name.Contains(searchby) || p.Address.Contains(searchby) || p.Cities.CityName.Contains(searchby));


            //判斷取幾筆
            if (list.Count() - page * 10 < 10)
                list = list.OrderBy(p => p.PlaceID).Skip(page * 10).Take(list.Count() - page * 10);
            else
                list = list.OrderBy(p => p.PlaceID).Skip(page * 10).Take(10);
            return list;
        }



        public IQueryable<Places> GetPlacesByCondition(string id, string nothing)
        {
            return place.GetAll().Where(p => p.Name.Contains(id) || p.Address.Contains(id) || p.Cities.CityName.Contains(id));
        }
        public Places GetPlace(int id)
        {
            return place.GetByID(id);
        }
        //public void PostRestaurant(Places rest)
        //{
        //    place.Create(rest);
        //}
        //public void PutRestaurant(Places rest)
        //{
        //    place.Update(rest);
        //}
        //public void DeleteRestaurant(Places rest)
        //{
        //    place.Delete(rest);
        //}
    }
}
