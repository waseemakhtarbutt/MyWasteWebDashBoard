using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmalForLife.Models
{
    public class MMrClean
    {
        public string FileName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string CollectorDateTime { get; set; } = string.Empty;
        public int Status { get; set; } = 0;
        public string StatusDescription { get; set; } = string.Empty;
        public double Weight { get; set; } = 0;
        public int GreenPoints { get; set; } = 0;

        public string CreatedAt { get; set; }

        public string UpdatedAt { get; set; }

        public string CreationDate { get; set; }
    }
}