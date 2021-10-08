using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
    public class TokenViewModel
    {
        public int ID { get; set; } = 0;
        public string FullName { get; set; } = "";
        public string FileName { get; set; } = "";
        public string Token { get; set; } = "";
        public string Phone { get; set; } = "";
        public decimal Longitude { get; set; } = 0;
        public decimal Latitude { get; set; } = 0;
        public string Address { get; set; } = "";
        public string Email { get; set; } = "";
        public decimal PaidAmount { get; set; } = 0;
        public decimal? RemainingAmount { get; set; } = 0;
        public string QRCode { get; set; } = "";
        public Int32 GreenPoints { get; set; } = 0;
        public int? CityId { get; set; } = 0;
        public string CityDescription { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int RoleID { get; set; } = 0;
        public int SMSCode { get; set; } = 0;
        public bool IsVerified { get; set; }
        public string City { get; set; }
        public string DeviceToken { get; set; }
        public int ChildrenCount { get; set; }
        public int MembersCount { get; set; }
        public int EmployeesCount { get; set; }
        public  int? AreaID { get; set; }
        public string AreaName { get; set; }
        public string UnionCouncil { get; set; }
        public int CurrentMonthGP { get; set; }
        public int? CompanyID { get; set; }
        public string BusinessKey { get; set; }
        public string Type { get; set; }
        public bool IsConnectedWithGPN { get; set; }
        public string GPNConnectionMessage { get; set; }
        public decimal? RedeemedPoints { get; set; }
        public decimal? RedeemablePoints { get; set; }
        public decimal? WalletBalance { get; set; }
        public string FacebookKey { get; set; }
        public string SocailMediaName { get; set; }
        public string SocialMediaKey { get; set; }
        public DateTime DueDate { get; set; }

    }
}
