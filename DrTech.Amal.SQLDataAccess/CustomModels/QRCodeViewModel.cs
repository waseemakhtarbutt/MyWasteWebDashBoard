using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
    public class QRCodeViewModel
    {
            public int ID { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
        
    }

    public class UserTemplateViewModel
    {
        public string SMSCode { get; set; }
    }
}
