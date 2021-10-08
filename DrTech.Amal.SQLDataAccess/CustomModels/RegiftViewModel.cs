using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
    public class RegiftViewModel
    {
        public RegiftViewModel()
        { }

        public int ID { get; set; }
        public string Description { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string FileNameTakenByUser { get; set; }
        public string FileNameTakenByDriver { get; set; }
        public string FileNameTakenByOrg { get; set; }
        public bool? CollectedPendingConfirmation { get; set; }
        public bool? DeliveredPendingConfirmation { get; set; }
        public int OrderID { get; set; }
        public int? AssignTo { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; } 
        public string UserAddress { get; set; }
        public string StatusName { get; set; }
        public DateTime? PickDate { get; set; }
        public string PickupDate { get; set; }
        public List<RegiftSubItemViewModel> RegiftSubItems { get; set; }
        public int? GPV { get; set; }
        public int TotalGP { get; set; }
        public int OrderStatusID { get; set; }
        public string Comments { get; set; }
        public List<CommentsViewModel> RegiftComments { get; set; }
    }

    public class RegiftSubItemViewModel
    {
        public RegiftSubItemViewModel()
        { }

        public int ID { get; set; }
        public int? TypeID { get; set; }
        public int? Qty { get; set; }
    }  
}
