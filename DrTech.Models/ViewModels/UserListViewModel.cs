using Microsoft.AspNetCore.Http;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models.ViewModels
{
    public class UserListViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public double Longitude { get; set; } = 0;
        public double Latitude { get; set; } = 0;
        public string SocialMediaType { get; set; } = string.Empty;

        public string MemberSince { get; set; }

        public double GreenPoints { get; set; } = 0;

        public int ReplantCount { get; set; } = 0;
        public int ReuseCount { get; set; } = 0;
        public int RefuseCount { get; set; } = 0;
        public int BinCount { get; set; } = 0;
        public int RecycleCount { get; set; } = 0;
        public int ReduceCount { get; set; } = 0;
        public int ReportCount { get; set; } = 0;
        public int RegiftCount { get; set; } = 0;



    }
}
