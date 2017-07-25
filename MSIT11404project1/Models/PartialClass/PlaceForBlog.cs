using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSIT11404project1.Models
{
    [MetadataType(typeof(PlaceForBlogMeta))]
    public partial class PlaceForBlog
    {
        public class PlaceForBlogMeta
        {
            public int PlaceID { get; set; }
            public string PlaceName { get; set; }
        }
    }
}