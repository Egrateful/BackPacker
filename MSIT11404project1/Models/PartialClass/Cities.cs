using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace MSIT11404project1.Models
{ 
    [MetadataType(typeof(CityMetaData))]
    public partial class Cities
    {
        public class CityMetaData
        {
            [DisplayName("城市編號")]
            public int CityID { get; set; }

            [DisplayName("城市名稱")]
            public string CityName { get; set; }

            [DisplayName("所在國家")]
            public string CityCountry { get; set; }

            [DisplayName("城市區域")]
            public string CityContinent { get; set; }
        }
        
    }
}