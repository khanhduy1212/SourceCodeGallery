using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace XProject.Domain.Entities
{
  public  class XDistrict : EntityBase
    {
       
        public int StateId { get; set; }
        public int NameId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Polygon { get; set; }
        [ForeignKey("NameId")]
        public virtual XDistrictText DistrictTexts { get; set; }
    }
}
