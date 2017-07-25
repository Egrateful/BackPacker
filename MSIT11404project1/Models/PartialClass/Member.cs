using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MSIT11404project1.Models
{
    public enum Sex
    {
        女士,
        先生
    }
    [MetadataType(typeof(MembersMeta))]
    public partial class Members
    {
        public class MembersMeta
        {
            [DisplayName("會員編號")]
            public int MemberID { get; set; }
            [DisplayName("電子信箱")]
            public string Email { get; set; }
            [DisplayName("密　　碼 ")]
            public string Password { get; set; }
            [DisplayName("姓　　名")]
            public string UserName { get; set; }

            [DisplayName("西元生日")]
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            public Nullable<System.DateTime> Birthday { get; set; }

            [DisplayName("證件號碼")]
            public string OfficialID { get; set; }
            [DisplayName("性　　別")]
            public Sex Gender { get; set; }
            [DisplayName("住　　址")]
            public string Address { get; set; }
            [DisplayName("行動電話")]
            public string Phone { get; set; }
            [DisplayName("照    片")]
            public byte[] UserPhoto { get; set; }

        }
    }
}