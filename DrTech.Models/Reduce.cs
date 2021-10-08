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
    public class Reduce : BaseModel
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        public string FileName { get; set; } = "";

        public string Idea { get; set; } = "";

        [BsonIgnore]
        public IFormFile File { get; set; }

       // [BsonIgnore]
        public double Longitude { get; set; } = 0;
      //  [BsonIgnore]
        public double Latitude { get; set; } = 0;

        public int GreenPoints { get; set; } = 0;

        public double Status { get; set; } = 0;
        public string StatusDescription { get; set; } = string.Empty;


        [BsonIgnore]
        public string UserId { get; set; } = string.Empty;

    }
}
