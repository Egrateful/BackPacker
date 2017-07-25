using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MSIT11404project1.Models;

namespace MSIT11404project1.Models
{
    [MetadataType(typeof(housemain))]
    public partial class HouseMain
    {
        private MSIT11404Entities db = new MSIT11404Entities();
        public class housemain
        {
            
            public int HouseKey { get; set; }
            [DisplayName("房源種類")]
            public Nullable<int> HouseSourceID { get; set; }
            [DisplayName("房源空間")]
            public Nullable<int> HouseSpaceTypeID { get; set; }
            
            [DisplayName("地址")]
           
            public string Adress { get; set; }
            [DisplayName("所在國家")]
         
            public string Country { get; set; }
            [DisplayName("所在城市")]
            public string State { get; set; }
            [DisplayName("廁所數量")]
            public Nullable<int> BathroomNum { get; set; }

            [DisplayName("房間數")]
            public Nullable<int> BedroomNum { get; set; }



            [DisplayName("房東同住")]
            public Nullable<bool> IsOwnerHome { get; set; }
            [DisplayName("是否在線")]
            public Nullable<bool> IsActive { get; set; }
            [DisplayName("會員編號")]
            public Nullable<int> MemberID { get; set; }
            [DisplayName("是否停權")]
            public Nullable<bool> IsFreeze { get; set; }
            [DisplayName("容納人數")]
            [Range(1,15)]
            public Nullable<int> PeopleAllowed { get; set; }
            [DisplayName("床數")]
            public Nullable<int> BedAvailableNum { get; set; }
            [DisplayName("價格")]
            [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString ="{0:c0}")]
            public Nullable<decimal> Price { get; set; }

            [DisplayName("登入日期")]
           
            [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString ="{0:yyyy-MM-dd}")]
            public Nullable<System.DateTime> CreateDate { get; set; }
            [DisplayName("房源型態")]
            public virtual HouseSourceType HouseSourceType { get; set; }
            [DisplayName("房源類別")]
            public virtual HouseSpace HouseSpace { get; set; }
            [DisplayName("負評記錄")]
            public Nullable<int> BadComment { get; set; }
            [DisplayName("房源名稱")]
            
            public string HouseName { get; set; }
           
            }
        
        
            //從MemberID找HouseKey並判斷是否有發布房間
            public bool findhousekey(int MemberID,out List<int> housekeys) {
            var housekey = db.HouseMain.Where(n => n.MemberID == MemberID).Select(n => n.HouseKey).ToList();
            if (housekey != null)
            {
                housekeys = housekey;
                return true;

            }
            else {
                housekeys = null;
                return false;
            }
               
        }
    }
}