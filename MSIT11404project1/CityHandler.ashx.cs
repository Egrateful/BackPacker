using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSIT11404project1.Models;
using Newtonsoft.Json;
namespace MSIT11404project1
{
    /// <summary>
    /// CityHandler 的摘要描述
    /// </summary>
    public class CityHandler : IHttpHandler
    {
        MSIT11404Entities db = new MSIT11404Entities();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            List<string> CityName = new List<string>();
            foreach (var obj in db.Cities.ToList()) {
                CityName.Add(obj.CityName);
            }
            context.Response.Write(JsonConvert.SerializeObject(CityName.ToList()));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}