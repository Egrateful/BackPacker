using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace MSIT11404project1.Models
{
    [MetadataType(typeof(PicMeta))]
    public partial class Pic
    {
        public class PicMeta
        {
            public int PhotoID { get; set; }
            public int DocID { get; set; }
            public string PhotoName { get; set; }
            public byte[] Photo { get; set; }
            public string Description { get; set; }
            public string TakeTime { get; set; }
            public Nullable<bool> IsShow { get; set; }
            public Nullable<bool> IsCover { get; set; }
        }
    }
}