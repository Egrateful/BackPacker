using MSIT11404project1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MSIT11404project1.Controllers
{
    public class TravelMessageApiController : ApiController
    {
        IRepository<Message> mess = new Repository<Message>();

        public void PostMessage(Message message)
        {
            mess.Create(message);
        }
    }
}
