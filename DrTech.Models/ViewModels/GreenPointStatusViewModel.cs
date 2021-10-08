using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models.ViewModels
{
    public class GreenPointStatusViewModel
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double GreenPoints { get; set; }
        public double Cash { get; set; }
        public string Status { get; set; }
    }
}
