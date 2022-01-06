using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace XProject.Domain.Entities
{
    public class XProduct : XBaseEntiry
    {


        public string CodeLanguage { get; set; }
        public string UrlImage { get; set; }
        public string Link { get; set; }
        public int CurrentId { get; set; }
        public int Type { get; set; }
        public int TypeId { get; set; }
        public int IsLink { get; set; }

        [ForeignKey("TypeId")]
        public virtual XMenu XMenu  { get; set; }
    [NotMapped]
        public XProduct[] XProductsArray { get; set; }
        [NotMapped]
        public List<XProduct> XProductsList { get; set; }

        public string Includes { get; set; }
     
        public string Title { get; set; }
        [NotMapped]
        public int CountImg { get; set; }
    }
    
}
