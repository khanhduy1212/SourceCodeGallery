using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XProject.Domain.Models
{
    public class ResponseModel<T>
    {
        public int Code { get; set; }
        public int UserId { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
