using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
    public class GPNViewModelForSMS
    {
        public string Name { get; set; }
        public string ChildName { get; set; }
        public string SchoolName { get; set; }

        public string OrgName { get; set; }

        public string NGOName { get; set; }

        public string Branch { get; set; }
    }
    public class GOILocationViewModelForSMS
    {
        public string Company { get; set; }
        public string Location { get; set; }
        public string Date { get; set; }
        public string Weight { get; set; }
    }
}
