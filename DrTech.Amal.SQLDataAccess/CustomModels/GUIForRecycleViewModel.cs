using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
   public class GUIForRecycleViewModel
    {
        public decimal Weight { get; set; }
        public int companyID { get; set; }
        public DateTime collectDate { get; set; }
        public int businessID { get; set; }
        public int typeID { get; set; }
    }
    public class GUIViewModel
    {
        public DateTime Date { get; set; }
        public decimal Weight { get; set; }
        public int GreenPoints { get; set; }
        public DateTime Time { get; set; }
    }
    public class GOIForGrapViewModel
    {
        public int companyID1 { get; set; }
        public int companyID2 { get; set; }
        public int companyID3 { get; set; }
    }
}
