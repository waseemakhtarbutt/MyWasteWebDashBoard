using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models.ViewModels
{
    public class ReplantViewModel
    {
        public string Id { get; set; } = "";

        public double Longitude { get; set; } = 0;
        public double Latitude { get; set; } = 0;
        public string FileName { get; set; } = "";
        public string Description { get; set; } = "";
        public string UserId { get; set; } = "";
        public int TreeCount { get; set; } = 0;
        public string Height { get; set; } = "";
        public string Reminder { get; set; } = "";
        public int PlantName { get; set; } = 0;
        public string PlantNameDescription { get; set; } = "";
        public int Status { get; set; } = 0;
        public int GreenPoints { get; set; } = 0;
        public List<ReplantViewModel> Child { get; set; }
    }
}
