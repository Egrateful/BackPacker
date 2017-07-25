using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace MSIT11404project1.Models
{
    [MetadataType(typeof(MoodMeta))]
    public partial class Mood
    {
        public class MoodMeta
        {
            public int MoodID { get; set; }
            public string MoodName { get; set; }
        }
    }
}