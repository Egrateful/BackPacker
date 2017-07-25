using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MSIT11404project1.Models;

namespace MSIT11404project1.Controllers
{
    public class HotelApiController : ApiController
    {
        IRepository<Hotels> Hotel = new Repository<Hotels>();
        IRepository<HouseMain> house = new Repository<HouseMain>();
        IRepository<Cities> city = new Repository<Cities>();
        public IQueryable<Hotels> GetAllHotels()
        {
            return Hotel.GetAll();
        }

        public IQueryable<Hotels> GetHotelsBySearch(string searchby, int page, string x, string y)
        {

            IQueryable<Hotels> list = Hotel.GetAll();

            var houselist = house.GetAll().Where(house=>house.IsActive==true).ToList();
            var templist = list.ToList();
            for (int i = 0; i < houselist.Count; i++)
            {
                Hotels h = new Hotels();
                h.HotelID = 5000 + i;
                h.Name = houselist[i].HouseName;
                h.Address = houselist[i].Adress;
                h.Image = houselist[i].ImageForRoom.ToList().First().ImageByte;
                string cityname = houselist[i].State;
                Cities tempcity = city.GetAll().Where(c => c.CityName.Contains(cityname)).First();
                h.Cities = tempcity;
                h.CityID = tempcity.CityID;
                templist.Add(h);
            }
            list = templist.AsQueryable();




            //有沒有輸入條件
            if (searchby != "all")
                list = list.Where(h => h.Name.Contains(searchby) || h.Address.Contains(searchby) || h.Cities.CityName.Contains(searchby));


            //判斷取幾筆
            if (list.Count() - page * 10 < 10)
                list = list.OrderByDescending(h => h.HotelID).Skip(page * 10).Take(list.Count() - page * 10);
            else
                list = list.OrderByDescending(h => h.HotelID).Skip(page * 10).Take(10);
            return list;
        }

        //[Route(template: "api/HotelApi/hi",Name ="hi")]
        //public IQueryable<Hotels> GetHotelsAndHouses(HotelAndHouse hah)
        //{

        //    IQueryable<Hotels> list = Hotel.GetAll();

        //    //有沒有輸入條件
        //    if (hah.searchby != "all")
        //        list = list.Where(h => h.Name.Contains(hah.searchby) || h.Address.Contains(hah.searchby) || h.Cities.CityName.Contains(hah.searchby));


        //    //判斷取幾筆
        //    if (list.Count() - hah.page * 10 < 10)
        //        list = list.OrderByDescending(h => h.HotelID).Skip(hah.page * 10).Take(list.Count() - hah.page * 10);
        //    else
        //        list = list.OrderByDescending(h => h.HotelID).Skip(hah.page * 10).Take(10);
        //    return list;
        //}





        public IQueryable<Hotels> GetHotelsByCondition(string id, string nothing)
        {
            return Hotel.GetAll().Where(h => h.Name.Contains(id) || h.Address.Contains(id) || h.Cities.CityName.Contains(id));
        }
        public Hotels GetHotel(int id)
        {
            return Hotel.GetByID(id);
        }
        //public void PostRestaurant(Hotels hotel)
        //{
        //    Hotel.Create(hotel);
        //}
        //public void PutRestaurant(Hotels hotel)
        //{
        //    Hotel.Update(hotel);
        //}
        public void DeleteHotel(Hotels hotel)
        {
            Hotel.Delete(hotel);
        }
    }

    public class HotelAndHouse
    {
        public string searchby { get; set; }
        public int page { get; set; }
        public string x { get; set; }
        public string y { get; set; }
    }
}
