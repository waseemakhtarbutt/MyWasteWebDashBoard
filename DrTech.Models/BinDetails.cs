using DrTech.Models.Common;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models
{
    [BsonDiscriminator("BinDetails")]
    public class BinDetails : BaseModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Capacity { get; set; } 
        public string FileName { get; set; }
        public double Price { get; set; }
        public string QRCode { get; set; }
        public string BinName { get; set; }
        public string Description { get; set; }
    }
}
