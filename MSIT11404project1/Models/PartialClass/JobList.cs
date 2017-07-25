using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MSIT11404project1.Models
{
    [MetadataType(typeof(JobListMetadata))]
    public partial class JobList
    {
        public class JobListMetadata
        {
            [DisplayName("工作編號")]
            [Required(ErrorMessage = "請輸入工作編號")]
            [Range(1, 10000, ErrorMessage = "請輸入1到10000之間")]
            public int JobId { get; set; }

            [DisplayName("公司編號")]
            [Required(ErrorMessage = "請輸入公司編號")]
            [Range(1, 10000, ErrorMessage = "請輸入1到10000之間")]
            public int CompanyId { get; set; }

            [DisplayName("公司名稱")]
            [Required(ErrorMessage = "請輸入公司名稱")]
            public int CompanyName { get; set; }
            
            [DisplayName("工作頭銜")]
            [StringLength(50)]
            public string JobTitle { get; set; }

            [DisplayName("工作描述")]
            [StringLength(50)]
            public string JobDescription { get; set; }

            [DisplayName("聯絡人員")]
            [StringLength(50)]
            public string Contacts { get; set; }

            [DisplayName("連絡電話")]
            [StringLength(50)]
            public string Phone { get; set; }

            [DisplayName("電子郵件")]
            [StringLength(50)]
            public string Email { get; set; }

            [DisplayName("公司地址")]
            [StringLength(50)]
            public string Address { get; set; }

            [DisplayName("所在城市")]
            [StringLength(50)]
            public string City { get; set; }

            [DisplayName("所在區域")]
            [StringLength(50)]
            public string Region { get; set; }

            [DisplayName("郵遞區號")]
            [StringLength(50)]
            public string PostalCode { get; set; }

            [DisplayName("所在國家")]
            [StringLength(50)]
            public string Country { get; set; }

            [DisplayName("教育程度")]
            [StringLength(50)]
            public string EducationalBackground { get; set; }

            [DisplayName("相關科系")]
            [StringLength(50)]
            public string Department { get; set; }

            [DisplayName("工作圖片")]
            public string Photo { get; set; }
        }
    }
}