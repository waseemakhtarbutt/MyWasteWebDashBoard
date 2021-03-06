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
    
    public partial class SchoolStaff
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SchoolStaff()
        {
            this.SchoolGP_Log = new HashSet<SchoolGP_Log>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public int SchoolID { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string EmployeeID { get; set; }
        public bool IsVerified { get; set; }
        public int UserID { get; set; }
        public string FileName { get; set; }
        public System.DateTime SessionStartDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> SessionEndDate { get; set; }
        public string Gender { get; set; }
    
        public virtual School School { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SchoolGP_Log> SchoolGP_Log { get; set; }
        public virtual User User { get; set; }
    }
}
