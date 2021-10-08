using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models
{
    public class Disclaimer
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string DisclaimerHeading { get; set; } = "";
        public string DisclaimerText { get; set; } = "";
    }
}
