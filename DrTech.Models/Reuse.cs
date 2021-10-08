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
    public class Reuse : BaseModel
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        [BsonIgnore]
        public IFormFile File { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Idea { get; set; } = string.Empty;
        public int GreenPoints { get; set; } = 0;

        // [BsonIgnore]
        // public string UserId { get; set; } = string.Empty;

        //add
        public int Type { get; set; } = 0;
        public string TypeDescription { get; set; } = string.Empty;
        public int SubType { get; set; } = 0;
        public string SubTypeDescription { get; set; } = string.Empty;
        public string RecieptFileName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string DeliveryDate { get; set; } = string.Empty;
        public int DonateTo { get; set; } = 0;
        public string DonateToDescription { get; set; } = string.Empty;
        public int Status { get; set; } = 0;
        public string StatusDescription { get; set; } = string.Empty;
        public int City { get; set; } = 0;
        public string CityDescription { get; set; } = string.Empty;
        public int AgeGroup { get; set; } = 0;
        public string AgeGroupDescription { get; set; } = string.Empty;
        public string NeedType { get; set; } = string.Empty;
        public string RegiftId { get; set; } = string.Empty;


        [BsonIgnore]
        public string SubTypeTitle { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;

        public double Longitude { get; set; } = 0;
        public double Latitude { get; set; } = 0;
    }
}
