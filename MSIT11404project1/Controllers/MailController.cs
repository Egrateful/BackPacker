using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Mail;
namespace MSIT11404project1.Controllers
{
    public class MailController : ApiController
    {
        public class sendmail {
           public string account { get; set; }
           public  string password { get; set; }
           public string textting { get; set; }
           public string landlordemail { get; set; }
        }
        public void PostMail(sendmail mail) {

            SmtpClient one = new SmtpClient("smtp.gmail.com", 587);
            one.Credentials = new NetworkCredential(mail.account, mail.password);
            one.EnableSsl = true;
            one.Send(mail.account, mail.landlordemail, "檢舉了喔", mail.textting);
            one.Dispose();
            
        }
    }
}
