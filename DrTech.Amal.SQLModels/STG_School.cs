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
    
    public partial class STG_School
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int GreenPoints { get; set; }
        public string Level { get; set; }
        public int ParentsGreenPoints { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string FileName { get; set; }
        public string Email { get; set; }
        public string BranchName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonPhone { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<bool> IsVerified { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<bool> IsMainBranch { get; set; }
        public string RegFormat { get; set; }
    }
}
