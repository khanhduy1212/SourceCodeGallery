using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace XProject.Domain.Entities
{
    public class XMenu : XBaseEntiry
    {


        public string CodeLanguage { get; set; }
        public string UrlImage { get; set; }
        public string LinkMenu { get; set; }
        public int IdParentMenu { get; set; }
    
        public string SmallDescription { get; set; }
        public int IsParent { get; set; }
        public int IsCategoryMenu { get; set; }
        public int ShowHomeMenu { get; set; }
        public int CurrentId { get; set; }

        [NotMapped]
        public XMenu[] XMenusArray { get; set; }
        [NotMapped]
        public List<XMenu> XMenusList { get; set; }
        [NotMapped]
        public string NameParent { get; set; }
        [NotMapped]
        public int CountImg { get; set; }
    }
}
