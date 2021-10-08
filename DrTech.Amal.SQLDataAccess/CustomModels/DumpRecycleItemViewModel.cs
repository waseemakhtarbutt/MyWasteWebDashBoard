using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
   public class DumpRecycleItemViewModel
    {
        public DumpRecycleItemViewModel()
        {
            lists = new List<SubItem>();
        }
        public int businessID { get; set; }
        public int companyID { get; set; }
        public DateTime collectDate { get; set; }
        public int RecycleID { get; set; }
        public decimal? TWeight { get; set; }
        public List<SubItem> lists { get; set; }
    }
    public class SubItem
    {
        public int typeID { get; set; }
        public decimal? Weight { get; set; }
        public decimal? rate { get; set; }
        public decimal? total { get; set; }
    }
    public class ChangePasswordveiwModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class RecycleSaveDataView
    {
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public decimal? Weight { get; set; }
        public DateTime CollectionDate { get; set; }

    }
}
