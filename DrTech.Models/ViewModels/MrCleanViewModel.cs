using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models.ViewModels
{
    public class MrCleanViewModel
    {
        public string  Id { get; set; }
        //[BsonElement("FullName")]

        //[BsonElement] Id ID
        public string FileName { get; set; }
        //[BsonElement]
        public string Description { get; set; }
        //[BsonElement]
        public string UserId { get; set; }
        //[BsonElement]

        public string CollectorDateTime { get; set; }

        public int Status { get; set; }
        public string StatusDescription { get; set; }

        public double Weight { get; set; }

        public double Longitude { get; set; } = 0;
        public double Latitude { get; set; } = 0;

    }
}
