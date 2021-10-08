using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
  

    public class RecycleDetailChartVM
    {
        public RecycleDetailChartVM()
        {
            series = new List<Records>();
        }
        public string name { get; set; }
        public List<Records> series { get; set; }
    }
    public class Records
    {
        public string name { get; set; }
        public decimal? value { get; set; }
    }
    public class MonthViewModel
    {
        public string MON { get; set; }
        public string name { get; set; }
        public Nullable<decimal> wei { get; set; }
    }
}
