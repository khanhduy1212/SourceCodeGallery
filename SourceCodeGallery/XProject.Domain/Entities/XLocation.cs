using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace XProject.Domain.Entities
{
   public class XLocation : EntityBase
    {
     
        public string CountryCode { get; set; }
        public int StateId { get; set; }
        public int DistrictId { get; set; }
        public int NameId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Polygon { get; set; }
        [ForeignKey("NameId")]
        public virtual XLocationText LocationTexts { get; set; }
    }
}
