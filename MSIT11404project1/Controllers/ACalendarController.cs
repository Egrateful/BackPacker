using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MSIT11404project1.Models;
namespace MSIT11404project1.Controllers
{
    public class ACalendarController : ApiController
    {
        IRepository<Calendar> ds = new Repository<Calendar>();
        public void DeleteCalendar(Calendar cal)
        {
            ds.Delete(cal);
        }

        public void PostCalender(Calendar cal)
        {
            ds.Create(cal);

        }
       
    }
}
