//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MSIT11404project1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HouseQuality
    {
        public int HouseKey { get; set; }
        public int MemberID { get; set; }
        public string Messages { get; set; }
        public Nullable<int> Score { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int CommentID { get; set; }
        public Nullable<bool> IsChecked { get; set; }
    
        public virtual HouseMain HouseMain { get; set; }
        public virtual Members Members { get; set; }
    }
}