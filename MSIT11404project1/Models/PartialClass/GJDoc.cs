using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSIT11404project1.Models
{
    [MetadataType(typeof(GJDocMeta))]
    public partial class GJDoc 
    {
        public class GJDocMeta
        {
            public int DocID { get; set; }
            public int Nid { get; set; }
            public Nullable<int> State { get; set; }

            public virtual BlogUser BlogUser { get; set; }
            public virtual Doc Doc { get; set; }

        }
    }
}