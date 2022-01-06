using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XProject.Web.Models
{
    public class FacebookLoginRequestModel
    {
        public string TokenKey { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string FirstName { get; set; }
        public string UrlReturn { get; set; }
        public int TypeRegister { get; set; }
    }
    public class GoogleUserResponseModel
    {
        public string id { get; set; }

        public string name { get; set; }

        public string given_name { get; set; }

        public string family_name { get; set; }

        public string gender { get; set; }

        public string email { get; set; }
        public string picture { get; set; }
        public string locale { get; set; }

        public string verified_email { get; set; }
    }
}