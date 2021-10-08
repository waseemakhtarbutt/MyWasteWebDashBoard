using DrTech.Models.Common;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace DrTech.Models
{
    //[BsonIgnoreExtraElements]
    public class Replant : BaseModel
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        public double Longitude { get; set; } = 0;
        public double Latitude { get; set; } = 0;
        [BsonIgnore]
        public IFormFile File { get; set; }
        public string FileName { get; set; } = "";
        public int PlantName { get; set; } = 0;
        public string PlantNameDescription { get; set; } = "";
        public string Description { get; set; } = "";
        public int TreeCount { get; set; } = 0;
        public string Height { get; set; } = "";
        public string Reminder { get; set; } = "";
        public string Parent { get; set; } = "";
        public int Status { get; set; } = 0;
        public string StatusDescription { get; set; } = "";
        public int GreenPoints { get; set; } = 0;

      //  [BsonIgnore]
        public string UserId { get; set; } = string.Empty;

    }
}