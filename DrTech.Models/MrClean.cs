using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using DrTech.Models.Common;
using Newtonsoft.Json;
//using Microsoft.AspNetCore.Http;



namespace DrTech.Models
{
    [BsonDiscriminator("MrClean")]
    public class MrClean : BaseModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string CollectorDateTime { get; set; } = string.Empty;
        public int Status { get; set; } = 0;
        public string StatusDescription { get; set; } = string.Empty;
        public double Weight { get; set; } = 0;
        public int GreenPoints { get; set; } = 0;

        public DateTime CreationDate { get; set; }

    }
}
