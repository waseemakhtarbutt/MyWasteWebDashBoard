using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
    public class DesegregatedDataViewModel
    {
        public int UserID { get; set; }
        public int ID { get; set; }
        public decimal Weight { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public DateTime? date { get; set; }

        public bool? IsActive { get; set; }
    }
    public class DateRangeViewMdoel
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public int companyID { get; set; }
        public int branchID { get; set; }
    }
}
