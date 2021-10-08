using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
    public class SegregatedDataViewModel
    {
        public string Type { get; set; }
        public int RecycleID { get; set; }
        public decimal? Weight { get; set; }
        public decimal? rate { get; set; }
        public decimal? total { get; set; }
        public int RowNumber { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? CollectedDate { get; set; }
        public int Days { get; set; }

    }

    public class SegregatedDataViewModelType
    {
        public string Type { get; set; }
        public List<SegregatedDataViewModel> Model { get; set; }


    }
}
