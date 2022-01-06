using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace XProject.Domain.Entities
{
  public  class XCountry : XBaseEntiry
    {

        public bool IsLangauge { get; set; }
        public string Code { get; set; }
        public string UrlImage { get; set; }
        public string Class { get; set; }
        
    }
}
