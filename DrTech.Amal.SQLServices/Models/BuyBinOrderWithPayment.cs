using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrTech.Amal.SQLServices.Models
{
    public  class BuyBinOrderWithPayment
    {
        public BuyBinOrderWithPayment()
        {
            this.BuyBinComments = new HashSet<BuyBinComment>();
        }

        public int ID { get; set; }
        public int BinID { get; set; }
        public int StatusID { get; set; }
        public int UserID { get; set; }
        public int Qty { get; set; }
        public string TrackingNumber { get; set; }
        public string FileName { get; set; }
        public decimal Price { get; set; }
        public decimal? RemainingAmount { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> GreenPoints { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<int> UserPaymentID { get; set; }

        public virtual BinDetail BinDetail { get; set; }
        public virtual Status Status { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuyBinComment> BuyBinComments { get; set; }
        public virtual UserPayment UserPayment { get; set; }
        public decimal? deductedFromWallet { get; set; }
        public int paymentMethodID { get; set; }
        public decimal? PaidAmount { get; set; }


    }
}