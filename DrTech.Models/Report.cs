using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using DrTech.Models.Common;
using Newtonsoft.Json;
//using Microsoft.AspNetCore.Http;



namespace DrTech.Models
{
    [BsonDiscriminator("Report")]
    public class Report : BaseModel
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public double Longitude { get; set; } = 0;
        public double Latitude { get; set; } = 0;

        [BsonIgnore]
        public IFormFile File { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int Priority { get; set; } = 0;
        public int Status { get; set; } = 0;
        public string StatusDescription { get; set; } = string.Empty;

        public int GreenPoints { get; set; } = 0;
        
        public string UserId { get; set; } = string.Empty;
    }
}
