using System;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
    public class DriverTokenViewModel
    {
        public int ID { get; set; } = 0;
        public string FullName { get; set; } = "";
        public string FileName { get; set; } = "";
        public string Token { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public string Email { get; set; } = "";
        public int? CityID { get; set; }
        public string PIN { get; set; } = "";
        public int? VehicleId { get; set; }
        public string RegNumber { get; set; } = "";
        public string LicenseFileName { get; set; } = "";
    }
}
