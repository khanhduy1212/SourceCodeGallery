﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace XProject.Domain.Entities
{
    public class XNew : XBaseEntiry
    {


        public string CodeLanguage { get; set; }
        public string UrlImage { get; set; }
        public string Link { get; set; }
        public int CurrentId { get; set; }
        public int Type { get; set; }

        [NotMapped]
        public XNew[] XNewsArray { get; set; }
        [NotMapped]
        public List<XNew> XNewsList { get; set; }
       

    }
    
}
