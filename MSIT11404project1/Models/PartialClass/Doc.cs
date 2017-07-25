using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MSIT11404project1.Models
{
    [MetadataType(typeof(DocMeta))]
    public partial class Doc
    {
        public class DocMeta
        {
            public int DocID { get; set; }
            public int Nid { get; set; }
            public string Title { get; set; }

            [AllowHtml]
            public string Article { get; set; }
            public string CreateTime { get; set; }
            public string ModifyTime { get; set; }
            public bool IsShow { get; set; }
            public int Good { get; set; }
            public int PlaceID { get; set; }
            public int MoodID { get; set; }
        }
    }
}