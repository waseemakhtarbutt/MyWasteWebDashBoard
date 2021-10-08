using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models
{
    public class DonationType
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonIgnore]
        public IFormFile File { get; set; }
        public string FileName { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Type { get; set; } = "";

    }
}
