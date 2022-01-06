using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XProject.Web.Areas.Admin.Models
{
    public class SelectModel
    {
        public string id { get; set; }
        public string text { get; set; }
    }
    //nếu dùng int
    public class SelectIntModel
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}