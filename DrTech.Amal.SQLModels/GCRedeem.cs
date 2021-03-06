//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DrTech.Amal.SQLModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class GCRedeem
    {
        public int ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<decimal> GCRedeemed { get; set; }
        public Nullable<decimal> AmountGivenToUser { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual User User { get; set; }
    }
}
