using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace XProject.Domain.Entities
{
    public class XPartner : XBaseEntiry
    {


        public string CodeLanguage { get; set; }
        public string UrlImage { get; set; }
        public string Link { get; set; }
        public int CurrentId { get; set; }

        [NotMapped]
        public XPartner[] XPartnersArray { get; set; }
        [NotMapped]
        public List<XPartner> XPartnersList { get; set; }
       

    }
    
}
