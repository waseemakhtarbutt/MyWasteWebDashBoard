using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
 public   class AssociationVeiwModel
    {
        public int SchoolID { get; set; }
        public int BusinessID { get; set; }
        public int OrganizationID { get; set; }
        public int UserID { get; set; }
        public int GreenPoints { get; set; }
        public int RsID { get; set; }
        public string Type { get; set; }
        public int LoginAdminUserId { get; set; }
        public int? ChildID { get; set; }

        public int? StaffID { get; set; }
        public int? EmployeeID { get; set; }
        public int? MemeberID { get; set; }
    }
}
