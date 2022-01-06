using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace XProject.Domain.Entities
{
    public class XContent : XBaseEntiry
    {


        public string CodeLanguage { get; set; }
        public string UrlImage { get; set; }
        public string Link { get; set; }
        public int CurrentId { get; set; }
        

        [NotMapped]
        public XContent[] XContentsArray { get; set; }
        [NotMapped]
        public List<XContent> XContentsList { get; set; }
      
        public int Type { get; set; }
        [ForeignKey("Type")]
        public virtual XType XType { get; set; }


    }
    
}
