using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmalForLife.Models
{
    public class MReduce
    {

        public string FileName { get; set; } = "";

        public string Idea { get; set; } = "";


        public double Longitude { get; set; } = 0;
        public double Latitude { get; set; } = 0;

        public int GreenPoints { get; set; } = 0;

        public double Status { get; set; } = 0;
        public string StatusDescription { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}