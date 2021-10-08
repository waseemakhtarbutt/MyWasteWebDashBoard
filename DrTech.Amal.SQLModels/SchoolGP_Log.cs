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
    
    public partial class SchoolGP_Log
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int GreenPoints { get; set; }
        public int SchoolID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> RsID { get; set; }
        public string Type { get; set; }
        public Nullable<int> ChildID { get; set; }
        public Nullable<int> StaffID { get; set; }
    
        public virtual Child Child { get; set; }
        public virtual School School { get; set; }
        public virtual SchoolStaff SchoolStaff { get; set; }
        public virtual User User { get; set; }
    }
}
