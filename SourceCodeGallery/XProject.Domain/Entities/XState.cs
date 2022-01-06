using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;

namespace XProject.Domain.Entities
{
   public class XState : EntityBase
    {
      
        public string CountryCode { get; set; }
        public int NameId { get; set; }
        public string StateCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Polygon { get; set; }
        [ForeignKey("NameId")]
        public virtual XStateText StateTexts { get; set; }
    }
}
