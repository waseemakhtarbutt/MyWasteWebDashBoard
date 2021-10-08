using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models.ViewModels
{
    public class UserDetailViewModel
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string FileName { get; set; }
        public string UserId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string MemberSince { get; set; }

        public string UserType { get; set; }

        public double GreenPoints { get; set; } = 0;

        public int RefuseCount { get; set; } = 0;
        public int ReduceCount { get; set; } = 0;
        public int ReuseCount { get; set; } = 0;
        public int RegiftCount { get; set; } = 0;
        public int ReportCount { get; set; } = 0;
        public int RecycleCount { get; set; } = 0;
        public int ReplantCount { get; set; } = 0;
        public int BinCount { get; set; } = 0;
    }
}
