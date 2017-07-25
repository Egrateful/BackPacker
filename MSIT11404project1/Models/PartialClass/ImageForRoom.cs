using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MSIT11404project1.Models;
namespace MSIT11404project1.Models
{
    [MetadataType(typeof(imageforroom))]
    public partial class ImageForRoom
    {
        public class imageforroom {
            [DisplayName("照片編號")]
            public int ImageKey { get; set; }
            [DisplayName("房源編號")]
            public Nullable<int> HouseKey { get; set; }
            [DisplayName("照片來源")]
            public byte[] ImageByte { get; set; }
        }

    }
}