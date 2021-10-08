using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrTech.Amal.SQLServices.Models
{
    public class MSchedule
    {
        public int AreaID;
        public string AreaName;
        public string DriverName;
        public int DriverID;
        public DateTime Date;
        public DateTime fTime;
        public DateTime tTime;
        public string FromTime;
        public string ToTime;
        public string Day;
        public bool Active;
        public string Status;
        public int CityID;
    }
}