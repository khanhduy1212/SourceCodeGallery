using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace XProject.Domain.Entities
{
    public class XEmail : XBaseEntiry
    {


        public string Phone { get; set; }
        public string Address { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }


    }
    
}
