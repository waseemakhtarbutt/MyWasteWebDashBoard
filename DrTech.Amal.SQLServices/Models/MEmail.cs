using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrTech.Amal.SQLServices.Models
{
    public class MEmail
    {
        public string Id { get; set; }
        public string EmailTo { get; set; } = string.Empty;
        public string EmailCC { get; set; } = string.Empty;
        public string EmailSubject { get; set; } = string.Empty;
        public string EmailBody { get; set; } = string.Empty;
        public string Status { get; set; } = "0";
        public string ServerMessage { get; set; } = "";
        public short? TryCount { get; set; } = 0;
        public string ReceiverUserType { get; set; } = "";
    }
}