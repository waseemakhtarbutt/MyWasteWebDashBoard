using DrTech.Models.Common;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models
{
   public class PlantTree : BaseModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonIgnore]
        public IFormFile File { get; set; }
        public string FileName { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Season { get; set; } = "";

        public string Type { get; set; } = "";


    }
}
