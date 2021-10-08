using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmalForLife.Models
{
    public class MResponse
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        //public bool IsSuccess { get; set; }
        public object Data { get; set; }
    }
}