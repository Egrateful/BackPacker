using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MSIT11404project1.Models;
using MSIT11404project1.Areas.Accomodation.Models;
namespace MSIT11404project1.Controllers
{
    public class bookingapiController : ApiController
    {
        public class alterbook {
            public int bookid { get; set; }
            public bool Isconfirm { get; set; }
            public bool Isreject { get; set; }
            
        }

        public void Postbooks(Booking books) {
            books.Createdate = DateTime.Now;
            BookingRepository a = new BookingRepository();
            a.Create(books);
        }

       [Route(template:"api/booking/alter",Name ="AlterConfirm")]
        public void Postbooks(alterbook reply) {
            BookingRepository breop = new BookingRepository();
             Booking ch=breop.GetByID(reply.bookid);
            ch.IsRejected = reply.Isreject;
            ch.IsConfirm = reply.Isconfirm;
            ch.ReplyDate = DateTime.Now;

            breop.Update(ch);
        }
    }
}
