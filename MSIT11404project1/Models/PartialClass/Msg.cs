using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace MSIT11404project1.Models
{
    public partial class Msg
    {
        [MetadataType(typeof(MsgMeta))]
        public class MsgMeta
        {
            public int MsgID { get; set; }
            public int DocID { get; set; }
            public string Msg1 { get; set; }
            public string CreateTime { get; set; }
            public Nullable<bool> IsShow { get; set; }
            public Nullable<int> Nid { get; set; }

        }



    }
}