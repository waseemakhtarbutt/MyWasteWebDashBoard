using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmalForLife.Models
{
    public class MReplant
    {
        public double Longitude { get; set; } = 0;
        public double Latitude { get; set; } = 0;
        public string FileName { get; set; } = "";
        public int PlantName { get; set; } = 0;
        public string PlantNameDescription { get; set; } = "";
        public string Description { get; set; } = "";
        public int TreeCount { get; set; } = 0;
        public string Height { get; set; } = "";
        public string Reminder { get; set; } = "";
        public string Parent { get; set; } = "";
        public int Status { get; set; } = 0;
        public string StatusDescription { get; set; } = "";
        public int GreenPoints { get; set; } = 0;
        public string UserId { get; set; } = string.Empty;


    }
}