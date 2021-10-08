using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models.ViewModels
{
    public class BinDetailsViewModel
    {
        public IFormFile File { get; set; }

        public string Capacity { get; set; }
        //[BsonElement]
        public string FileName { get; set; }
        //[BsonElement]
        public double Price { get; set; }

        public string QRCode { get; set; }
        //[BsonElement]

        public string BinName { get; set; }
        //[BsonElement]

        public string Description { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }
    }
}
