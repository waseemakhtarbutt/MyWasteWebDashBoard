using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
   public class ShiftViewModel
    {
        public string startTimeHours { get; set; }
        public string startTimeMinutes { get; set; }
        public string endTimeHours { get; set; }
        public string endTimeMinutes { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }
}
