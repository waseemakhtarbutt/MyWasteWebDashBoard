using DrTech.Models.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models
{
    //[BsonDiscriminator("BuyBin")]
    public class BuyBin : BaseModel
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public string BinId { get; set; } = string.Empty;
        public int Status { get; set; } = 0;
        public string StatusDescription { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;

        public int Qty { get; set; } = 0;
        public string TrackingNumber { get; set; } = "";
        public string FileName { get; set; }
        public double Price { get; set; }
        public string BinName { get; set; }




    }
}
