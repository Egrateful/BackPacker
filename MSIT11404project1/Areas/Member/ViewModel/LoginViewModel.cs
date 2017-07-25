using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MSIT11404project1.Models;
using System.ComponentModel;


namespace MSIT11404project1.Areas.Member.ViewModel
{
    public class LoginViewModel
    {
        [DisplayName("電子郵件")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "電子郵件格式不對")]
        [Required(ErrorMessage = "要輸入電子郵件地址")]
        public string Email { get; set; }

        [DisplayName("密碼")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "要輸入密碼")]
        public string Password { get; set; }


        [DisplayName("輸入驗證碼")]
        [Required(ErrorMessage = "要輸入驗證碼")]          
        public string Captcha{ get; set; }
       

        //[DisplayName("記住我")]
        //public bool RememberMe { get; set; }
    }
}

