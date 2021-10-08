using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models.ViewModels
{
    public class MapMarker
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Type { get; set; }
        public string PinImage { get; set; }
        public string Label { get; set; }
        public double GreenPoints { get; set; }
        public double Cash { get; set; }
        public string Status { get; set; }
        public string FileName { get; set; }

        public int PlantCount { get; set; }
        public string Description { get; set; }
    }
}
