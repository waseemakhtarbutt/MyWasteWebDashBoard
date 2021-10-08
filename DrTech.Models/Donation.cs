using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.AspNetCore.Http;
using DrTech.Models.Common;

namespace DrTech.Models
{
    [BsonIgnoreExtraElements]
    public class Reuse11 : BaseModel
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public double Longitude { get; set; } = 0;
        public double Latitude { get; set; } = 0;

        //[BsonIgnore]
        //public IFormFile File { get; set; }
        public int Type { get; set; } = 0;
        public string TypeDescription { get; set; } = string.Empty;
        public int SubType { get; set; } = 0;
        public string SubTypeDescription { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
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

        public string NeedType { get; set; }= string.Empty;
    }
}