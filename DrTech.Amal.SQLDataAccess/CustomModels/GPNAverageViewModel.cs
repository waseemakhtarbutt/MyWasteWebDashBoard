using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
    public class GPNAverageViewModel
    {
        public GPNAverageViewModel()
        { }

        public decimal Refuses { get; set; }
        public decimal Reduces { get; set; }
        public decimal Reuses { get; set; }
        public decimal Replants { get; set; }
        public decimal Recycles { get; set; }
        public decimal Regifts { get; set; }
        public decimal Reports { get; set; }
        public decimal Bins { get; set; }

        public decimal Redeemed { get; set; } 
        public decimal TotalGW { get; set; }
        public decimal TotalGP { get; set; }
        public decimal? RedeemedPoints { get; set; }
        public decimal? RedeemablePoints { get; set; }
    }
}
