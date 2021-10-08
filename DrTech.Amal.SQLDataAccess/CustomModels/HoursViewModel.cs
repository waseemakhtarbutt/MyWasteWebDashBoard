using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
   public class HoursViewModel
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime SchduleDate { get; set; }
        public int AreaID { get; set; }
    }
}
