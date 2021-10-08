using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Amal.Common.ServerResponse
{
    public class ResponseObject<TObject>
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        //public bool IsSuccess { get; set; }
        public TObject Data { get; set; }
    }
}