using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace XProject.Domain.Entities
{
    public class XAttribute : XBaseEntiry
    {


        public string CodeLanguage { get; set; }
        public int CurrentId { get; set; }
        public int Type { get; set; }

        [NotMapped]
        public XAttribute[] XAttributesArray { get; set; }
        [NotMapped]
        public List<XAttribute> XAttributesList { get; set; }
       

    }
    
}
