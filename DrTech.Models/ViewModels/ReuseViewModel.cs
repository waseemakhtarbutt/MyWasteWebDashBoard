using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models.ViewModels
{
    public class ReuseViewModel
    {
        public string Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Type { get; set; }
        public string TypeDescription { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string DeliveryDate { get; set; }
        public int DonateTo { get; set; }
        public string DonateToDescription { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public int City { get; set; }
        public string CityDescription { get; set; }
        
        public IFormFile File { get; set; }
        public int  GreenPoints { get; set; }
    }
}