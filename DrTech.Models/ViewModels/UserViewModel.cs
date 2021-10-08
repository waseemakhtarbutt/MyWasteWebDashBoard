using Microsoft.AspNetCore.Http;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models.ViewModels
{
    public class UserViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        public IFormFile File { get; set; }
        public string FileName { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public double Longitude { get; set; } = 0;
        public double Latitude { get; set; } = 0;

        public string OSType { get; set; } = string.Empty;

        public string SocialMediaId { get; set; } = string.Empty;

        public string SocialMediaType { get; set; } = string.Empty;

        public string DeviceId { get; set; } = string.Empty;

        public int City { get; set; } = 0;

        public double GreenPoints { get; set; } = 0;

        public string CityDescription { get; set; } = string.Empty;

        public bool IsVerified { get; set; } = false;

        public bool SMSCodeRequired { get; set; } = false;

    }
}
