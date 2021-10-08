using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmalForLife.Models
{
    public class MBuyBin
    {
        public string Id { get; set; } 
        public string BinId { get; set; } = string.Empty;
        public int Status { get; set; } = 0;
        public string StatusDescription { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;

        public int Qty { get; set; } = 0;
        public string TrackingNumber { get; set; } = "";
        public string FileName { get; set; }
        public double Price { get; set; }
        public string BinName { get; set; }
    }
    }