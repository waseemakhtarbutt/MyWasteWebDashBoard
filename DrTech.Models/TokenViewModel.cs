using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models
{
    public class TokenViewModel
    {
        public string UserId { get; set; } = "";
        public string FullName { get; set; } = "";

        public string FileName { get; set; } = "";
        public string Token { get; set; } = "";
        public string Phone { get; set; } = "";
        public double Longitude { get; set; } = 0;
        public double Latitude { get; set; } = 0;
        public string Address { get; set; } = "";
        public string Email { get; set; } = "";

        public string QRCode { get; set; } = "";

        public double GreenPoints { get; set; } = 0;

        public int City { get; set; } = 0;

        public string CityDescription { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "";

        public int SMSCode { get; set; } = 0;

        public bool IsVerified { get; set; }
    }
}
