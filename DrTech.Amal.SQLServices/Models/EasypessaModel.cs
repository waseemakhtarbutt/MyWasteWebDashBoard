using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrTech.Amal.SQLServices.Models
{
    public class EasypessaModel
    {
        public string storeId { get; set; }
        public string amount { get; set; }
        public string postBackURL { get; set; }
        public string orderRefNum { get; set; }
        public string expiryDate { get; set; }
        public int autoRedirect { get; set; }
        public string paymentMethod { get; set; }
        public string emailAddr { get; set; }
        public string mobileNum { get; set; }
        public string merchantHashedReq { get; set; }
        public string auth_token { get; set; }
    }

    
}