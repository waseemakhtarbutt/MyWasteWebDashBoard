using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
  public  class PaymentViewModel
    {
        public int binId { get; set; }
        public int qty { get; set; }
        public string fileName { get; set; }
        public int paymentMethodID { get; set; }
        public string PaymentMethod { get; set; }
        public decimal price { get; set; }
        public string amount { get; set; }
        public decimal paidAmount { get; set; }
        public decimal deductedFromWallet { get; set; }
        public string Mobile { get; set; } 
        public string Email { get; set; }
        public string auth_token { get; set; }
        public string postBackURL { get; set; }
        public int UserID { get; set; }
        public string transactionNumber { get; set; }
        public string batchNumber { get; set; }


    }
}
