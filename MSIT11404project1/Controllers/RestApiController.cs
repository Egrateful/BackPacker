using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MSIT11404project1.Models;

namespace MSIT11404project1.Controllers
{
    public class RestApiController : ApiController
    {
        IRepository<Restaurants> Restaurant = new Repository<Restaurants>();
        public IQueryable<Restaurants> GetAllRestaurants()
        {
            return Restaurant.GetAll();
        }
        public IQueryable<Restaurants> GetRestaurantsBySearch(string searchby, int page,string x, string y )
        {
            IQueryable<Restaurants> list = Restaurant.GetAll();

            //有沒有輸入條件
            if (searchby != "all")
                list = list.Where(r => r.Name.Contains(searchby) || r.Address.Contains(searchby) || r.Cities.CityName.Contains(searchby));


            //判斷取幾筆
            if (list.Count() - page * 10 < 10)
                list = list.OrderBy(r => r.RestaurantID).Skip(page * 10).Take(list.Count() - page * 10);
            else
                list = list.OrderBy(r => r.RestaurantID).Skip(page * 10).Take(10);

            return list;
        }

        
        public IQueryable<Restaurants> GetRestaurantsByCondition(string id, string nothing)
        {
            return Restaurant.GetAll().Where(r => r.Name.Contains(id) || r.Address.Contains(id) || r.Cities.CityName.Contains(id));
        }
        public Restaurants GetRestaurant(int id)
        {
            return Restaurant.GetByID(id);
        }
        public void PostRestaurant(Restaurants rest)
        {
            Restaurant.Create(rest);
        }
        public void PutRestaurant(Restaurants rest)
        {
            Restaurant.Update(rest);
        }
        public void DeleteRestaurant(Restaurants rest)
        {
            Restaurant.Delete(rest);
        }

    }
}
