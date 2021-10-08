using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models
{
    public class Location
    {
        public double Longitude { get; set; } = 0;
        //[BsonElement]
        public double Latitude { get; set; } = 0;
    }
}
