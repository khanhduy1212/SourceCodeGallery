using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace XProject.Domain
{
    public class XBaseEntiry : EntityBase
    {
        public int Active { get; set; }
        public string KeySearch { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreateUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedUser { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public int? Order { get; set; }
       
        public XBaseEntiry()
        {
            Active = 1;
            CreateDate = DateTime.Now;
            
        }
        [NotMapped]
        public string CreateUserName { get; set; }
        [NotMapped]
        public string ModifiedUserName { get; set; }
        
    }
}
