using DrTech.Amal.SQLServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmalForLife.Models
{
    public class MUser
    {
        public string Id { get; set; }
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

        public string Status { get; set; } = String.Empty;

        public string datetime { get; set; } = String.Empty;

        public int City { get; set; } = 0;

        public string FileName { get; set; } = "";
        public DateTime createdate { get; set; }

        public List<MReplant> Replant { get; set; } = new List<MReplant>();

        public List<MReuse> Reuse { get; set; } = new List<MReuse>();

        public List<MRefuse> Refuse { get; set; } = new List<MRefuse>();

        public IList<MBuyBin> BuyBinDetails { get; set; } = new List<MBuyBin>();

        public List<MReduce> Reduse { get; set; } = new List<MReduce>();

        public List<MReport> Report { get; set; } = new List<MReport>();
        public List<MSocialMedia> SocialMedia { get; set; } = new List<MSocialMedia>();
        public string CityDescription { get; set; }

        public string QRCode { get; set; }
        public string DeviceId { get; set; } = string.Empty;
        public string OSType { get; set; } = string.Empty;
        public string SocialMediaId { get; set; } = string.Empty;
        public bool IsVerified { get; set; } = false;

        public bool SMSCodeRequired { get; set; } = false;
    }
}