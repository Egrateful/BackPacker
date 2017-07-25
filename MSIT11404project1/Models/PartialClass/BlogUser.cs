using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSIT11404project1.Models
{
    [MetadataType(typeof(BlogUserMeta))]
    public partial class  BlogUser
    {
        public class BlogUserMeta
        {
            public int Nid { get; set; }
            public string Accon { get; set; }
            public string Name { get; set; }
            public byte[] HeadPic { get; set; }
            public Nullable<int> LevelState { get; set; }
        }

    }
}