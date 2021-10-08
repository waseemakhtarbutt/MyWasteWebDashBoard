using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmalForLife.Models
{
    public class MReport
    {
        public double Longitude { get; set; } = 0;
        public double Latitude { get; set; } = 0;
        public string FileName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int Priority { get; set; } = 0;
        public int Status { get; set; } = 0;
        public string StatusDescription { get; set; } = string.Empty;

        public int GreenPoints { get; set; } = 0;

        public string UserId { get; set; } = string.Empty;
    }
}