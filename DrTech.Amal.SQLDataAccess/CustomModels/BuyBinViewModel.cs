using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
    public class BuyBinViewModel
    {
        public BuyBinViewModel()
        { }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string FileNameTakenByUser { get; set; }
        public int OrderID { get; set; }
        public int? AssignTo { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; } 
        public string UserAddress { get; set; }
        public string StatusName { get; set; }
        public DateTime? DeliverDate { get; set; }
        public string DeliveryDate { get; set; }
        public List<BinDetailViewModel> BinSubItems { get; set; }
        public int? GPV { get; set; }
        public int TotalGP { get; set; }
        public int OrderStatusID { get; set; }
        public double Price { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? RemainingAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string Comments { get; set; }
        public List<CommentsViewModel> BuyBinComments { get; set; }
    }

    public class BinDetailViewModel
    {
        public BinDetailViewModel()
        { }

        public int ID { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
    }
}
