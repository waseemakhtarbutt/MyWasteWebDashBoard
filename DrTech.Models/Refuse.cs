using DrTech.Models.Common;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models
{
    public class Refuse : BaseModel
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public string FileName { get; set; } = "";
        public string Idea { get; set; } = "";
        [BsonIgnore]
        public IFormFile File { get; set; }
        public double Longitude { get; set; } = 0;
        public double Latitude { get; set; } = 0;
        public double Status { get; set; } = 0;
        public string StatusDescription { get; set; } = string.Empty;
        public int GreenPoints { get; set; } = 0;

        [BsonIgnore]
        public string UserId { get; set; } = string.Empty;

    }
}
