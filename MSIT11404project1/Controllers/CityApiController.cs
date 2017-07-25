using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MSIT11404project1.Models;
namespace MSIT11404project1.Controllers
{
    public class CityApiController : ApiController
    {
        IRepository<Cities> db = new Repository<Cities>();
        public Result PostNewCity(Cities city)
        {
            Result r = new Result();
            var q = db.GetAll().Where(c => c.CityName == city.CityName).ToList();
            if (q.Count == 0)
            {
                r.checkResult = 0;
                db.Create(city);
            }
            else
            {
                r.checkResult = 1;
            }
            return r;
        }
        [HttpGet]
        public Result GetCheckCity(string id) {
            string strcity = id.ToString();
            Result r = new Result();
            var q = db.GetAll().Where(c => c.CityName == strcity).ToList();
            if (q.Count == 0)
            {
                r.checkResult = 0;
            }
            else
            {
                r.checkResult = 1;
            }
            return r;
        }
        [HttpGet]
        public string GetCheckCity(int id,string n)
        {
            var name = db.GetAll().Where(c => c.CityID == id).Select(c => c.CityName).First();
            return name;
        }
        [HttpGet]
        public TempCity GetCheckCity(string id, string getData,string m)
        {
            var tempcity = db.GetAll().Where(c => c.CityName == id).Select(c => new TempCity { CityID = c.CityID, CityCountry = c.CityCountry, CityName = c.CityName, CityContinent = c.CityContinent }).First();
            return tempcity;
        }
        [HttpGet]
        public int GetCheckCity(string id, string getid)
        {
            var cityid = db.GetAll().Where(c => c.CityName == id).Select(c => c.CityID).First();
            return cityid;
        }
    }
    public class Result
    {
       public  int checkResult;
    }
    public class TempCity
    {
        public int CityID;
        public string CityContinent;
        public string CityName;
        public string CityCountry;
    }
}
