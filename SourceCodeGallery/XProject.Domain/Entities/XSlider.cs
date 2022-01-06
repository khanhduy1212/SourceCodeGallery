using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace XProject.Domain.Entities
{
    public class XSlider : XBaseEntiry
    {


        public string CodeLanguage { get; set; }
        public string UrlImage { get; set; }
        public string Link { get; set; }
        public int CurrentId { get; set; }

        [NotMapped]
        public XSlider[] XSlidersArray { get; set; }
        [NotMapped]
        public List<XSlider> XSlidersList { get; set; }
       

    }
    
}
