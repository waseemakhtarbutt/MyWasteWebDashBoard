//using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
    public  class UserViewModel
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public Nullable<int> UserTypeID { get; set; }
        public Nullable<int> RoleID { get; set; }
        public string Password { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public Nullable<int> GreenPoints { get; set; }
        public string FileName { get; set; }
        public string QRCode { get; set; }
        public string DeviceID { get; set; }
        public string OSType { get; set; }
        public string SocialMediaKey { get; set; }
        public string SocialMediaName { get; set; }
        public string Status { get; set; }
        public Nullable<int> CityId { get; set; }
        public Nullable<bool> IsVerified { get; set; }
        public Nullable<bool> SMSCodeRequired { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }

       // public IFormFile File { get; set; }

        public int UserID { get; set; }
    }
}
