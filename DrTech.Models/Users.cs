using DrTech.Models.Common;
using DrTech.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static DrTech.Common.Extentions.Constants;

namespace DrTech.Models
{
    [BsonDiscriminator("Users")]
    public class Users : BaseModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string FullName { get; set; } = "";
        public string Phone { get; set; } = "";
        //[BsonElement]
        public string Address { get; set; } = "";
        //[BsonElement]
        public string Email { get; set; } = "";
        public int UserTypeId { get; set; } = 0;
        public string UserRole { get; set; } = "";
        public bool IsActive { get; set; } = false;
        public string Password { get; set; } = "";
        public double Longitude { get; set; } = 0;
        public double Latitude { get; set; } = 0;
        public double GreenPoints { get; set; } = 0;
        public double Cash { get; set; } = 0;

        [BsonElement(CollectionNames.BUYBIN)]
        public List<BuyBin> BuyBinDetails { get; set; } = new List<BuyBin>();

        [BsonElement(CollectionNames.REPLANT)]
        public List<Replant> Replant { get; set; } = new List<Replant>();

        [BsonElement(CollectionNames.Refuse)]
        public List<Refuse> Refuse { get; set; } = new List<Refuse>();

        [BsonElement(CollectionNames.Reduce)]
        public List<Reduce> Reduce { get; set; } = new List<Reduce>();

        [BsonElement(CollectionNames.Report)]
        public List<Report> Report { get; set; } = new List<Report>();

        public string FileName { get; set; } = "";

        public string QRCode { get; set; } = "";

        [BsonElement(CollectionNames.REUSE)]
        public List<Reuse> Reuse { get; set; } = new List<Reuse>();

        public string DeviceId { get; set; } = string.Empty;
        public string OSType { get; set; } = string.Empty;
        public string SocialMediaId { get; set; } = string.Empty;


        [BsonElement(CollectionNames.SocialMedia)]
        public List<SocialMedia> SocialMedia { get; set; } = new List<SocialMedia>();

        public string Status { get; set; } = String.Empty;

        public string datetime { get; set; } = String.Empty;

        public int City { get; set; } = 0;
        public string CityDescription { get; set; } = string.Empty;

        [BsonElement(CollectionNames.Child)]
        public List<Child> children { get; set; } = new List<Child>();

        [BsonElement(CollectionNames.Employee)]
        public List<Employment> Employments { get; set; } = new List<Employment>();

        [BsonElement(CollectionNames.MEMBERS)]
        public List<Members> Memberships { get; set; } = new List<Members>();

        public Int32 SMSCode { get; set; } = 0;

        public bool IsVerified { get; set; } = false;

        public bool SMSCodeRequired { get; set; } = false;

        //  public IFormFile File { get; set; } 

        //  public string UserId { get; set; } = string.Empty;

        // public string SocialMediaType { get; set; } = string.Empty;


    }
}
