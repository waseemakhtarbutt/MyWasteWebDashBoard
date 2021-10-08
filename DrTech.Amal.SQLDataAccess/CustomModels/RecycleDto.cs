using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
  public class RecycleDto
    {
        public int ID { get; set; }
        public string statusDescription { get; set; }
        public int userId { get; set; }
        public string userName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? Fdate { get; set; }
        public string CityName { get; set; }
        public string areaName { get; set; }
        public string Address { get; set; }
        public string collectorDateTime { get; set; }
        public string CreateDate { get; set; }
        public DateTime? collectionDate { get; set; }
    }
}
