using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
    public class CommonViewModelForEmail
    {
        public string Idea { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string FileName { get; set; }

        public decimal Longitude { get; set; } = 0;
        public decimal Latitude { get; set; } = 0;

        public string TypeDescription { get; set; }

        public string SubTypeDescription { get; set; }

        public string PlantName { get; set; }
        public int TreeCount { get; set; }
        public string Height { get; set; }

        public string DonateToDescription { get; set; }

        public string TrackingNumber { get; set; }

        public string BinName { get; set; }

        public string ChildName { get; set; }

        public string RollNo { get; set; }

        public string SchoolName { get; set; }

        public string Gender { get; set; }

        public string Branch { get; set; }

        public string OrgName { get; set; }
        public string EmployeeID { get; set; }
        public string Email { get; set; }
    }
}
