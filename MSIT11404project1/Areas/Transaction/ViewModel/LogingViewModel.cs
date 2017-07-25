using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MSIT11404project1.Areas.ViewModel
{
    public class LogingViewModel
    {
        [DisplayName("電子郵件信箱")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "未輸入帳號")]
        [EmailAddress(ErrorMessage = "密碼格式錯誤")]
        public string Email { get; set; }
        [DisplayName("密碼")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "未輸入密碼")]
        public string Password { get; set; }
        [DisplayName("記住我")]
        public bool RememberMe { get; set; }

        
    }
}