using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
    public class RecycleViewModel
    {
        public RecycleViewModel()
        { }

        public int ID { get; set; }
        public string Description { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string FileNameTakenByUser { get; set; }
        public string FileNameTakenByDriver { get; set; }
        public string FileNameTakenByOrg { get; set; }
        public bool? CollectedPendingConfirmation { get; set; }
        public bool? DeliveredPendingConfirmation { get; set; }
        public int OrderID { get; set; }
        public int? AssignTo { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string CityName { get; set; }
        public string UserPhone { get; set; } 
        public string UserAddress { get; set; }
        public string StatusName { get; set; }
        public DateTime? CollectDate { get; set; }
        public DateTime CollectorDate { get; set; }
        public List<RecycleSubItemViewModel> RecycleSubItems { get; set; }
        public int? GPV { get; set; }
        public int TotalGP { get; set; }
        public int OrderStatusID { get; set; }
        public decimal Cash { get; set; }
        public string Comments { get; set; }
        public List<CommentsViewModel> RecycleComments { get; set; }
    }

    public class RecycleSubItemViewModel
    {
        public RecycleSubItemViewModel()
        { }

        public int ID { get; set; }
        public string Description { get; set; }
        public decimal? Weight { get; set; }
    }
    public class RecycleRequest
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int StatusID { get; set; } = 0;
        public int BranchID { get; set; } = 0;
    }
}
