using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace XProject.Domain.Entities
{
    public class Language : EntityBase
    {
        public string Value { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }

        //public int CurrencyID { get; set; }
        //[ForeignKey("CurrencyID")]
        //public virtual Currency Currency { get; set; }
    }
}
