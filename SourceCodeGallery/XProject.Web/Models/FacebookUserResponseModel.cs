﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XProject.Web.Models
{
    public class FacebookUserResponseModel
    {
        public string id { get; set; }

        public string name { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string gender { get; set; }

        public string email { get; set; }
        
    }
}